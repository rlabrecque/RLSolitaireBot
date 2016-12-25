using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace SolitaireAI {
	public static class BitmapExtensions {
		//public enum 
	public static Bitmap DrawRect(this Bitmap bitmap, Rectangle rect, Color color) {
			for (int x = rect.X; x < rect.X + rect.Width; ++x) {
				for (int y = rect.Y; y < rect.Y + rect.Height; ++y) {
					bitmap.SetPixel(x, y, color);
				}
			}

			return bitmap;
		}
	}
}
