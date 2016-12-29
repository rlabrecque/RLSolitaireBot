using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;

namespace SolitaireAI {
	public enum Number {
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
		Diamond, // Red
		Heart,   // Red
		Spade,   // Black
		Club,    // Black

		TOTAL
	}

	public struct Card {
		public Number m_Number;
		public Suit m_Suit;
	}

	public struct State {
		public bool m_bCardsInStock;
		public Card m_CurrentCardInWaste;
		public Card[] m_Foundation;
		public Card[] m_Tableau;
	}

	public struct ReferenceImages {
		public Mat[] m_Numbers;
		public Mat[] m_Suits;
	}

	public static class CvScalarColor {
		public static MCvScalar Red = new Bgr(Color.Red).MCvScalar;
		public static MCvScalar Green = new Bgr(Color.Green).MCvScalar;
		public static MCvScalar Blue = new Bgr(Color.Blue).MCvScalar;
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

		State m_State;
		ReferenceImages m_ReferenceImages;

		public override void OnAttach() {
			m_ReferenceImages.m_Numbers = new Mat[(int)Number.TOTAL];
			for (int i = 0; i < (int)Number.TOTAL; ++i) {
				string filename = String.Format("ReferenceImages/Numbers/{0}.bmp", (Number)i);
				m_ReferenceImages.m_Numbers[i] = CvInvoke.Imread(filename, ImreadModes.Grayscale);
			}

			m_ReferenceImages.m_Suits = new Mat[(int)Suit.TOTAL];
			for (int i = 0; i < (int)Suit.TOTAL; ++i) {
				string filename = String.Format("ReferenceImages/Suits/{0}.bmp", (Suit)i);
				m_ReferenceImages.m_Suits[i] = CvInvoke.Imread(filename, ImreadModes.Grayscale);
			}

			m_State.m_Foundation = new Card[foundationSlots];
			m_State.m_Tableau = new Card[tableauSlots];
		}

		public override void OnDetach() {
			for (int i = 0; i < (int)Number.TOTAL; ++i) {
				m_ReferenceImages.m_Numbers[i].Dispose();
			}

			for (int i = 0; i < (int)Suit.TOTAL; ++i) {
				m_ReferenceImages.m_Suits[i].Dispose();
			}
		}

		public override Bitmap OnGameFrame(byte[] data, Size size, int stride) {
			using (Mat img = Util.ByteArrayToMat(data, size, stride))
			using (Mat imgHsv = new Mat()) {
				CvInvoke.CvtColor(img, imgHsv, ColorConversion.Bgr2Hsv);

				// Stock
				{
					var rect = new Rectangle(leftOffset, topOffsetRow1, cardWidth, cardHeight);

					using (Mat imgHsvRoi = new Mat(imgHsv, rect)) {
						m_State.m_bCardsInStock = IsCard(imgHsvRoi);
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
						m_State.m_CurrentCardInWaste = ParseCard(card);
					}

					CvInvoke.Rectangle(img, rect, CvScalarColor.Blue);
				}

				// Foundation
				for (int i = 0; i < foundationSlots; ++i) {
					var rect = new Rectangle(leftOffset + (cardWidth * (3 + i)) + (cardSpacing * (3 + i)), topOffsetRow1, cardWidth, cardHeight);

					using (Mat card = new Mat(img, rect)) {
						m_State.m_Foundation[i] = ParseCard(card);
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
						m_State.m_Tableau[i] = ParseCard(card);
					}

					CvInvoke.Rectangle(img, rect, CvScalarColor.Blue);
				}

				return img.CopyToBitmap();
			}
		}

		Card ParseCard(Mat card) {
			// First check if a card exists at this location
			using (Mat cardHsv = new Mat()) {
				CvInvoke.CvtColor(card, cardHsv, ColorConversion.Bgr2Hsv);

				if (!IsCard(cardHsv)) {
					return new Card() { m_Number = Number.NONE };
				}
			}

			// Get Suit
			Suit suit = Suit.Diamond;
			{
				Rectangle rect = new Rectangle(4, 31, 22, 26);
				using (Mat grey = new Mat(card, rect)) {
					CvInvoke.CvtColor(grey, grey, ColorConversion.Bgr2Gray);
					CvInvoke.Threshold(grey, grey, 200, 255, ThresholdType.Binary);

					//grey.Bitmap.Save("ReferenceImages/Suits/s.bmp");
					//m_Return = grey.Bitmap;

					double lastSimilarity = 0;
					for (int i = 0; i < (int)Suit.TOTAL; ++i) {
						double similarity = Util.GetSimilarity(grey, m_ReferenceImages.m_Suits[i]);
						//print((Suit)i + ": " + similarity);
						if (similarity > lastSimilarity) {
							suit = (Suit)i;
							lastSimilarity = similarity;
						}
					}
				}

				CvInvoke.Rectangle(card, rect, CvScalarColor.Green);
			}

			// Get Number
			Number number = Number.NONE;
			{
				Rectangle rect = new Rectangle(3, 3, 27, 27);
				using (Mat grey = new Mat(card, rect)) {
					CvInvoke.CvtColor(grey, grey, ColorConversion.Bgr2Gray);
					CvInvoke.Threshold(grey, grey, 200, 255, ThresholdType.Binary);

					double lastSimilarity = 0;
					for (int i = 0; i < (int)Number.TOTAL; ++i) {
						double similarity = Util.GetSimilarity(grey, m_ReferenceImages.m_Numbers[i]);
						//print((Number)i + ": " + similarity);
						if (similarity > lastSimilarity) {
							number = (Number)i;
							lastSimilarity = similarity;
						}
					}
				}

				CvInvoke.Rectangle(card, rect, CvScalarColor.Green);
			}

			return new Card() { m_Number = number, m_Suit = suit };
		}

		public bool IsCard(Mat img) {
			using (Mat thresh = new Mat()) {
				ScalarArray lower = new ScalarArray(new MCvScalar(55, 25, 25));
				ScalarArray upper = new ScalarArray(new MCvScalar(190, 256, 256));
				CvInvoke.InRange(img, lower, upper, thresh);
				return (CvInvoke.Mean(thresh).V0 < 200);
			}
		}

		public override string GetState() {
			StringBuilder state = new StringBuilder(250);
			state.Append("CardsInStock: ");
			state.AppendLine(m_State.m_bCardsInStock.ToString());

			state.Append("CurrentCardInWaste: ");
			if (m_State.m_CurrentCardInWaste.m_Number != Number.NONE) {
				state.Append(m_State.m_CurrentCardInWaste.m_Number);
				state.Append(" ");
				state.AppendLine(m_State.m_CurrentCardInWaste.m_Suit.ToString());
			}
			else {
				state.AppendLine("----");
			}

			state.AppendLine("Foundation: ");
			foreach (Card card in m_State.m_Foundation) {
				if (card.m_Number != Number.NONE) {
					state.Append(card.m_Number);
					state.Append(" ");
					state.AppendLine(card.m_Suit.ToString());
				}
				else {
					state.AppendLine("----");
				}
			}

			state.AppendLine("Tableau: ");
			foreach (Card card in m_State.m_Tableau) {
				if (card.m_Number != Number.NONE) {
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
