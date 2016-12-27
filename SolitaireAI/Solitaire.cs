using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.CV.UI;


namespace SolitaireAI {
	enum Suit {
		Diamond, // Red
		Heart,   // Red
		Spade,   // Black
		Club,    // Black
	}

	struct Card {
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
			var imgGray = new Image<Gray, byte>(capture);
			imgGray = imgGray.ThresholdBinary(new Gray(120), new Gray(255));

			// Values for the smallest window size: Width: 624, Height: 398
            const int cardWidth = 55;
            const int cardHeight = 75;
            const int cardSpacing = 29;
            const int leftOffset = 31;
            const int topOffsetRow1 = 24;
            const int topOffsetRow2 = 122;

			// Stock
			{
				var rect = new Rectangle(leftOffset, topOffsetRow1, cardWidth, cardHeight);
				
				using (Mat hist = new Mat())
				using (Mat hist2 = new Mat())
				using (VectorOfMat vm = new VectorOfMat())
				using (VectorOfMat vm2 = new VectorOfMat()) {
					vm.Push(imgHsv.GetSubRect(rect));
					vm2.Push(new Image<Hsv, byte>(m_ReferenceImages.m_EmptyStock));
					var channels = new int[] { 0 };
					var histSize = new int[] { 256 };
					var ranges = new float[] { 0, 256, };
					CvInvoke.CalcHist(vm, channels, null, hist, histSize, ranges, false);
					CvInvoke.CalcHist(vm2, channels, null, hist2, histSize, ranges, false);

					//CvInvoke.Normalize(hist, hist, 0, 255, NormType.MinMax);
					//CvInvoke.Normalize(hist2, hist2, 0, 255, NormType.MinMax);


					//double res = CvInvoke.CompareHist(hist, hist2, HistogramCompMethod.Bhattacharyya);
					//Debug.Log("Cards in Stock: " + (res > 0.5));

					double res = CvInvoke.CompareHist(hist, hist2, HistogramCompMethod.Correl);
					m_State.m_bCardsInStock = (res < 0);
					Debug.Log(res + " Cards in Stock: " + m_State.m_bCardsInStock);

					/*for (int i = 0; i < 5; i++) {
						HistogramCompMethod compare_method = (HistogramCompMethod)i;
						double res = CvInvoke.CompareHist(hist, hist2, compare_method);

						Debug.Log(" Method [" + i + "] : " + res);
					}*/
				}

				CvInvoke.Rectangle(img, rect, new Bgr(255, 0, 0).MCvScalar);
			}

			// Waste
			{
				var rect = new Rectangle(leftOffset + cardWidth + cardSpacing, topOffsetRow1, cardWidth, cardHeight);
				CvInvoke.Rectangle(img, rect, new Bgr(255, 0, 0).MCvScalar);
			}

			// Foundation
			for (int i = 0; i < 4; ++i) {
				var rect = new Rectangle(leftOffset + (cardWidth * 3) + (cardSpacing * 3) + ((cardWidth + cardSpacing) * i), topOffsetRow1, cardWidth, cardHeight);
				CvInvoke.Rectangle(img, rect, new Bgr(255, 0, 0).MCvScalar);
			}

			// Tableau
			for (int i = 0; i < 7; ++i) {
				int rowCenter = leftOffset + (cardWidth / 2) + ((cardWidth + cardSpacing) * i);

				int bottom = 0;
				for (int y = img.Height - 1; y > topOffsetRow2; --y) {
					if (imgGray[y, rowCenter].Intensity > 0) {
						bottom = y;
						break;
					}
				}

				var rect = new Rectangle(rowCenter - (cardWidth / 2), topOffsetRow2, cardWidth, bottom - topOffsetRow2);
				CvInvoke.Rectangle(img, rect, new Bgr(255, 0, 0).MCvScalar);
			}

			capture = img.Bitmap;

			return capture;
		}
	}
}
