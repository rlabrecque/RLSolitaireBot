using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using EasyHook;
using System.Runtime.Remoting.Channels.Ipc;
using System.Runtime.Remoting;
using System.Runtime.InteropServices;
using System.IO;
using Capture.Interface;
using Capture.Hook;
using Capture;

namespace SolitaireAI {
	public partial class Form1 : Form {
		Solitaire m_Solitaire;

		public Form1() {
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e) {
		}

		private void btnInject_Click(object sender, EventArgs e) {
			btnCapture.Enabled = false;

			if (_captureProcess == null) {
				AttachProcess();
			}
			else {
				DetachProcess();
			}
		}

		Process _process;
		CaptureProcess _captureProcess;
		private void AttachProcess() {
			btnInject.Enabled = false;

			string exeName = Path.GetFileNameWithoutExtension(textBox1.Text);
			Process[] processes = Process.GetProcessesByName(exeName);
			if (processes.Length == 0) {
				MessageBox.Show("No executable found matching: '" + exeName + "'");
				return;
			}

			// Simply attach to the first one found.
			Process process = processes[0];

			// If the process doesn't have a mainwindowhandle yet, skip it (we need to be able to get the hwnd to set foreground etc)
			if (process.MainWindowHandle == IntPtr.Zero) { return; }

			// Skip if the process is already hooked (and we want to hook multiple applications)
			if (HookManager.IsHooked(process.Id)) { return; }

			CaptureConfig cc = new CaptureConfig() { Direct3DVersion = Direct3DVersion.AutoDetect, ShowOverlay = cbDrawOverlay.Checked };

			_process = process;

			var captureInterface = new CaptureInterface();
			captureInterface.RemoteMessage += new MessageReceivedEvent(CaptureInterface_RemoteMessage);
			_captureProcess = new CaptureProcess(process, cc, captureInterface);

			Thread.Sleep(10);

			btnCapture.Enabled = true;
			btnInject.Text = "Detach";
			btnInject.Enabled = true;

			m_Solitaire = new Solitaire();
		}

		private void DetachProcess() {
			HookManager.RemoveHookedProcess(_captureProcess.Process.Id);
			_captureProcess.CaptureInterface.Disconnect();
			_captureProcess = null;

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
			DoRequest();
		}

		void DoRequest() {
			this.Invoke(new MethodInvoker(
				delegate () {
					_captureProcess.BringProcessWindowToFront();
					// Initiate the screenshot of the CaptureInterface, the appropriate event handler within the target process will take care of the rest
					Rectangle region = new Rectangle(int.Parse(txtCaptureX.Text), int.Parse(txtCaptureY.Text), int.Parse(txtCaptureWidth.Text), int.Parse(txtCaptureHeight.Text));
					ImageFormat format = (ImageFormat)Enum.Parse(typeof(ImageFormat), cmbFormat.Text);
					_captureProcess.CaptureInterface.BeginGetScreenshot(region, new TimeSpan(0, 0, 2), Callback, null, format);
				}
			));
		}

		void Callback(IAsyncResult result) {
			if (_captureProcess == null) {
				return;
			}

			using (Screenshot screenshot = _captureProcess.CaptureInterface.EndGetScreenshot(result))
				try {
					_captureProcess.CaptureInterface.DisplayInGameText("Screenshot captured...");

					if (screenshot != null && screenshot.Data != null) {
						pictureBox1.Invoke(new MethodInvoker(
							delegate () {
								if (pictureBox1.Image != null) {
									pictureBox1.Image.Dispose();
								}
								pictureBox1.Image = screenshot.ToBitmap();
							}
						));
					}

					Thread t = new Thread(new ThreadStart(DoRequest));
					t.Start();
				}
				catch {
				}
		}
	}
}
