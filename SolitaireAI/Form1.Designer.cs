namespace SolitaireAI {
	partial class Form1 {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.btnInject = new System.Windows.Forms.Button();
			this.btnCapture = new System.Windows.Forms.Button();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.txtCaptureX = new System.Windows.Forms.TextBox();
			this.txtCaptureY = new System.Windows.Forms.TextBox();
			this.txtCaptureWidth = new System.Windows.Forms.TextBox();
			this.txtCaptureHeight = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.txtDebugLog = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.cbDrawOverlay = new System.Windows.Forms.CheckBox();
			this.label8 = new System.Windows.Forms.Label();
			this.label12 = new System.Windows.Forms.Label();
			this.cmbFormat = new System.Windows.Forms.ComboBox();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.SuspendLayout();
			// 
			// btnInject
			// 
			this.btnInject.Location = new System.Drawing.Point(8, 58);
			this.btnInject.Name = "btnInject";
			this.btnInject.Size = new System.Drawing.Size(98, 23);
			this.btnInject.TabIndex = 0;
			this.btnInject.Text = "Inject";
			this.btnInject.UseVisualStyleBackColor = true;
			this.btnInject.Click += new System.EventHandler(this.btnInject_Click);
			// 
			// btnCapture
			// 
			this.btnCapture.Enabled = false;
			this.btnCapture.Location = new System.Drawing.Point(112, 32);
			this.btnCapture.Name = "btnCapture";
			this.btnCapture.Size = new System.Drawing.Size(75, 49);
			this.btnCapture.TabIndex = 1;
			this.btnCapture.Text = "Run";
			this.btnCapture.UseVisualStyleBackColor = true;
			this.btnCapture.Click += new System.EventHandler(this.btnCapture_Click);
			// 
			// pictureBox1
			// 
			this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
			| System.Windows.Forms.AnchorStyles.Left)
			| System.Windows.Forms.AnchorStyles.Right)));
			this.pictureBox1.Location = new System.Drawing.Point(193, 13);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(797, 405);
			this.pictureBox1.TabIndex = 2;
			this.pictureBox1.TabStop = false;
			// 
			// textBox1
			// 
			this.textBox1.Location = new System.Drawing.Point(8, 32);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(98, 20);
			this.textBox1.TabIndex = 6;
			this.textBox1.Text = "Solitaire.exe";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(5, 135);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(14, 13);
			this.label1.TabIndex = 7;
			this.label1.Text = "X";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(119, 135);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(14, 13);
			this.label2.TabIndex = 8;
			this.label2.Text = "Y";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(5, 161);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(35, 13);
			this.label3.TabIndex = 9;
			this.label3.Text = "Width";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(96, 161);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(38, 13);
			this.label4.TabIndex = 10;
			this.label4.Text = "Height";
			// 
			// txtCaptureX
			// 
			this.txtCaptureX.Location = new System.Drawing.Point(48, 132);
			this.txtCaptureX.Name = "txtCaptureX";
			this.txtCaptureX.Size = new System.Drawing.Size(47, 20);
			this.txtCaptureX.TabIndex = 11;
			this.txtCaptureX.Text = "0";
			// 
			// txtCaptureY
			// 
			this.txtCaptureY.Location = new System.Drawing.Point(139, 132);
			this.txtCaptureY.Name = "txtCaptureY";
			this.txtCaptureY.Size = new System.Drawing.Size(47, 20);
			this.txtCaptureY.TabIndex = 12;
			this.txtCaptureY.Text = "0";
			// 
			// txtCaptureWidth
			// 
			this.txtCaptureWidth.Location = new System.Drawing.Point(48, 158);
			this.txtCaptureWidth.Name = "txtCaptureWidth";
			this.txtCaptureWidth.Size = new System.Drawing.Size(47, 20);
			this.txtCaptureWidth.TabIndex = 13;
			this.txtCaptureWidth.Text = "0";
			// 
			// txtCaptureHeight
			// 
			this.txtCaptureHeight.Location = new System.Drawing.Point(139, 158);
			this.txtCaptureHeight.Name = "txtCaptureHeight";
			this.txtCaptureHeight.Size = new System.Drawing.Size(47, 20);
			this.txtCaptureHeight.TabIndex = 14;
			this.txtCaptureHeight.Text = "0";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(5, 184);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(184, 13);
			this.label5.TabIndex = 15;
			this.label5.Text = "Width of 0 means capture full window";
			// 
			// txtDebugLog
			// 
			this.txtDebugLog.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
			| System.Windows.Forms.AnchorStyles.Right)));
			this.txtDebugLog.Location = new System.Drawing.Point(5, 424);
			this.txtDebugLog.Multiline = true;
			this.txtDebugLog.Name = "txtDebugLog";
			this.txtDebugLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.txtDebugLog.Size = new System.Drawing.Size(985, 90);
			this.txtDebugLog.TabIndex = 16;
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(5, 16);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(174, 13);
			this.label6.TabIndex = 17;
			this.label6.Text = "EXE Name of Direct3D Application:";
			// 
			// cbDrawOverlay
			// 
			this.cbDrawOverlay.AutoSize = true;
			this.cbDrawOverlay.Checked = true;
			this.cbDrawOverlay.CheckState = System.Windows.Forms.CheckState.Checked;
			this.cbDrawOverlay.Location = new System.Drawing.Point(8, 230);
			this.cbDrawOverlay.Name = "cbDrawOverlay";
			this.cbDrawOverlay.Size = new System.Drawing.Size(90, 17);
			this.cbDrawOverlay.TabIndex = 26;
			this.cbDrawOverlay.Text = "Draw Overlay";
			this.cbDrawOverlay.UseVisualStyleBackColor = true;
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(3, 113);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(52, 13);
			this.label8.TabIndex = 27;
			this.label8.Text = "REGION:";
			// 
			// label12
			// 
			this.label12.AutoSize = true;
			this.label12.Location = new System.Drawing.Point(5, 206);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(55, 13);
			this.label12.TabIndex = 33;
			this.label12.Text = "FORMAT:";
			// 
			// cmbFormat
			// 
			this.cmbFormat.FormattingEnabled = true;
			this.cmbFormat.Items.AddRange(new object[] {
			"Bitmap",
			"Jpeg",
			"Png",
			"PixelData"});
			this.cmbFormat.Location = new System.Drawing.Point(65, 203);
			this.cmbFormat.Name = "cmbFormat";
			this.cmbFormat.Size = new System.Drawing.Size(121, 21);
			this.cmbFormat.TabIndex = 34;
			this.cmbFormat.Text = "Bitmap";
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1002, 526);
			this.Controls.Add(this.cmbFormat);
			this.Controls.Add(this.label12);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.cbDrawOverlay);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.txtDebugLog);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.txtCaptureHeight);
			this.Controls.Add(this.txtCaptureWidth);
			this.Controls.Add(this.txtCaptureY);
			this.Controls.Add(this.txtCaptureX);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.pictureBox1);
			this.Controls.Add(this.btnCapture);
			this.Controls.Add(this.btnInject);
			this.Name = "Form1";
			this.Text = "Test Screenshot Direct3D API Hook";
			this.Load += new System.EventHandler(this.Form1_Load);
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnInject;
		private System.Windows.Forms.Button btnCapture;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox txtCaptureX;
		private System.Windows.Forms.TextBox txtCaptureY;
		private System.Windows.Forms.TextBox txtCaptureWidth;
		private System.Windows.Forms.TextBox txtCaptureHeight;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox txtDebugLog;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.CheckBox cbDrawOverlay;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.ComboBox cmbFormat;
	}
}

