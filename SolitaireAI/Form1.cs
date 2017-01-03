using System;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using System.IO;
using Capture.Interface;
using Capture.Hook;
using Capture;

using PInvoke;

namespace SolitaireAI {
	public partial class Form1 : Form {
		public static Form1 Instance { get; private set; }

		IBot m_Bot;
		public static CaptureProcess m_captureProcess;
		bool m_StepThisFrame = false;

		public Form1() {
			InitializeComponent();
			Instance = this;
		}

		private void Form1_Load(object sender, EventArgs e) {
			//m_BotInfoComboBox.Items.Add(new InputTestInfo());
			m_BotInfoComboBox.Items.Add(new SolitaireInfo());
			m_BotInfoComboBox.SelectedIndex = 0;

			AttachProcess();
		}

		private void m_InjectButton_Click(object sender, EventArgs e) {
			if (m_captureProcess == null) {
				AttachProcess();
			}
			else {
				DetachProcess();
			}
		}
		
		private void m_SteppingEnabledCheckbox_CheckedChanged(object sender, EventArgs e) {
			m_StepThisFrameButton.Enabled = m_SteppingEnabledCheckbox.Checked;
		}

		private void m_StepThisFrame_Click(object sender, EventArgs e) {
			m_StepThisFrame = true;
		}

		private void AttachProcess() {
			m_InjectButton.Enabled = false;

			string exeName = Path.GetFileNameWithoutExtension(((IBotInfo)m_BotInfoComboBox.SelectedItem).GetExecutableName);
			Process[] processes = Process.GetProcessesByName(exeName);
			if (processes.Length == 0) {
				MessageBox.Show("No executable found matching: '" + exeName + "'");
				m_InjectButton.Enabled = true;
				return;
			}
			
			Process process = processes[0];
			
			// If the process doesn't have a mainwindowhandle yet, skip it (we need to be able to get the hwnd to set foreground etc)
			if (process.MainWindowHandle == IntPtr.Zero) { return; }

			// Skip if the process is already hooked (and we want to hook multiple applications)
			if (HookManager.IsHooked(process.Id)) { return; }

			CaptureConfig cc = new CaptureConfig() { Direct3DVersion = Direct3DVersion.AutoDetect, ShowOverlay = true };
			
			//captureInterface.RemoteMessage += new MessageReceivedEvent(CaptureInterface_RemoteMessage);
			m_captureProcess = new CaptureProcess(process, cc, new CaptureInterface());

			Thread.Sleep(10);

			IBot.print("Creating and attaching script: " + ((IBotInfo)m_BotInfoComboBox.SelectedItem).ToString() + " by " + ((IBotInfo)m_BotInfoComboBox.SelectedItem).Author);
			m_Bot = ((IBotInfo)m_BotInfoComboBox.SelectedItem).CreateBot;
			m_Bot.OnAttach();

			m_InjectButton.Text = "Detach";
			m_InjectButton.Enabled = true;
			m_SteppingEnabledCheckbox.Checked = true;
			m_SteppingEnabledCheckbox.Enabled = true;

			myTimer.Tick += new EventHandler(TimerEventProcessor);
			myTimer.Interval = 100;
			myTimer.Start();

			CaptureScreenshot();
		}

		System.Windows.Forms.Timer myTimer = new System.Windows.Forms.Timer();
		unsafe void TimerEventProcessor(Object myObject, EventArgs myEventArgs) {
			User32.SafeDCHandle dc = User32.GetDC(m_captureProcess.Process.MainWindowHandle);
			User32.PostMessage(m_captureProcess.Process.MainWindowHandle, User32.WindowMessage.WM_PAINT, (void*)dc.DangerousGetHandle(), null);
		}

		private void DetachProcess() {
			myTimer.Stop();

			m_InjectButton.Text = "Inject";
			m_InjectButton.Enabled = true;
			m_SteppingEnabledCheckbox.Checked = false;
			m_SteppingEnabledCheckbox.Enabled = false;

			IBot.print("Unloading script");
			m_Bot.OnDetach();
			m_Bot = null;

			HookManager.RemoveHookedProcess(m_captureProcess.Process.Id);
			m_captureProcess.CaptureInterface.Disconnect();
			m_captureProcess = null;

			m_DebugVisualizer.Image = null;
		}

		/*void CaptureInterface_RemoteMessage(MessageReceivedEventArgs message) {
			System.Diagnostics.Debug.WriteLine(String.Format("{0}\r\n{1}", message, txtDebugLog.Text));
			txtDebugLog.Invoke(new MethodInvoker(
				delegate () {
					txtDebugLog.Text = String.Format("{0}\r\n{1}", message, txtDebugLog.Text);
				}
			));
		}

		void ScreenshotManager_OnScreenshotDebugMessage(int clientPID, string message) {
			txtDebugLog.Invoke(new MethodInvoker(
				delegate () {
					txtDebugLog.Text = String.Format("{0}:{1}\r\n{2}", clientPID, message, txtDebugLog.Text);
				}
			));
		}*/
		
		void CaptureScreenshot() {
			if(m_captureProcess == null || m_Bot == null) {
				return;
			}

			this.Invoke(new MethodInvoker(
				delegate () {
					m_captureProcess.CaptureInterface.BeginGetScreenshot(Rectangle.Empty, new TimeSpan(0, 0, 1), ScreenshotCallback, null, format: ImageFormat.PixelData);
				}
			));
		}

		void ScreenshotCallback(IAsyncResult result) {
			if (m_captureProcess == null || m_Bot == null) {
				return;
			}

			using (Screenshot screenshot = m_captureProcess.CaptureInterface.EndGetScreenshot(result)) {
				try {
					if (screenshot != null && screenshot.Data != null) {
						m_DebugVisualizer.Invoke(new MethodInvoker(
							delegate () {
								if (m_DebugVisualizer.Image != null) {
									m_DebugVisualizer.Image.Dispose();
								}

								Bitmap screen = m_Bot.OnGameFrame(screenshot.Data, new Size(screenshot.Width, screenshot.Height), screenshot.Stride);
								if (screen != null) {
									m_DebugVisualizer.Image = screen;
								}

								if (!m_SteppingEnabledCheckbox.Checked || m_StepThisFrame) {
									m_Bot.OnThink();
									m_StepThisFrame = false;
								}

								m_StateDisplayLabel.Text = m_Bot.GetState();
							}
						));
					}

					Thread t = new Thread(new ThreadStart(CaptureScreenshot));
					t.Start();
				}
				catch {
				}
			}
		}

		delegate void UpdateLogListBoxDelegate(string msg);
		public void AddToListbox(string msg) {
			// Check whether the caller must call an invoke method when making method calls to listBoxCCNetOutput because the caller is 
			// on a different thread than the one the listBoxCCNetOutput control was created on.
			if (m_LogListBox.InvokeRequired) {
				UpdateLogListBoxDelegate update = new UpdateLogListBoxDelegate(AddToListbox);
				m_LogListBox.Invoke(update, msg);
			}
			else {
				m_LogListBox.Items.Add(msg);

				// Make sure the last item is made visible
				m_LogListBox.SelectedIndex = m_LogListBox.Items.Count - 1;
				m_LogListBox.ClearSelected();
			}
		}

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData) {
			if (keyData == Keys.Escape) {
				Close();
				return true;
			}
			else if(keyData == Keys.F5) {
				System.Diagnostics.Debug.WriteLine("F5 PRESSED IN MAIN WINDOW");
			}

			return base.ProcessCmdKey(ref msg, keyData);
		}
	}
}
