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
		King
	}

	public enum Suit {
		Diamond, // Red
		Heart,   // Red
		Spade,   // Black
		Club     // Black
	}

	public struct Card {
		public Number m_Number;
		public Suit m_Suit;
	}

	public struct State {
		public bool m_bCardsInStock;
		public Card? m_CurrentCardInWaste;
		public Card?[] m_Foundation;
		public Card?[] m_Tableau;
	}

	public struct ReferenceImages {
		public Mat[] m_Numbers;
		public Mat[] m_Suits;
	}

	public class Solitaire : IBot {
		State m_State;
		ReferenceImages m_ReferenceImages;
		Bitmap m_Return;

		public override void OnAttach() {
			m_ReferenceImages.m_Numbers = new Mat[13];
			m_ReferenceImages.m_Suits = new Mat[4];
			for (int i = 0; i < m_ReferenceImages.m_Numbers.Length; ++i) {
				string filename = String.Format("ReferenceImages/Numbers/{0}.bmp", (Number)i);
				m_ReferenceImages.m_Numbers[i] = CvInvoke.Imread(filename, ImreadModes.Grayscale);
			}
			for (int i = 0; i < m_ReferenceImages.m_Suits.Length; ++i) {
				string filename = String.Format("ReferenceImages/Suits/{0}.bmp", (Suit)i);
				m_ReferenceImages.m_Suits[i] = CvInvoke.Imread(filename, ImreadModes.Grayscale);
			}

			m_State.m_Foundation = new Card?[4];
			m_State.m_Tableau = new Card?[7];
		}

		public override void OnDetach() {
		}

		public override Bitmap OnGameFrame(Bitmap capture) {
			Mat img = new Image<Bgr, byte>(capture).Mat;

			Mat imgHsv = new Mat();
			CvInvoke.CvtColor(img, imgHsv, ColorConversion.Bgr2Hsv);
			
			// Values for the smallest window size: Width: 624, Height: 398
			/*const int cardWidth = 55;
			const int cardHeight = 75;
			const int cardSpacing = 29;
			const int leftOffset = 31;
			const int topOffsetRow1 = 24;
			const int topOffsetRow2 = 122;*/

			// Values for the biggest window size: Width: 1920, Height: 1080
			const int cardWidth = 141;
			const int cardHeight = 191;
			const int cardSpacing = 73;
			const int leftOffset = 248;
			const int topOffsetRow1 = 47;
			const int topOffsetRow2 = 295;

			// Stock
			{
				var rect = new Rectangle(leftOffset, topOffsetRow1, cardWidth, cardHeight);

				Mat imgHsvRoi = new Mat(imgHsv, rect);
				m_State.m_bCardsInStock = IsCard(imgHsvRoi);
				
				CvInvoke.Rectangle(img, rect, new Bgr(255, 0, 0).MCvScalar);
			}

			// Waste
			{
				const int startX = leftOffset + (cardWidth * 2) + (cardSpacing * 2);
				const int stopX = leftOffset + (cardWidth * 2) + cardSpacing;
				const int centerY = topOffsetRow1 + (cardHeight / 2);

				int rightEdge = 0;
				for (int x = startX; x > stopX; --x) {
					Mat pixel = new Mat(imgHsv, new Rectangle(x, centerY, 1, 1));
					if (IsCard(pixel)) {
						break;
					}
					rightEdge = x;
				}

				CvInvoke.Line(img, new Point(startX, centerY), new Point(rightEdge, centerY), new Bgr(Color.Red).MCvScalar);

				var rect = new Rectangle(rightEdge - cardWidth, topOffsetRow1, cardWidth, cardHeight);

				Mat card = new Mat(img, rect);
				m_State.m_CurrentCardInWaste = ParseCard(card);

				CvInvoke.Rectangle(img, rect, new Bgr(255, 0, 0).MCvScalar);
			}

			// Foundation
			for (int i = 0; i < m_State.m_Foundation.Length; ++i) {
				var rect = new Rectangle(leftOffset + (cardWidth * (3 + i)) + (cardSpacing * (3 + i)), topOffsetRow1, cardWidth, cardHeight);

				Mat card = new Mat(img, rect);
				m_State.m_Foundation[i] = ParseCard(card);

				CvInvoke.Rectangle(img, rect, new Bgr(255, 0, 0).MCvScalar);
			}

			// Tableau
			for (int i = 0; i < m_State.m_Tableau.Length; ++i) {
				int startHeight = img.Height - 1;
				int stopHeight = topOffsetRow2 + cardHeight;
				int cardCenterX = leftOffset + (cardWidth / 2) + ((cardWidth + cardSpacing) * i);

				int bottomEdge = 0;
				for (int y = startHeight; y > stopHeight; --y) {
					Mat pixel = new Mat(imgHsv, new Rectangle(cardCenterX, y, 1, 1));
					if (IsCard(pixel)) {
						break;
					}
					bottomEdge = y;
				}

				CvInvoke.Line(img, new Point(cardCenterX, img.Height - 1), new Point(cardCenterX, bottomEdge), new Bgr(Color.Red).MCvScalar);


				var rect = new Rectangle(cardCenterX - (cardWidth / 2), bottomEdge - cardHeight, cardWidth, cardHeight);

				//if( i == 0) {
				Mat card = new Mat(img, rect);
				m_State.m_Tableau[i] = ParseCard(card);

				CvInvoke.Rectangle(img, rect, new Bgr(255, 0, 0).MCvScalar);
			}

			//CvInvoke.Resize(img, img, new Size(900, 600));

			return m_Return ?? img.Bitmap;
		}

		Card? ParseCard(Mat card) {
			// Check if a card exists at this location at all.
			{
				Mat cardHsv = new Mat();
				CvInvoke.CvtColor(card, cardHsv, ColorConversion.Bgr2Hsv);

				if (!IsCard(cardHsv)) {
					return null;
				}
			}

			// Get Suit
			Suit suit = Suit.Diamond;
			{
				Rectangle rect = new Rectangle(4, 31, 22, 26);
				Mat grey = new Mat(card, rect);
				CvInvoke.CvtColor(grey, grey, ColorConversion.Bgr2Gray);
				CvInvoke.Threshold(grey, grey, 200, 255, ThresholdType.Binary);

				//grey.Bitmap.Save("ReferenceImages/Suits/s.bmp");
				//m_Return = grey.Bitmap;

				double lastSimilarity = 0;
				for (int i = 0; i < m_ReferenceImages.m_Suits.Length; ++i) {
					double similarity = Util.GetSimilarity(grey, m_ReferenceImages.m_Suits[i]);
					//print((Suit)i + ": " + similarity);
					if (similarity > lastSimilarity) {
						suit = (Suit)i;
						lastSimilarity = similarity;
					}
				}

				CvInvoke.Rectangle(card, rect, new Bgr(Color.Green).MCvScalar);
			}

			// Get Number
			Number number = 0;
			{
				Rectangle rect = new Rectangle(3, 3, 27, 27);
				Mat grey = new Mat(card, rect);
				CvInvoke.CvtColor(grey, grey, ColorConversion.Bgr2Gray);
				CvInvoke.Threshold(grey, grey, 200, 255, ThresholdType.Binary);

				double lastSimilarity = 0;
				for (int i = 0; i < m_ReferenceImages.m_Numbers.Length; ++i) {
					double similarity = Util.GetSimilarity(grey, m_ReferenceImages.m_Numbers[i]);
					//print((Number)i + ": " + similarity);
					if (similarity > lastSimilarity) {
						number = (Number)i;
						lastSimilarity = similarity;
					}
				}

				CvInvoke.Rectangle(card, rect, new Bgr(Color.Green).MCvScalar);
			}

			return new Card() { m_Number = number, m_Suit = suit };
		}

		public bool IsCard(Mat img) {
			Mat thresh = new Mat();
			CvInvoke.InRange(img, new ScalarArray(new MCvScalar(55, 25, 25)), new ScalarArray(new MCvScalar(190, 256, 256)), thresh);
			return (CvInvoke.Mean(thresh).V0 < 200);
		}

		public override string GetState() {
			StringBuilder state = new StringBuilder(120);
			state.Append("CardsInStock: ");
			state.AppendLine(m_State.m_bCardsInStock.ToString());

			state.Append("CurrentCardInWaste: ");
			if (m_State.m_CurrentCardInWaste.HasValue) {
				state.Append(m_State.m_CurrentCardInWaste.Value.m_Number);
				state.Append(" ");
				state.AppendLine(m_State.m_CurrentCardInWaste.Value.m_Suit.ToString());
			}
			else {
				state.AppendLine("----");
			}

			state.AppendLine("Foundation: ");
			foreach (Card? card in m_State.m_Foundation) {
				if (card.HasValue) {
					state.Append(card.Value.m_Number);
					state.Append(" ");
					state.AppendLine(card.Value.m_Suit.ToString());
				}
				else {
					state.AppendLine("----");
				}
			}

			state.AppendLine("Tableau: ");
			foreach (Card? card in m_State.m_Tableau) {
				if (card.HasValue) {
					state.Append(card.Value.m_Number);
					state.Append(" ");
					state.AppendLine(card.Value.m_Suit.ToString());
				}
				else {
					state.AppendLine("----");
				}
			}

			return state.ToString();
		}
	}
}
