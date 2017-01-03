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
			this.m_InjectButton = new System.Windows.Forms.Button();
			this.m_DebugVisualizer = new System.Windows.Forms.PictureBox();
			this.m_StateLabel = new System.Windows.Forms.Label();
			this.m_StateDisplayLabel = new System.Windows.Forms.Label();
			this.m_BotInfoComboBox = new System.Windows.Forms.ComboBox();
			this.m_SteppingEnabledCheckbox = new System.Windows.Forms.CheckBox();
			this.m_StepThisFrameButton = new System.Windows.Forms.Button();
			this.m_LogListBox = new System.Windows.Forms.ListBox();
			((System.ComponentModel.ISupportInitialize)(this.m_DebugVisualizer)).BeginInit();
			this.SuspendLayout();
			// 
			// m_InjectButton
			// 
			this.m_InjectButton.Location = new System.Drawing.Point(12, 39);
			this.m_InjectButton.Name = "m_InjectButton";
			this.m_InjectButton.Size = new System.Drawing.Size(121, 23);
			this.m_InjectButton.TabIndex = 0;
			this.m_InjectButton.Text = "Inject";
			this.m_InjectButton.UseVisualStyleBackColor = true;
			this.m_InjectButton.Click += new System.EventHandler(this.m_InjectButton_Click);
			// 
			// m_DebugVisualizer
			// 
			this.m_DebugVisualizer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.m_DebugVisualizer.Location = new System.Drawing.Point(139, 12);
			this.m_DebugVisualizer.Name = "m_DebugVisualizer";
			this.m_DebugVisualizer.Size = new System.Drawing.Size(1493, 601);
			this.m_DebugVisualizer.TabIndex = 2;
			this.m_DebugVisualizer.TabStop = false;
			// 
			// m_StateLabel
			// 
			this.m_StateLabel.AutoSize = true;
			this.m_StateLabel.Location = new System.Drawing.Point(9, 117);
			this.m_StateLabel.Name = "m_StateLabel";
			this.m_StateLabel.Size = new System.Drawing.Size(35, 13);
			this.m_StateLabel.TabIndex = 18;
			this.m_StateLabel.Text = "State:";
			// 
			// m_StateDisplayLabel
			// 
			this.m_StateDisplayLabel.AutoSize = true;
			this.m_StateDisplayLabel.Location = new System.Drawing.Point(9, 130);
			this.m_StateDisplayLabel.Name = "m_StateDisplayLabel";
			this.m_StateDisplayLabel.Size = new System.Drawing.Size(120, 13);
			this.m_StateDisplayLabel.TabIndex = 19;
			this.m_StateDisplayLabel.Text = "STATEDISPLAYLABEL";
			// 
			// m_BotInfoComboBox
			// 
			this.m_BotInfoComboBox.FormattingEnabled = true;
			this.m_BotInfoComboBox.Location = new System.Drawing.Point(12, 12);
			this.m_BotInfoComboBox.Name = "m_BotInfoComboBox";
			this.m_BotInfoComboBox.Size = new System.Drawing.Size(121, 21);
			this.m_BotInfoComboBox.TabIndex = 20;
			// 
			// m_SteppingEnabledCheckbox
			// 
			this.m_SteppingEnabledCheckbox.AutoSize = true;
			this.m_SteppingEnabledCheckbox.Enabled = false;
			this.m_SteppingEnabledCheckbox.Location = new System.Drawing.Point(12, 68);
			this.m_SteppingEnabledCheckbox.Name = "m_SteppingEnabledCheckbox";
			this.m_SteppingEnabledCheckbox.Size = new System.Drawing.Size(104, 17);
			this.m_SteppingEnabledCheckbox.TabIndex = 21;
			this.m_SteppingEnabledCheckbox.Text = "Enable Stepping";
			this.m_SteppingEnabledCheckbox.UseVisualStyleBackColor = true;
			this.m_SteppingEnabledCheckbox.CheckedChanged += new System.EventHandler(this.m_SteppingEnabledCheckbox_CheckedChanged);
			// 
			// m_StepThisFrameButton
			// 
			this.m_StepThisFrameButton.Enabled = false;
			this.m_StepThisFrameButton.Location = new System.Drawing.Point(12, 91);
			this.m_StepThisFrameButton.Name = "m_StepThisFrameButton";
			this.m_StepThisFrameButton.Size = new System.Drawing.Size(121, 23);
			this.m_StepThisFrameButton.TabIndex = 22;
			this.m_StepThisFrameButton.Text = "STEP";
			this.m_StepThisFrameButton.UseVisualStyleBackColor = true;
			this.m_StepThisFrameButton.Click += new System.EventHandler(this.m_StepThisFrame_Click);
			// 
			// m_LogListBox
			// 
			this.m_LogListBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.m_LogListBox.FormattingEnabled = true;
			this.m_LogListBox.Location = new System.Drawing.Point(139, 619);
			this.m_LogListBox.Name = "m_LogListBox";
			this.m_LogListBox.ScrollAlwaysVisible = true;
			this.m_LogListBox.Size = new System.Drawing.Size(1493, 108);
			this.m_LogListBox.TabIndex = 23;
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1644, 737);
			this.Controls.Add(this.m_LogListBox);
			this.Controls.Add(this.m_StepThisFrameButton);
			this.Controls.Add(this.m_SteppingEnabledCheckbox);
			this.Controls.Add(this.m_BotInfoComboBox);
			this.Controls.Add(this.m_StateDisplayLabel);
			this.Controls.Add(this.m_StateLabel);
			this.Controls.Add(this.m_DebugVisualizer);
			this.Controls.Add(this.m_InjectButton);
			this.Name = "Form1";
			this.Text = "SolitaireAI";
			this.Load += new System.EventHandler(this.Form1_Load);
			((System.ComponentModel.ISupportInitialize)(this.m_DebugVisualizer)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button m_InjectButton;
		private System.Windows.Forms.PictureBox m_DebugVisualizer;
		private System.Windows.Forms.Label m_StateLabel;
		private System.Windows.Forms.Label m_StateDisplayLabel;
		private System.Windows.Forms.ComboBox m_BotInfoComboBox;
		private System.Windows.Forms.CheckBox m_SteppingEnabledCheckbox;
		private System.Windows.Forms.Button m_StepThisFrameButton;
		private System.Windows.Forms.ListBox m_LogListBox;
	}
}

