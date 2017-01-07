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
using PInvoke;

namespace SolitaireAI {
	public enum Number {
		LOCKED = -2,
		NONE = -1,

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
		LOCKED = -2,
		NONE = -1,

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
		public Point m_Center;

		public override string ToString() {
			return m_Number + " " + m_Suit;
		}
	}

	public struct VisualState {
		public bool m_bCardsInStock;
		public Card m_CardInWaste;
		public Card[] m_Foundation;
		public List<Card>[] m_Tableau;
	}

	public struct ReferenceImages {
		public Mat[] m_Numbers;
		public Mat[] m_Suits;
		public Mat m_Locked;
	}
	
	public class SolitaireInfo : IBotInfo {
		public override string ToString() {
			return "Solitaire";
		}

		public string GetExecutableName { get { return "Solitaire.exe"; } }
		public string Author { get { return "Riley Labrecque"; } }
		public IBot CreateBot { get { return new Solitaire(); } }
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

		Mat m_DebugImg;
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
				CvInvoke.Threshold(sample2, sample2, 225, 255, ThresholdType.Binary);
				sample2.ConvertTo(sample2, DepthType.Cv32F);
				//sample2.CopyToBitmap().Save("ReferenceImages/Suits/_.bmp");

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
			m_VisualState.m_Tableau = new List<Card>[tableauSlots];
			for(int i = 0; i < tableauSlots; ++i) {
				m_VisualState.m_Tableau[i] = new List<Card>();
			}

			m_ReferenceImages.m_Locked = CvInvoke.Imread("ReferenceImages/Locked.bmp");
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
			using (Mat img = Util.ByteArrayToMat(data, size, stride))
			using (m_DebugImg = new Mat()) {
				img.CopyTo(m_DebugImg);
				// Stock
				{
					var rect = new Rectangle(leftOffset, topOffsetRow1, cardWidth, cardHeight);
					m_VisualState.m_bCardsInStock = IsCard(img, rect);
					CvInvoke.Rectangle(m_DebugImg, rect, CvScalarColor.Blue);
				}

				// Waste
				{
					const int stopX = leftOffset + (cardWidth * 2) + cardSpacing;
					const int centerY = topOffsetRow1 + (cardHeight / 2);
					
					int rightEdge = stopX;
					using (Mat slice = new Mat(img, new Rectangle(stopX, centerY, cardSpacing, 1))) {
						rightEdge += GetEdgeOfCardFromOutside(slice);
					}

					CvInvoke.Line(m_DebugImg, new Point(rightEdge, centerY), new Point(stopX + cardSpacing, centerY), CvScalarColor.Red);

					var rect = new Rectangle(rightEdge - cardWidth, topOffsetRow1, cardWidth, cardHeight);
					m_VisualState.m_CardInWaste = ParseCard(img, rect);
				}

				// Foundation
				for (int i = 0; i < foundationSlots; ++i) {
					var rect = new Rectangle(leftOffset + (cardWidth * (3 + i)) + (cardSpacing * (3 + i)), topOffsetRow1, cardWidth, cardHeight);
					m_VisualState.m_Foundation[i] = ParseCard(img, rect);
				}

				// Tableau
				for (int i = 0; i < tableauSlots; ++i) {
					m_VisualState.m_Tableau[i].Clear();

					Rectangle cardPos = new Rectangle(leftOffset + ((cardWidth + cardSpacing) * i), topOffsetRow2, cardWidth, cardHeight);
					if(i > 2) {
						cardPos.X += 1; // :(
					}

					while (true) {
						if (!IsCard(img, cardPos)) {
							CvInvoke.Rectangle(m_DebugImg, cardPos, CvScalarColor.Red);
							// Ugly hack :(
							if (m_VisualState.m_Tableau[i].Count == 0) {
								m_VisualState.m_Tableau[i].Add(new Card() { m_Number = Number.NONE, m_Suit = Suit.NONE, m_Center = cardPos.GetCenter() });
							}
							break;
						}

						Rectangle lockRect = new Rectangle(cardPos.X, cardPos.Y, cardWidth, 14);
						using (Mat locked = new Mat(img, lockRect)) {
							var similarityPercent = Util.GetSimilarity(m_ReferenceImages.m_Locked, locked);
							if (similarityPercent > 0.9) {
								CvInvoke.Rectangle(m_DebugImg, lockRect, CvScalarColor.Blue);
								
								m_VisualState.m_Tableau[i].Add(new Card() { m_Number = Number.LOCKED, m_Suit = Suit.LOCKED, m_Center = lockRect.GetCenter() });

								cardPos.Y += lockRect.Height;
								continue;
							}
						}

						Rectangle sliceRect = new Rectangle(cardPos.Right - 10, cardPos.Top + 1, 1, cardHeight / 2);
						using (Mat slice = new Mat(img, sliceRect)) {
							cardPos.Height = GetEdgeOfCardFromInside(slice);
							CvInvoke.Line(m_DebugImg, new Point(sliceRect.Left, sliceRect.Top), new Point(sliceRect.Left, sliceRect.Bottom), CvScalarColor.Red);
						}
						
						Card card = ParseCard(img, cardPos);
						m_VisualState.m_Tableau[i].Add(card);
						
						if (img.Height - cardPos.Bottom < cardHeight) {
							break;
						}
						
						cardPos.Y += cardPos.Height;
						cardPos.Height = cardHeight;
					}
				}

				return m_DebugReturn ?? m_DebugImg.CopyToBitmap();
			}
		}
		
		Card ParseCard(Mat img, Rectangle cardPos) {
			// First check if a card exists at this location
			if (!IsCard(img, cardPos)) {
				CvInvoke.Rectangle(m_DebugImg, cardPos, CvScalarColor.Red);
				return new Card() { m_Number = Number.NONE, m_Suit = Suit.NONE, m_Center = cardPos.GetCenter() };
			}

			CvInvoke.Rectangle(m_DebugImg, cardPos, CvScalarColor.Blue);

			// Get Suit
			Rectangle rect = new Rectangle(cardPos.X + 6, cardPos.Y + 32, 20, 10);
			Suit suit = (Suit)GetImageSimilarityFromKNN(m_SuitsKNN, img, rect);
			CvInvoke.Rectangle(m_DebugImg, rect, CvScalarColor.Green);

			// Get Number
			rect = new Rectangle(cardPos.X + 6, cardPos.Y + 6, 24, 24);
			Number number = (Number)GetImageSimilarityFromKNN(m_NumbersKNN, img, rect);
			CvInvoke.Rectangle(m_DebugImg, rect, CvScalarColor.Green);

			return new Card() { m_Number = number, m_Suit = suit, m_Center = cardPos.GetCenter() };
		}

		public int GetEdgeOfCardFromInside(Mat mat) {
			// TODO: Ugh IplImage
			using (var img = mat.ToImage<Bgr, byte>()) {
				for (int row = 0; row < mat.Rows; ++row) {
					Bgr pixel = img[row, 0];
					if (pixel.Blue <= 180 && pixel.Green <= 180 && pixel.Red <= 180) {
						return row + 1; // +1px for the border
					}
				}
			}

			return cardHeight;
		}

		public int GetEdgeOfCardFromOutside(Mat mat) {
			// TODO: Ugh IplImage
			using (var img = mat.ToImage<Bgr, byte>()) {
				for (int col = mat.Cols - 1; col >= 0; --col) {
					var pixel = img[0, col];
					if ((pixel.Blue >= 230 && pixel.Green >= 230 && pixel.Red >= 230)) {
						return col + 2; // +2px for the border
					}
				}

				return 0;
			}
		}
				
		public bool IsCard(Mat img, Rectangle cardPos) {
			using (Mat cardImg = new Mat(img, cardPos))
			using (Mat imgHsv = new Mat())
			using (Mat thresh = new Mat()) {
				CvInvoke.CvtColor(cardImg, imgHsv, ColorConversion.Bgr2Hsv);
				CvInvoke.InRange(imgHsv, ScalarArrayColor.HsvGreenLowerBound, ScalarArrayColor.HsvGreenUpperBound, thresh);
				return (CvInvoke.Mean(thresh).V0 < 200);
			}
		}

		public override void OnThink() {
			// First check if there's a card that we can move from the Tableau to the Foundation
			//for (int tableSlot = 0; tableSlot < tableauSlots; ++tableSlot) {
			//foreach (List<Card> tableauSlot in m_VisualState.m_Tableau) {
			for (int i = tableauSlots - 1; i >= 0; --i) {
				List<Card> tableauSlot = m_VisualState.m_Tableau[i];

				Card tableCard = tableauSlot[tableauSlot.Count - 1];
				if (tableCard.m_Number == Number.NONE) { continue; }

				for (int foundationSlot = 0; foundationSlot < foundationSlots; ++foundationSlot) {
					Card foundationCard = m_VisualState.m_Foundation[foundationSlot];
					//if (foundationCard.m_Number != Number.UNKNOWN) { continue; }
					if (tableCard.m_Number != foundationCard.m_Number + 1) { continue; }
					if (foundationCard.m_Suit != Suit.NONE && tableCard.m_Suit != foundationCard.m_Suit) { continue; }

					print("Move Tableau[" + tableCard + "] to Foundation[" + foundationCard + "]");
					MoveCard(tableCard, foundationCard);
					return;
				}
			}

			// Next check if there's a card that we can move from the Tableu elsewhere on the Tableu
			//for (int tableSlot = 0; tableSlot < tableauSlots; ++tableSlot) {
			for (int i = tableauSlots - 1; i >= 0; --i) {
				List<Card> tableauSlot = m_VisualState.m_Tableau[i];

				Card tableCard;
				for (int test = 0; ; ++test) {
					tableCard = tableauSlot[test];
					if (tableCard.m_Number == Number.NONE) { break; }
					if (tableCard.m_Number != Number.LOCKED) { break; }
				}

				if (tableCard.m_Number == Number.NONE) { continue; }
				
				for (int j = 0; j < tableauSlots; ++j) {
					if (j == i) { continue; }

					List<Card> PotentialTableauSlot = m_VisualState.m_Tableau[j];
					Card nextTableCard = PotentialTableauSlot[PotentialTableauSlot.Count - 1];

					// Check if the new space is empty, and then see if this is a king that we can move there.
					if (nextTableCard.m_Number == Number.NONE) {
						if (tableCard.m_Number == Number.King && tableCard.m_Number != tableauSlot[0].m_Number) {
							print("Move Tableau[" + tableCard + "] to Tableau[" + nextTableCard + "]");
							MoveCard(tableCard, nextTableCard);
							return;
						}

						continue;
					}

					if (tableCard.m_Number != nextTableCard.m_Number - 1) { continue; }
					if (IsSameColor(tableCard, nextTableCard)) { continue; }

					print("Move Tableau[" + tableCard + "] to Tableu [" + nextTableCard + "]");
					MoveCard(tableCard, nextTableCard);
					return;
				}
			}

			// Then see if we can move the card from the waste somewhere
			if (m_VisualState.m_CardInWaste.m_Number != Number.NONE) {
				for (int foundationSlot = 0; foundationSlot < foundationSlots; ++foundationSlot) {
					Card foundationCard = m_VisualState.m_Foundation[foundationSlot];
					//if (foundationCard.m_Number != Number.UNKNOWN) { continue; }
					if (m_VisualState.m_CardInWaste.m_Number != foundationCard.m_Number + 1) { continue; }
					if (foundationCard.m_Suit != Suit.NONE && m_VisualState.m_CardInWaste.m_Suit != foundationCard.m_Suit) { continue; }

					print("Move " + m_VisualState.m_CardInWaste + " from the Waste to Foundation[" + foundationSlot + "]");
					MoveCard(m_VisualState.m_CardInWaste, foundationCard);
					return;
				}

				//for (int potentialTableSlot = 0; potentialTableSlot < tableauSlots; ++potentialTableSlot) {
				for (int j = 0; j < tableauSlots; ++j) {
					List<Card> PotentialTableauSlot = m_VisualState.m_Tableau[j];
					Card nextTableCard = PotentialTableauSlot[PotentialTableauSlot.Count - 1];

					// Check if the new space is empty, and then see if this is a king that we can move there.
					if (nextTableCard.m_Number == Number.NONE) {
						if (m_VisualState.m_CardInWaste.m_Number == Number.King) {
							print("Move " + m_VisualState.m_CardInWaste + " from the Waste to Tableau[" + nextTableCard + "]");
							MoveCard(m_VisualState.m_CardInWaste, nextTableCard);
							return;
						}

						continue;
					}

					if (m_VisualState.m_CardInWaste.m_Number != nextTableCard.m_Number - 1) { continue; }
					if (IsSameColor(m_VisualState.m_CardInWaste, nextTableCard)) { continue; }

					print("Move " + m_VisualState.m_CardInWaste + " from the Waste to Tableau[" + nextTableCard + "]");
					MoveCard(m_VisualState.m_CardInWaste, nextTableCard);
					return;
				}
			}

			// Finally hit the deck
			print("Flip a card from the deck");
			Input.SendMouseLButtonDown(new Point(leftOffset + (cardWidth / 2), topOffsetRow1 + (cardHeight / 2)));
			System.Threading.Thread.Sleep(666);
		}

		public bool IsSameColor(Card a, Card b) {
			return ((a.m_Suit == Suit.Diamond || a.m_Suit == Suit.Heart) &&
					(b.m_Suit == Suit.Diamond || b.m_Suit == Suit.Heart)) ||
					((a.m_Suit == Suit.Club || a.m_Suit == Suit.Spade) &&
					(b.m_Suit == Suit.Club || b.m_Suit == Suit.Spade));
		}

		public void MoveCard(Card from, Card to) {
			//print("Moving " + from + " from " + from.m_Center + " to " + to.m_Center);
			Input.SendMouseLButtonDown(from.m_Center);
			System.Threading.Thread.Sleep(666);
			Input.SendMouseLButtonDown(to.m_Center);
			System.Threading.Thread.Sleep(666);
		}

		public override string GetState() {
			StringBuilder state = new StringBuilder(300);
			//state.Append("Current Action: ");
			//state.AppendLine(m_CurrentAction.ToString());
			//state.AppendLine();
			state.AppendLine("Visual State:");
			state.AppendLine("----------------");
			state.Append("Cards In Stock: ");
			state.AppendLine(m_VisualState.m_bCardsInStock.ToString());

			state.AppendLine();
			state.Append("Card In Waste: ");
			state.AppendLine((m_VisualState.m_CardInWaste.m_Number != Number.NONE) ? m_VisualState.m_CardInWaste.ToString() : "----");

			state.AppendLine();
			state.AppendLine("Foundation: ");
			foreach (Card card in m_VisualState.m_Foundation) {
				state.AppendLine((card.m_Number != Number.NONE) ? card.ToString() : "----");
			}

			state.AppendLine();
			state.AppendLine("Tableau: ");
			foreach (List<Card> tableuSlot in m_VisualState.m_Tableau) {
				foreach (Card card in tableuSlot) {
					state.AppendLine((card.m_Number != Number.NONE) ? card.ToString() : "----");
				}
				state.AppendLine();
			}

			return state.ToString();
		}
	}
}
