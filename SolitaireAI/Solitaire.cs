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
		public static readonly ScalarArray HsvGreenLowerBound = new ScalarArray(new MCvScalar(55, 25, 25));
		public static readonly ScalarArray HsvGreenUpperBound = new ScalarArray(new MCvScalar(180, 256, 256));
	}

	/*public enum Action {
		Idle,
		DoubleClick,
	}

	public struct BoardState {
		public List<Card> m_Stock;
		public List<Card> m_Waste;
		public List<Card>[] m_Foundation;
		public List<Card>[] m_Tableau;
	}*/

	public struct Card {
		public Number m_Number;
		public Suit m_Suit;

		public override string ToString() {
			return m_Number + " " + m_Suit;
		}
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

		const bool m_StepThink = true;
		Bitmap m_DebugReturn;
		//Action m_CurrentAction;
		//BoardState m_BoardState;
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

		public int GetImageSimilarityFromKNN(KNearest knn, Mat sample, Rectangle roi) {
			using (Mat sample2 = new Mat(sample, roi)) {
				CvInvoke.CvtColor(sample2, sample2, ColorConversion.Bgr2Gray);
				CvInvoke.Threshold(sample2, sample2, 200, 255, ThresholdType.Binary);
				CvInvoke.GaussianBlur(sample2, sample2, new Size(3, 3), 0);
				sample2.ConvertTo(sample2, DepthType.Cv32F);
				//gray.CopyToBitmap().Save("ReferenceImages/_.bmp");

				using (Mat sample3 = sample2.Reshape(0, 1)) {
					return (int)knn.Predict(sample3); //knn.FindNearest(sample, K, results, null, neighborResponses, null);
				}
			}
		}

		public override void OnAttach() {
			/*m_BoardState.m_Stock = new List<Card>(24);
			m_BoardState.m_Waste = new List<Card>(0);
			m_BoardState.m_Foundation = new List<Card>[foundationSlots];
			m_BoardState.m_Tableau = new List<Card>[tableauSlots];

			for (int i = 0; i < m_BoardState.m_Foundation.Length; ++i) {
				m_BoardState.m_Foundation[i] = new List<Card>(0);
			}

			for (int i = 0; i < m_BoardState.m_Tableau.Length; ++i) {
				m_BoardState.m_Tableau[i] = new List<Card>(i);
			}*/

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
			using (Mat img = Util.ByteArrayToMat(data, size, stride)) {
				// Stock
				{
					var rect = new Rectangle(leftOffset, topOffsetRow1, cardWidth, cardHeight);

					using (Mat roi = new Mat(img, rect)) {
						m_VisualState.m_bCardsInStock = IsCard(roi);
					}

					CvInvoke.Rectangle(img, rect, CvScalarColor.Blue);
				}

				// Waste
				{
					const int stopX = leftOffset + (cardWidth * 2) + cardSpacing;
					const int centerY = topOffsetRow1 + (cardHeight / 2);
					
					int rightEdge = stopX;
					using (Mat slice = new Mat(img, new Rectangle(stopX, centerY, cardSpacing, 1))) {
						rightEdge += GetEdgeOfCard(slice);
					}

					CvInvoke.Line(img, new Point(rightEdge, centerY), new Point(stopX + cardSpacing, centerY), CvScalarColor.Red);

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
					int stopY = topOffsetRow2 + cardHeight;
					int numPixelsY = img.Height - stopY - 1;
					int cardCenterX = leftOffset + (cardWidth / 2) + ((cardWidth + cardSpacing) * i);

					int bottomEdge = stopY;
					using (Mat slice = new Mat(img, new Rectangle(cardCenterX, stopY, 1, numPixelsY))) {
						bottomEdge += GetEdgeOfCard(slice);
					}

					CvInvoke.Line(img, new Point(cardCenterX, bottomEdge), new Point(cardCenterX, stopY + numPixelsY), CvScalarColor.Red);

					int leftEdge = cardCenterX - (cardWidth / 2);
					int topEdge = bottomEdge - cardHeight;
					var rect = new Rectangle(leftEdge, topEdge, cardWidth, cardHeight);

					using (Mat card = new Mat(img, rect)) {
						m_VisualState.m_Tableau[i] = ParseCard(card);
					}

					CvInvoke.Rectangle(img, rect, CvScalarColor.Blue);
				}

				Think();

				return m_DebugReturn ?? img.CopyToBitmap();
			}
		}

		Card ParseCard(Mat card) {
			// First check if a card exists at this location
			if (!IsCard(card)) {
				return new Card() { m_Number = Number.UNKNOWN, m_Suit = Suit.UNKNOWN };
			}

			// Get Suit
			Rectangle rect = new Rectangle(4, 31, 22, 26);
			Suit suit = (Suit)GetImageSimilarityFromKNN(m_SuitsKNN, card, rect);
			CvInvoke.Rectangle(card, rect, CvScalarColor.Green);

			// Get Number
			rect = new Rectangle(3, 3, 27, 27);
			Number number = (Number)GetImageSimilarityFromKNN(m_NumbersKNN, card, rect);
			CvInvoke.Rectangle(card, rect, CvScalarColor.Green);

			return new Card() { m_Number = number, m_Suit = suit };
		}

		public int GetEdgeOfCard(Mat mat) {
			using (var img = mat.ToImage<Bgr, byte>()) {
				for (int row = mat.Rows - 1; row >= 0; --row) {
					for (int col = mat.Cols - 1; col >= 0; --col) {
						var pixel = img[row, col];
						if ((pixel.Blue >= 230 && pixel.Green >= 230 && pixel.Red >= 230)) {
							return row + col + 2; // + 2 for the border
						}
					}
				}

				return 0;
			}
		}
		
		public bool IsCard(Mat img) {
			using (Mat imgHsv = new Mat())
			using (Mat thresh = new Mat()) {
				CvInvoke.CvtColor(img, imgHsv, ColorConversion.Bgr2Hsv);
				CvInvoke.InRange(imgHsv, ScalarArrayColor.HsvGreenLowerBound, ScalarArrayColor.HsvGreenUpperBound, thresh);
				return (CvInvoke.Mean(thresh).V0 < 200);
			}
		}

		public void Think() {
			// First check if there's a card that we can move from the Tableau to the Foundation
			for (int tableSlot = 0; tableSlot < tableauSlots; ++tableSlot) {
				Card tableCard = m_VisualState.m_Tableau[tableSlot];
				if (tableCard.m_Number == Number.UNKNOWN) { continue; }

				for (int foundationSlot = 0; foundationSlot < foundationSlots; ++foundationSlot) {
					Card foundationCard = m_VisualState.m_Foundation[foundationSlot];
					if (foundationCard.m_Number != Number.UNKNOWN && tableCard.m_Suit != foundationCard.m_Suit) { continue; }
					if (tableCard.m_Number != foundationCard.m_Number + 1) { continue; }

					print("Move " + tableCard + " from table: " + tableSlot + " to foundation: " + foundationSlot);
					return;
				}
			}
		}

		public override string GetState() {
			StringBuilder state = new StringBuilder(300);
			//state.Append("Current Action: ");
			//state.AppendLine(m_CurrentAction.ToString());
			//state.AppendLine();
			state.AppendLine("Visual State:");
			state.AppendLine();
			state.AppendLine("----------------");
			state.AppendLine();
			state.Append("Cards In Stock: ");
			state.AppendLine(m_VisualState.m_bCardsInStock.ToString());

			state.AppendLine();
			state.Append("Card In Waste: ");
			state.AppendLine((m_VisualState.m_CardInWaste.m_Number != Number.UNKNOWN) ? m_VisualState.m_CardInWaste.ToString() : "----");

			state.AppendLine();
			state.AppendLine("Foundation: ");
			foreach (Card card in m_VisualState.m_Foundation) {
				state.AppendLine((card.m_Number != Number.UNKNOWN) ? card.ToString() : "----");
			}

			state.AppendLine();
			state.AppendLine("Tableau: ");
			foreach (Card card in m_VisualState.m_Tableau) {
				state.AppendLine((card.m_Number != Number.UNKNOWN) ? card.ToString() : "----");
			}

			return state.ToString();
		}
	}
}
