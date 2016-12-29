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
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.txtDebugLog = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.m_StateLabel = new System.Windows.Forms.Label();
			this.m_StateDisplayLabel = new System.Windows.Forms.Label();
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
			// pictureBox1
			// 
			this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.pictureBox1.Location = new System.Drawing.Point(193, 13);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(626, 400);
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
			// txtDebugLog
			// 
			this.txtDebugLog.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtDebugLog.Location = new System.Drawing.Point(5, 419);
			this.txtDebugLog.Multiline = true;
			this.txtDebugLog.Name = "txtDebugLog";
			this.txtDebugLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.txtDebugLog.Size = new System.Drawing.Size(814, 90);
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
			// m_StateLabel
			// 
			this.m_StateLabel.AutoSize = true;
			this.m_StateLabel.Location = new System.Drawing.Point(5, 84);
			this.m_StateLabel.Name = "m_StateLabel";
			this.m_StateLabel.Size = new System.Drawing.Size(35, 13);
			this.m_StateLabel.TabIndex = 18;
			this.m_StateLabel.Text = "State:";
			// 
			// m_StateDisplayLabel
			// 
			this.m_StateDisplayLabel.AutoSize = true;
			this.m_StateDisplayLabel.Location = new System.Drawing.Point(5, 97);
			this.m_StateDisplayLabel.Name = "m_StateDisplayLabel";
			this.m_StateDisplayLabel.Size = new System.Drawing.Size(0, 13);
			this.m_StateDisplayLabel.TabIndex = 19;
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(831, 521);
			this.Controls.Add(this.m_StateDisplayLabel);
			this.Controls.Add(this.m_StateLabel);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.txtDebugLog);
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.pictureBox1);
			this.Controls.Add(this.btnInject);
			this.Name = "Form1";
			this.Text = "SolitaireAI";
			this.Load += new System.EventHandler(this.Form1_Load);
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnInject;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.TextBox txtDebugLog;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label m_StateLabel;
		private System.Windows.Forms.Label m_StateDisplayLabel;
	}
}

