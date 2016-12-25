using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
/*using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;*/


namespace SolitaireAI {
	struct State {
		bool m_bCardsInStock;
	}

	public class Solitaire {
		public Solitaire() {

		}

		public Bitmap Run(Bitmap capture) {
			//Image<Bgr, Byte> img = new Image<Bgr, Byte>(capture);
			/*using (Mat img = new Mat(200, 400, DepthType.Cv8U, 3)) {
				img.SetTo(new Bgr(255, 0, 0).MCvScalar);
				capture = img.Bitmap;
			}*/

			capture.DrawRect(new Rectangle(45, 45, 60, 60), Color.FromArgb(120, 0, 0, 255));

			return capture;
		}

	}
}
