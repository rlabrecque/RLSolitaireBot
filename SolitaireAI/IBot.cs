
namespace SolitaireAI {
	public abstract class IBot {
		public abstract void OnAttach();

		public abstract void OnDetach();

		public abstract System.Drawing.Bitmap OnGameFrame(System.Drawing.Bitmap capture);

		public abstract string GetState();

		public void print(string message) {
			System.Console.WriteLine(message);
			System.Diagnostics.Debug.WriteLine(message);
		}
	}
}
