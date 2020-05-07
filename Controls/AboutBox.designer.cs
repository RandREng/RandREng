namespace RandREng.Controls
{
	partial class AboutBox
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutBox));
            this.buttonOK = new System.Windows.Forms.Button();
            this.labelAppName = new System.Windows.Forms.Label();
            this.labelVersion = new System.Windows.Forms.Label();
            this.labelExtraInfo = new System.Windows.Forms.Label();
            this.labelWarning = new System.Windows.Forms.Label();
            this.labelCopyright = new System.Windows.Forms.Label();
            this.m_btSystemInfo = new System.Windows.Forms.Button();
            this.buttonCopyToClipBoard = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.linkRandR = new System.Windows.Forms.LinkLabel();
            this.assembliesUserControl1 = new RandREng.Controls.AssembliesUserControl();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.buttonOK.Location = new System.Drawing.Point(361, 519);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(176, 46);
            this.buttonOK.TabIndex = 0;
            this.buttonOK.Text = "OK";
            // 
            // labelAppName
            // 
            this.labelAppName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelAppName.BackColor = System.Drawing.Color.Transparent;
            this.labelAppName.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.labelAppName.Location = new System.Drawing.Point(425, 31);
            this.labelAppName.Name = "labelAppName";
            this.labelAppName.Size = new System.Drawing.Size(107, 54);
            this.labelAppName.TabIndex = 1;
            this.labelAppName.Text = "Application Name";
            this.labelAppName.UseMnemonic = false;
            // 
            // labelVersion
            // 
            this.labelVersion.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelVersion.BackColor = System.Drawing.Color.Transparent;
            this.labelVersion.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.labelVersion.Location = new System.Drawing.Point(425, 77);
            this.labelVersion.Name = "labelVersion";
            this.labelVersion.Size = new System.Drawing.Size(107, 31);
            this.labelVersion.TabIndex = 2;
            this.labelVersion.Text = "Version 1.0.0";
            this.labelVersion.UseMnemonic = false;
            // 
            // labelExtraInfo
            // 
            this.labelExtraInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelExtraInfo.BackColor = System.Drawing.Color.Transparent;
            this.labelExtraInfo.Location = new System.Drawing.Point(425, 108);
            this.labelExtraInfo.Name = "labelExtraInfo";
            this.labelExtraInfo.Size = new System.Drawing.Size(107, 0);
            this.labelExtraInfo.TabIndex = 4;
            this.labelExtraInfo.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.labelExtraInfo.UseMnemonic = false;
            this.labelExtraInfo.Visible = false;
            // 
            // labelWarning
            // 
            this.labelWarning.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelWarning.BackColor = System.Drawing.Color.Transparent;
            this.labelWarning.Font = new System.Drawing.Font("Tahoma", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.labelWarning.Location = new System.Drawing.Point(48, 534);
            this.labelWarning.Name = "labelWarning";
            this.labelWarning.Size = new System.Drawing.Size(260, 93);
            this.labelWarning.TabIndex = 6;
            this.labelWarning.Text = resources.GetString("labelWarning.Text");
            this.labelWarning.UseMnemonic = false;
            // 
            // labelCopyright
            // 
            this.labelCopyright.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelCopyright.BackColor = System.Drawing.Color.Transparent;
            this.labelCopyright.Location = new System.Drawing.Point(425, 108);
            this.labelCopyright.Name = "labelCopyright";
            this.labelCopyright.Size = new System.Drawing.Size(107, 31);
            this.labelCopyright.TabIndex = 3;
            this.labelCopyright.Text = "Copyright © 2011 by Custom Flooring Installations, Inc.";
            this.labelCopyright.UseMnemonic = false;
            // 
            // m_btSystemInfo
            // 
            this.m_btSystemInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.m_btSystemInfo.BackColor = System.Drawing.SystemColors.Control;
            this.m_btSystemInfo.Location = new System.Drawing.Point(361, 577);
            this.m_btSystemInfo.Name = "m_btSystemInfo";
            this.m_btSystemInfo.Size = new System.Drawing.Size(176, 46);
            this.m_btSystemInfo.TabIndex = 1;
            this.m_btSystemInfo.Text = "System Info";
            this.m_btSystemInfo.UseVisualStyleBackColor = false;
            this.m_btSystemInfo.Click += new System.EventHandler(this.m_btSystemInfo_Click);
            // 
            // buttonCopyToClipBoard
            // 
            this.buttonCopyToClipBoard.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCopyToClipBoard.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.buttonCopyToClipBoard.Location = new System.Drawing.Point(361, 635);
            this.buttonCopyToClipBoard.Name = "buttonCopyToClipBoard";
            this.buttonCopyToClipBoard.Size = new System.Drawing.Size(176, 46);
            this.buttonCopyToClipBoard.TabIndex = 0;
            this.buttonCopyToClipBoard.Text = "Copy Info";
            this.buttonCopyToClipBoard.Click += new System.EventHandler(this.buttonCopyToClipBoard_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(31, 31);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(380, 96);
            this.pictureBox1.TabIndex = 9;
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(24, -478);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(222, 27);
            this.label1.TabIndex = 10;
            this.label1.Text = "Loaded Assemblies";
            // 
            // linkCFI
            // 
            this.linkRandR.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.linkRandR.AutoSize = true;
            this.linkRandR.Location = new System.Drawing.Point(46, 646);
            this.linkRandR.Name = "linkRandR";
            this.linkRandR.Size = new System.Drawing.Size(272, 27);
            this.linkRandR.TabIndex = 13;
            this.linkRandR.TabStop = true;
            this.linkRandR.Text = "www.randreng.com";
            this.linkRandR.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.link_LinkClicked);
            // 
            // assembliesUserControl1
            // 
            this.assembliesUserControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.assembliesUserControl1.Location = new System.Drawing.Point(14, 100);
            this.assembliesUserControl1.Name = "assembliesUserControl1";
            this.assembliesUserControl1.Size = new System.Drawing.Size(533, 486);
            this.assembliesUserControl1.TabIndex = 12;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(425, 139);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(107, 31);
            this.label2.TabIndex = 14;
            this.label2.Text = "All Rights Reserved.";
            // 
            // AboutBox
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(11, 27);
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(559, 689);
            this.ControlBox = false;
            this.Controls.Add(this.label2);
            this.Controls.Add(this.linkRandR);
            this.Controls.Add(this.assembliesUserControl1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.buttonCopyToClipBoard);
            this.Controls.Add(this.m_btSystemInfo);
            this.Controls.Add(this.labelCopyright);
            this.Controls.Add(this.labelWarning);
            this.Controls.Add(this.labelExtraInfo);
            this.Controls.Add(this.labelVersion);
            this.Controls.Add(this.labelAppName);
            this.Controls.Add(this.buttonOK);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutBox";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "About Application Name";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

    
        private System.Windows.Forms.Button buttonOK;
        protected System.Windows.Forms.Label labelVersion;
        protected System.Windows.Forms.Label labelAppName;
        protected System.Windows.Forms.Label labelExtraInfo;
        private System.Windows.Forms.Label labelWarning;
        protected System.Windows.Forms.Label labelCopyright;
        private System.Windows.Forms.Button m_btSystemInfo;
        private System.Windows.Forms.Button buttonCopyToClipBoard;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private AssembliesUserControl assembliesUserControl1;
        private System.Windows.Forms.LinkLabel linkRandR;
        protected System.Windows.Forms.Label label2;
    }
}
