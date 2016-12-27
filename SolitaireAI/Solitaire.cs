using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;


namespace SolitaireAI {
	enum Suit {
		Diamond, // Red
		Heart,   // Red
		Spade,   // Black
		Club,    // Black
	}

	struct Card {
		public const int width = 85;
		public const int height = 115;
		int m_number;
		Suit m_Suit;
	}

	struct State {
		public bool m_bCardsInStock;
		public Card? m_CurrentCardInWaste;
	}

	struct ReferenceImages {
		public Bitmap m_EmptyStock;
	}

	public class Solitaire {
		State m_State;
		ReferenceImages m_ReferenceImages;


		public Solitaire() {
			m_ReferenceImages.m_EmptyStock = (Bitmap)Bitmap.FromFile("ReferenceImages/EmptyStock.bmp");
		}

		public Bitmap OnScreenshot(Bitmap capture) {
			var img = new Image<Bgr, byte>(capture);
			var imgHsv = new Image<Hsv, byte>(capture);
			var imgGray = img.Convert<Gray, byte>();
			imgGray = imgGray.ThresholdBinary(new Gray(120), new Gray(255));

			const int cardSpacing = 44;

			// Stock
			{
				const int leftOffset = 49;
				const int topOffset = 28;

				var rect = new Rectangle(leftOffset, topOffset, Card.width, Card.height);
				CvInvoke.Rectangle(img, rect, new Bgr(255, 0, 0).MCvScalar);

				//var hist = new Mat();
				//CvInvoke.CalcHist(new Image<Hsv, byte>(m_ReferenceImages.m_EmptyStock), new int[] { 0, 1, 2 }, null, hist, new int[] { 8, 8, 8 }, new float[] {0, 256, 0, 256, 0, 256}, false);
				//Console.WriteLine(hist);
			}

			// Waste
			{
				const int leftOffset = 178;
				const int topOffset = 28;

				var rect = new Rectangle(leftOffset, topOffset, Card.width, Card.height);
				CvInvoke.Rectangle(img, rect, new Bgr(255, 0, 0).MCvScalar);
			}

			// Foundation
			for (int i = 0; i < 4; ++i) {
				const int leftOffset = 437;
				const int topOffset = 28;
				int rowCenter = leftOffset + (Card.width / 2) + ((Card.width + cardSpacing) * i);

				var rect = new Rectangle(rowCenter - (Card.width / 2), topOffset, Card.width, Card.height);
				CvInvoke.Rectangle(img, rect, new Bgr(255, 0, 0).MCvScalar);
			}

			// Tableau
			for (int i = 0; i < 7; ++i) {
				const int leftOffset = 49;
				const int topOffset = 178;
				int rowCenter = leftOffset + (Card.width / 2) + ((Card.width + cardSpacing) * i);

				int bottom = 0;
				for (int y = img.Height - 1; y > topOffset; --y) {
					if (imgGray[y, rowCenter].Intensity > 0) {
						bottom = y;
						break;
					}
				}

				var rect = new Rectangle(rowCenter - (Card.width / 2), topOffset, Card.width, bottom - topOffset);
				CvInvoke.Rectangle(img, rect, new Bgr(255, 0, 0).MCvScalar);
			}

			capture = img.Bitmap;

			return capture;
		}
	}
}
