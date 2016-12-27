using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolitaireAI {
	public static class Debug {
		public static void Log(string message) {
			Console.WriteLine(message);
			System.Diagnostics.Debug.WriteLine(message);
		}
	}
}
