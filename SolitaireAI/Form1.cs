using System;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using System.IO;
using System.Collections;
using Capture.Interface;
using Capture.Hook;
using Capture;
using System.Text;
using System.Collections.Generic;

namespace SolitaireAI {
	public partial class Form1 : Form {
		Solitaire m_Solitaire;
		CaptureProcess m_captureProcess;

		public Form1() {
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e) {
			AttachProcess();
		}

		private void btnInject_Click(object sender, EventArgs e) {
			if (m_captureProcess == null) {
				AttachProcess();
			}
			else {
				DetachProcess();
			}
		}
		
		private void AttachProcess() {
			btnInject.Enabled = false;

			string exeName = Path.GetFileNameWithoutExtension(textBox1.Text);
			Process[] processes = Process.GetProcessesByName(exeName);
			if (processes.Length == 0) {
				MessageBox.Show("No executable found matching: '" + exeName + "'");
				btnInject.Enabled = true;
				return;
			}
			
			Process process = processes[0];
			
			// If the process doesn't have a mainwindowhandle yet, skip it (we need to be able to get the hwnd to set foreground etc)
			if (process.MainWindowHandle == IntPtr.Zero) { return; }

			// Skip if the process is already hooked (and we want to hook multiple applications)
			if (HookManager.IsHooked(process.Id)) { return; }

			CaptureConfig cc = new CaptureConfig() { Direct3DVersion = Direct3DVersion.AutoDetect, ShowOverlay = true };
			
			var captureInterface = new CaptureInterface();
			captureInterface.RemoteMessage += new MessageReceivedEvent(CaptureInterface_RemoteMessage);
			m_captureProcess = new CaptureProcess(process, cc, captureInterface);

			Thread.Sleep(10);

			btnInject.Text = "Detach";
			btnInject.Enabled = true;

			m_Solitaire = new Solitaire();
			
			CaptureScreenshot();
		}

		private void DetachProcess() {
			HookManager.RemoveHookedProcess(m_captureProcess.Process.Id);
			m_captureProcess.CaptureInterface.Disconnect();
			m_captureProcess = null;

			btnInject.Text = "Inject";
			btnInject.Enabled = true;
		}

		void CaptureInterface_RemoteMessage(MessageReceivedEventArgs message) {
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
		}

		private void btnCapture_Click(object sender, EventArgs e) {
			CaptureScreenshot();
		}

		void CaptureScreenshot() {
			this.Invoke(new MethodInvoker(
				delegate () {
					//m_captureProcess.BringProcessWindowToFront();
					// Initiate the screenshot of the CaptureInterface, the appropriate event handler within the target process will take care of the rest
					Rectangle region = new Rectangle(0, 0, 0, 0);
					TimeSpan timeout = new TimeSpan(0, 0, 1);
					ImageFormat format = ImageFormat.Bitmap;
					m_captureProcess.CaptureInterface.BeginGetScreenshot(region, timeout, ScreenshotCallback, null, format);
				}
			));
		}

		void ScreenshotCallback(IAsyncResult result) {
			if (m_captureProcess == null) {
				return;
			}

			using (Screenshot screenshot = m_captureProcess.CaptureInterface.EndGetScreenshot(result)) {
				try {
					m_captureProcess.CaptureInterface.DisplayInGameText("Screenshot captured...");

					if (screenshot != null && screenshot.Data != null) {
						pictureBox1.Invoke(new MethodInvoker(
							delegate () {
								if (pictureBox1.Image != null) {
									pictureBox1.Image.Dispose();
								}
								pictureBox1.Image = m_Solitaire.OnScreenshot(screenshot.ToBitmap());
								m_StateDisplayLabel.Text = m_Solitaire.GetState();
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
	}
}
