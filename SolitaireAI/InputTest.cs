using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolitaireAI {
	public class InputTestInfo : IBotInfo {
		public override string ToString() {
			return "InputTest";
		}

		public string GetExecutableName { get { return "Discord.exe"; } }
		public string Author { get { return "Riley Labrecque"; } }
		public IBot CreateBot { get { return new InputTest(); } }
	}

	class InputTest : IBot {
		public override void OnAttach() {
		}

		public override void OnDetach() {
		}

		public override System.Drawing.Bitmap OnGameFrame(byte[] data, System.Drawing.Size size, int stride) {
			return null;
		}

		public override void OnThink() {
			Input.SendKey(VK.F5);
		}

		public override string GetState() {
			return "";
		}
	}
}