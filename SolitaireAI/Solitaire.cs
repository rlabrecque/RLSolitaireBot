using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.CV.ML;
using Emgu.CV.ML.MlEnum;
using System.Linq;

namespace SolitaireAI {
	public enum Number {
		UNKNOWN = -1,

		A,
		Two,
		Three,
		Four,
		Five,
		Six,
		Seven,
		Eight,
		Nine,
		Ten,
		Jack,
		Queen,
		King,

		TOTAL
	}

	public enum Suit {
		UNKNOWN = -1,

		Diamond = 0, // Red
		Heart,       // Red
		Spade,       // Black
		Club,        // Black

		TOTAL
	}

	public static class CvScalarColor {
		public static readonly MCvScalar Red = new Bgr(Color.Red).MCvScalar;
		public static readonly MCvScalar Green = new Bgr(Color.Green).MCvScalar;
		public static readonly MCvScalar Blue = new Bgr(Color.Blue).MCvScalar;
	}

	public static class ScalarArrayColor {
		public static readonly ScalarArray HsvGreenLowerBound = new ScalarArray(new MCvScalar(75, 25, 25));
		public static readonly ScalarArray HsvGreenUpperBound = new ScalarArray(new MCvScalar(190, 256, 256));
	}

	public enum Action {
		Idle,
		DoubleClick,
	}

	public struct BoardState {
		public List<Card> m_Stock;
		public List<Card> m_Waste;
		public List<Card>[] m_Foundation;
		public List<Card>[] m_Tableau;
	}

	public struct Card {
		public Number m_Number;
		public Suit m_Suit;
	}

	public struct VisualState {
		public bool m_bCardsInStock;
		public Card m_CardInWaste;
		public Card[] m_Foundation;
		public Card[] m_Tableau;
	}

	public struct ReferenceImages {
		public Mat[] m_Numbers;
		public Mat[] m_Suits;
	}
	
	public class SolitaireInfo : IBotInfo {
		public override string ToString() {
			return "Solitaire";
		}

		public string GetExecutableName { get { return "Solitaire.exe"; } }
		public string Author { get { return "Riley Labrecque"; } }
		public IBot GetBot { get { return new Solitaire(); } }
	}

	public class Solitaire : IBot {
		// Values for the biggest window size: Width: 1920, Height: 1080
		const int cardWidth = 141;
		const int cardHeight = 191;
		const int cardSpacing = 73;
		const int leftOffset = 248;
		const int topOffsetRow1 = 47;
		const int topOffsetRow2 = 295;
		const int foundationSlots = 4;
		const int tableauSlots = 7;

		Bitmap m_DebugReturn;
		Action m_CurrentAction;
		BoardState m_BoardState;
		VisualState m_VisualState;
		ReferenceImages m_ReferenceImages;

		KNearest m_NumbersKNN;
		KNearest m_SuitsKNN;

		public KNearest CreateAndTrainKNN(Mat[] referenceImages) {
			using (Mat samples = new Mat())
			using (Matrix<float> responses = new Matrix<float>(referenceImages.Length, 1)) {

				for (int i = 0; i < referenceImages.Length; ++i) {
					responses[i, 0] = i;

					using (Mat img = referenceImages[i])
					using (Mat sample = img.Reshape(0, 1)) {
						samples.PushBack(sample);
					}
				}

				using (Mat samples2 = new Mat()) {
					samples.ConvertTo(samples2, DepthType.Cv32F);

					KNearest knn = new KNearest();
					knn.DefaultK = 1;
					knn.IsClassifier = true;
					knn.Train(samples2, DataLayoutType.RowSample, responses);
					return knn;
				}
			}
		}

		public int GetImageSimilarityFromKNN(KNearest knn, Mat sample) {
			using (Mat sample2 = sample.Reshape(0, 1)) {
				sample2.ConvertTo(sample2, DepthType.Cv32F);
				return (int)knn.Predict(sample2); //knn.FindNearest(sample, K, results, null, neighborResponses, null);
			}
		}

		public override void OnAttach() {
			m_BoardState.m_Stock = new List<Card>(24);
			m_BoardState.m_Waste = new List<Card>(0);
			m_BoardState.m_Foundation = new List<Card>[foundationSlots];
			m_BoardState.m_Tableau = new List<Card>[tableauSlots];

			for (int i = 0; i < m_BoardState.m_Foundation.Length; ++i) {
				m_BoardState.m_Foundation[i] = new List<Card>(0);
			}

			for (int i = 0; i < m_BoardState.m_Tableau.Length; ++i) {
				m_BoardState.m_Tableau[i] = new List<Card>(i);
			}

			m_VisualState.m_Foundation = new Card[foundationSlots];
			m_VisualState.m_Tableau = new Card[tableauSlots];

			m_ReferenceImages.m_Numbers = new Mat[(int)Number.TOTAL];
			m_ReferenceImages.m_Suits = new Mat[(int)Suit.TOTAL];

			for (int i = 0; i < (int)Number.TOTAL; ++i) {
				string filename = String.Format("ReferenceImages/Numbers/{0}.bmp", (Number)i);
				m_ReferenceImages.m_Numbers[i] = CvInvoke.Imread(filename, ImreadModes.Grayscale);
			}

			for (int i = 0; i < (int)Suit.TOTAL; ++i) {
				string filename = String.Format("ReferenceImages/Suits/{0}.bmp", (Suit)i);
				m_ReferenceImages.m_Suits[i] = CvInvoke.Imread(filename, ImreadModes.Grayscale);
			}

			m_NumbersKNN = CreateAndTrainKNN(m_ReferenceImages.m_Numbers);
			m_SuitsKNN = CreateAndTrainKNN(m_ReferenceImages.m_Suits);
		}

		public override void OnDetach() {
			m_NumbersKNN.Dispose();
			for (int i = 0; i < (int)Number.TOTAL; ++i) {
				m_ReferenceImages.m_Numbers[i].Dispose();
			}

			m_SuitsKNN.Dispose();
			for (int i = 0; i < (int)Suit.TOTAL; ++i) {
				m_ReferenceImages.m_Suits[i].Dispose();
			}
		}

		public override Bitmap OnGameFrame(byte[] data, Size size, int stride) {
			if (m_CurrentAction != Action.Idle) {
				return null;
			}

			using (Mat img = Util.ByteArrayToMat(data, size, stride))
			using (Mat imgHsv = new Mat()) {
				CvInvoke.CvtColor(img, imgHsv, ColorConversion.Bgr2Hsv);

				// Stock
				{
					var rect = new Rectangle(leftOffset, topOffsetRow1, cardWidth, cardHeight);

					using (Mat imgHsvRoi = new Mat(imgHsv, rect)) {
						m_VisualState.m_bCardsInStock = IsCard(imgHsvRoi);
					}

					CvInvoke.Rectangle(img, rect, CvScalarColor.Blue);
				}

				// Waste
				{
					const int startX = leftOffset + (cardWidth * 2) + (cardSpacing * 2);
					const int stopX = leftOffset + (cardWidth * 2) + cardSpacing;
					const int centerY = topOffsetRow1 + (cardHeight / 2);

					int rightEdge = stopX;
					for (int x = startX; x > stopX; --x) {
						using (Mat pixel = new Mat(imgHsv, new Rectangle(x, centerY, 1, 1))) {
							if (IsCard(pixel)) {
								break;
							}
						}
						rightEdge = x;
					}

					CvInvoke.Line(img, new Point(startX, centerY), new Point(rightEdge, centerY), CvScalarColor.Red);

					var rect = new Rectangle(rightEdge - cardWidth, topOffsetRow1, cardWidth, cardHeight);

					using (Mat card = new Mat(img, rect)) {
						m_VisualState.m_CardInWaste = ParseCard(card);
					}

					CvInvoke.Rectangle(img, rect, CvScalarColor.Blue);
				}

				// Foundation
				for (int i = 0; i < foundationSlots; ++i) {
					var rect = new Rectangle(leftOffset + (cardWidth * (3 + i)) + (cardSpacing * (3 + i)), topOffsetRow1, cardWidth, cardHeight);

					using (Mat card = new Mat(img, rect)) {
						m_VisualState.m_Foundation[i] = ParseCard(card);
					}

					CvInvoke.Rectangle(img, rect, CvScalarColor.Blue);
				}

				// Tableau
				for (int i = 0; i < tableauSlots; ++i) {
					int startHeight = img.Height - 1;
					int stopHeight = topOffsetRow2 + cardHeight;
					int cardCenterX = leftOffset + (cardWidth / 2) + ((cardWidth + cardSpacing) * i);

					int bottomEdge = stopHeight;
					for (int y = startHeight; y > stopHeight; --y) {
						using (Mat pixel = new Mat(imgHsv, new Rectangle(cardCenterX, y, 1, 1))) {
							if (IsCard(pixel)) {
								break;
							}
						}
						bottomEdge = y;
					}

					CvInvoke.Line(img, new Point(cardCenterX, img.Height - 1), new Point(cardCenterX, bottomEdge), CvScalarColor.Red);

					var rect = new Rectangle(cardCenterX - (cardWidth / 2), bottomEdge - cardHeight, cardWidth, cardHeight);

					using (Mat card = new Mat(img, rect)) {
						m_VisualState.m_Tableau[i] = ParseCard(card);
					}

					CvInvoke.Rectangle(img, rect, CvScalarColor.Blue);
				}

				return m_DebugReturn ?? img.CopyToBitmap();
			}
		}

		Card ParseCard(Mat card) {
			// First check if a card exists at this location
			using (Mat cardHsv = new Mat()) {
				CvInvoke.CvtColor(card, cardHsv, ColorConversion.Bgr2Hsv);

				if (!IsCard(cardHsv)) {
					return new Card() { m_Number = Number.UNKNOWN };
				}
			}

			// Get Suit
			Suit suit = Suit.UNKNOWN;
			{
				Rectangle rect = new Rectangle(4, 31, 22, 26);
				using (Mat gray = new Mat(card, rect)) {
					CvInvoke.CvtColor(gray, gray, ColorConversion.Bgr2Gray);
					CvInvoke.Threshold(gray, gray, 200, 255, ThresholdType.Binary);

					suit = (Suit)GetImageSimilarityFromKNN(m_SuitsKNN, gray);
				}

				CvInvoke.Rectangle(card, rect, CvScalarColor.Green);
			}

			// Get Number
			Number number = Number.UNKNOWN;
			{
				Rectangle rect = new Rectangle(3, 3, 27, 27);
				using (Mat gray = new Mat(card, rect)) {
					CvInvoke.CvtColor(gray, gray, ColorConversion.Bgr2Gray);
					CvInvoke.Threshold(gray, gray, 200, 255, ThresholdType.Binary);

					number = (Number)GetImageSimilarityFromKNN(m_NumbersKNN, gray);
				}

				CvInvoke.Rectangle(card, rect, CvScalarColor.Green);
			}

			return new Card() { m_Number = number, m_Suit = suit };
		}
		
		public bool IsCard(Mat img) {
			using (Mat thresh = new Mat()) {
				ScalarArray lower = ScalarArrayColor.HsvGreenLowerBound;
				ScalarArray upper = ScalarArrayColor.HsvGreenUpperBound;
				CvInvoke.InRange(img, lower, upper, thresh);
				return (CvInvoke.Mean(thresh).V0 < 200);
			}
		}

		public override string GetState() {
			StringBuilder state = new StringBuilder(250);
			state.Append("Current Action: ");
			state.AppendLine(m_CurrentAction.ToString());
			state.AppendLine();
			state.AppendLine("Visual State:");
			state.AppendLine("----------------");
			state.Append("Cards In Stock: ");
			state.AppendLine(m_VisualState.m_bCardsInStock.ToString());

			state.Append("Card In Waste: ");
			if (m_VisualState.m_CardInWaste.m_Number != Number.UNKNOWN) {
				state.Append(m_VisualState.m_CardInWaste.m_Number);
				state.Append(" ");
				state.AppendLine(m_VisualState.m_CardInWaste.m_Suit.ToString());
			}
			else {
				state.AppendLine("----");
			}

			state.AppendLine("Foundation: ");
			foreach (Card card in m_VisualState.m_Foundation) {
				if (card.m_Number != Number.UNKNOWN) {
					state.Append(card.m_Number);
					state.Append(" ");
					state.AppendLine(card.m_Suit.ToString());
				}
				else {
					state.AppendLine("----");
				}
			}

			state.AppendLine("Tableau: ");
			foreach (Card card in m_VisualState.m_Tableau) {
				if (card.m_Number != Number.UNKNOWN) {
					state.Append(card.m_Number);
					state.Append(" ");
					state.AppendLine(card.m_Suit.ToString());

				}
				else {
					state.AppendLine("----");
				}
			}

			return state.ToString();
		}
	}
}
