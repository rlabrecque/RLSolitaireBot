using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace SolitaireAI {
	struct State {
		bool m_bCardsInStock;
	}

	public class Solitaire {
		public Solitaire() {

		}

		public Bitmap Run(Bitmap capture) {
			capture.DrawRect(new Rectangle(45, 45, 60, 60), Color.FromArgb(120, 0, 0, 255));

			return capture;
		}

	}
}
