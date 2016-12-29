
namespace SolitaireAI {
	public interface IBotInfo {
		IBot GetBot { get; }
		string GetExecutableName { get; }
	}

	public abstract class IBot {
		public abstract void OnAttach();

		public abstract void OnDetach();

		public abstract System.Drawing.Bitmap OnGameFrame(byte[] data, System.Drawing.Size size, int stride);

		public abstract string GetState();

		public void print(string message) {
			System.Console.WriteLine(message);
			System.Diagnostics.Debug.WriteLine(message);
		}
	}
}
