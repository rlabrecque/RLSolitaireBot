﻿using System;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using System.IO;
using Capture.Interface;
using Capture.Hook;
using Capture;

namespace SolitaireAI {
	public partial class Form1 : Form {
		IBot m_Bot;
		CaptureProcess m_captureProcess;

		public Form1() {
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e) {
			comboBox1.Items.Add(new SolitaireInfo());
			comboBox1.SelectedIndex = 0;

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

			string exeName = Path.GetFileNameWithoutExtension(((IBotInfo)comboBox1.SelectedItem).GetExecutableName);
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
			
			//captureInterface.RemoteMessage += new MessageReceivedEvent(CaptureInterface_RemoteMessage);
			m_captureProcess = new CaptureProcess(process, cc, new CaptureInterface());

			Thread.Sleep(10);

			btnInject.Text = "Detach";
			btnInject.Enabled = true;

			m_Bot = ((IBotInfo)comboBox1.SelectedItem).GetBot;

			m_Bot.OnAttach();
			
			CaptureScreenshot();

			myTimer.Tick += new EventHandler(TimerEventProcessor);
			myTimer.Interval = 100;
			myTimer.Start();
		}

		System.Windows.Forms.Timer myTimer = new System.Windows.Forms.Timer();
		void TimerEventProcessor(Object myObject, EventArgs myEventArgs) {
			const UInt32 WM_PAINT = 0x000F;
			IntPtr dc = NativeMethods.GetDC(m_captureProcess.Process.MainWindowHandle);
			NativeMethods.PostMessage(m_captureProcess.Process.MainWindowHandle, WM_PAINT, dc, IntPtr.Zero);
			NativeMethods.ReleaseDC(m_captureProcess.Process.MainWindowHandle, dc);
		}

		private void DetachProcess() {
			myTimer.Stop();
			m_Bot.OnDetach();
			HookManager.RemoveHookedProcess(m_captureProcess.Process.Id);
			m_captureProcess.CaptureInterface.Disconnect();
			m_captureProcess = null;

			btnInject.Text = "Inject";
			btnInject.Enabled = true;
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
			this.Invoke(new MethodInvoker(
				delegate () {
					m_captureProcess.CaptureInterface.BeginGetScreenshot(Rectangle.Empty, new TimeSpan(0, 0, 1), ScreenshotCallback, null, format: ImageFormat.PixelData);
				}
			));
		}

		void ScreenshotCallback(IAsyncResult result) {
			if (m_captureProcess == null) {
				return;
			}

			using (Screenshot screenshot = m_captureProcess.CaptureInterface.EndGetScreenshot(result)) {
				try {
					if (screenshot != null && screenshot.Data != null) {
						pictureBox1.Invoke(new MethodInvoker(
							delegate () {
								if (pictureBox1.Image != null) {
									pictureBox1.Image.Dispose();
								}
								pictureBox1.Image = m_Bot.OnGameFrame(screenshot.Data,  new Size(screenshot.Width, screenshot.Height), screenshot.Stride);
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
	}
}
