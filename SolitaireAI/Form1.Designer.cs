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
			this.m_StateLabel = new System.Windows.Forms.Label();
			this.m_StateDisplayLabel = new System.Windows.Forms.Label();
			this.comboBox1 = new System.Windows.Forms.ComboBox();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.SuspendLayout();
			// 
			// btnInject
			// 
			this.btnInject.Location = new System.Drawing.Point(12, 39);
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
			this.pictureBox1.Location = new System.Drawing.Point(139, 12);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(680, 497);
			this.pictureBox1.TabIndex = 2;
			this.pictureBox1.TabStop = false;
			// 
			// m_StateLabel
			// 
			this.m_StateLabel.AutoSize = true;
			this.m_StateLabel.Location = new System.Drawing.Point(12, 65);
			this.m_StateLabel.Name = "m_StateLabel";
			this.m_StateLabel.Size = new System.Drawing.Size(35, 13);
			this.m_StateLabel.TabIndex = 18;
			this.m_StateLabel.Text = "State:";
			// 
			// m_StateDisplayLabel
			// 
			this.m_StateDisplayLabel.AutoSize = true;
			this.m_StateDisplayLabel.Location = new System.Drawing.Point(12, 78);
			this.m_StateDisplayLabel.Name = "m_StateDisplayLabel";
			this.m_StateDisplayLabel.Size = new System.Drawing.Size(120, 13);
			this.m_StateDisplayLabel.TabIndex = 19;
			this.m_StateDisplayLabel.Text = "STATEDISPLAYLABEL";
			// 
			// comboBox1
			// 
			this.comboBox1.FormattingEnabled = true;
			this.comboBox1.Location = new System.Drawing.Point(12, 12);
			this.comboBox1.Name = "comboBox1";
			this.comboBox1.Size = new System.Drawing.Size(121, 21);
			this.comboBox1.TabIndex = 20;
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(831, 521);
			this.Controls.Add(this.comboBox1);
			this.Controls.Add(this.m_StateDisplayLabel);
			this.Controls.Add(this.m_StateLabel);
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
		private System.Windows.Forms.Label m_StateLabel;
		private System.Windows.Forms.Label m_StateDisplayLabel;
		private System.Windows.Forms.ComboBox comboBox1;
	}
}

