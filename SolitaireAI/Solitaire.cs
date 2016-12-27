using System;
using System.Collections.Generic;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;

namespace SolitaireAI {
	public enum Suit {
		Diamond, // Red
		Heart,   // Red
		Spade,   // Black
		Club,    // Black
	}

	public struct Card {
		public int m_Number;
		public Suit m_Suit;
	}

	public struct State {
		public bool m_bCardsInStock;
		public Card? m_CurrentCardInWaste;
		public Card?[] m_Foundation;
		public Card?[] m_Tableau;
	}

	public struct ReferenceImages {
		public Bitmap m_EmptyStock;
		public Bitmap[] m_Suits;
	}

	public class Solitaire {
		public State m_State;
		ReferenceImages m_ReferenceImages;

		public Solitaire() {
			m_ReferenceImages.m_EmptyStock = (Bitmap)Bitmap.FromFile("ReferenceImages/EmptyStock.bmp");
			m_ReferenceImages.m_Suits = new Bitmap[4];
			for(int i = 0; i < 4; ++i) {
				string filename = String.Format("ReferenceImages/Suits/{0}.bmp", (Suit)i);
				m_ReferenceImages.m_Suits[i] = (Bitmap)Bitmap.FromFile(filename);
			}

			m_State.m_Foundation = new Card?[4];
			m_State.m_Tableau = new Card?[7];
		}

		public Bitmap OnScreenshot(Bitmap capture) {
			var img = new Image<Bgr, byte>(capture);
			var imgHsv = new Image<Hsv, byte>(capture);
			var imgGray = new Image<Gray, byte>(capture);

			imgGray = imgGray.ThresholdBinary(new Gray(120), new Gray(255));

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
				imgHsv.ROI = rect;
				m_State.m_bCardsInStock = (Util.CompareHistograms(imgHsv.Mat, new Image<Hsv, byte>(m_ReferenceImages.m_EmptyStock).Mat) < 0);
				Util.Log(" Cards in Stock: " + m_State.m_bCardsInStock);
				imgHsv.ROI = Rectangle.Empty;
				
				CvInvoke.Rectangle(img, rect, new Bgr(255, 0, 0).MCvScalar);
			}

			// Waste
			{
				var rect = new Rectangle(leftOffset + cardWidth + cardSpacing, topOffsetRow1, cardWidth, cardHeight);
				CvInvoke.Rectangle(img, rect, new Bgr(255, 0, 0).MCvScalar);
			}

			// Foundation
			for (int i = 0; i < m_State.m_Foundation.Length; ++i) {
				var rect = new Rectangle(leftOffset + (cardWidth * 3) + (cardSpacing * 3) + ((cardWidth + cardSpacing) * i), topOffsetRow1, cardWidth, cardHeight);
				CvInvoke.Rectangle(img, rect, new Bgr(255, 0, 0).MCvScalar);
			}

			// Tableau
			for (int i = 0; i < m_State.m_Tableau.Length; ++i) {
				int rowCenter = leftOffset + (cardWidth / 2) + ((cardWidth + cardSpacing) * i);

				int bottom = 0;
				for (int y = img.Height - 1; y > topOffsetRow2; --y) {
					if (imgGray[y, rowCenter].Intensity > 0) {
						bottom = y;
						break;
					}
				}

				var rect = new Rectangle(rowCenter - (cardWidth / 2), bottom - cardHeight, cardWidth, cardHeight);
				
				m_State.m_Tableau[i] = ParseCard(new Mat(img.Mat, rect));

				CvInvoke.Rectangle(img, rect, new Bgr(255, 0, 0).MCvScalar);
			}

			CvInvoke.Resize(img, img, new Size(900, 600));
			capture = img.Bitmap;

			return capture;
		}

		Card ParseCard(Mat card) {
			// Get Suit
			var rect = new Rectangle(6, 34, 21, 24);
			//new Mat(card, rect).Bitmap.Save("ReferenceImages/Suits/s.bmp");

			Suit suit = Suit.Diamond;
			double lastSimilarity = double.MaxValue;
			for (int i = 0; i < 4; ++i) {
				double similarity = Util.GetSimilarity(new Mat(card, rect), new Image<Bgr, byte>(m_ReferenceImages.m_Suits[i]).Mat);
				Util.Log((Suit)i + ": " + similarity);
				if (similarity < lastSimilarity) {
					suit = (Suit)i;
					lastSimilarity = similarity;
				}
			}

			/*if(!bMatched) {
				//throw new Exception("Suit Missmatch!");
				Util.Log("Suit Missmatch!");
			}*/
			
			CvInvoke.Rectangle(card, rect, new Bgr(0, 255, 0).MCvScalar);

			return new Card() { m_Number = 0, m_Suit = suit };
		}
	}
}
