namespace RandREng.Controls
{
    partial class ErrorForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panelEx = new System.Windows.Forms.Panel();
            this.labelError = new System.Windows.Forms.Label();
            this.btnDetails = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.labelHeading2 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.labelHeading1 = new System.Windows.Forms.Label();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCopy = new System.Windows.Forms.Button();
            this.textBox = new System.Windows.Forms.TextBox();
            this.buttonEmail = new System.Windows.Forms.Button();
            this.panelEx.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelEx
            // 
            this.panelEx.Controls.Add(this.buttonEmail);
            this.panelEx.Controls.Add(this.labelError);
            this.panelEx.Controls.Add(this.btnDetails);
            this.panelEx.Controls.Add(this.label3);
            this.panelEx.Controls.Add(this.labelHeading2);
            this.panelEx.Controls.Add(this.panel1);
            this.panelEx.Controls.Add(this.buttonOK);
            this.panelEx.Controls.Add(this.buttonCopy);
            this.panelEx.Controls.Add(this.textBox);
            this.panelEx.Location = new System.Drawing.Point(0, 0);
            this.panelEx.Name = "panelEx";
            this.panelEx.Size = new System.Drawing.Size(494, 362);
            this.panelEx.TabIndex = 0;
            // 
            // labelError
            // 
            this.labelError.AutoSize = true;
            this.labelError.Location = new System.Drawing.Point(2, 157);
            this.labelError.Name = "labelError";
            this.labelError.Size = new System.Drawing.Size(67, 13);
            this.labelError.TabIndex = 7;
            this.labelError.Text = "Error Details:";
            this.labelError.Visible = false;
            // 
            // btnDetails
            // 
            this.btnDetails.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDetails.Location = new System.Drawing.Point(293, 176);
            this.btnDetails.Name = "btnDetails";
            this.btnDetails.Size = new System.Drawing.Size(88, 23);
            this.btnDetails.TabIndex = 6;
            this.btnDetails.Text = "Details";
            this.btnDetails.UseVisualStyleBackColor = true;
            this.btnDetails.Click += new System.EventHandler(this.btnAdvanced_Click);
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(12, 93);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(398, 42);
            this.label3.TabIndex = 4;
            this.label3.Text = "We have created an error report that will help us diagnose this problem.  We will" +
    " treat this report as confidential and anonymous.  Please contact your support r" +
    "epresentative.";
            // 
            // labelHeading2
            // 
            this.labelHeading2.AutoSize = true;
            this.labelHeading2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelHeading2.Location = new System.Drawing.Point(12, 75);
            this.labelHeading2.Name = "labelHeading2";
            this.labelHeading2.Size = new System.Drawing.Size(272, 13);
            this.labelHeading2.TabIndex = 1;
            this.labelHeading2.Text = "Please tell Custom Flooring about this problem.";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.labelHeading1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(494, 60);
            this.panel1.TabIndex = 3;
            // 
            // labelHeading1
            // 
            this.labelHeading1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelHeading1.Location = new System.Drawing.Point(12, 17);
            this.labelHeading1.Name = "labelHeading1";
            this.labelHeading1.Size = new System.Drawing.Size(425, 27);
            this.labelHeading1.TabIndex = 0;
            this.labelHeading1.Text = " has experienced an error and must be closed. We apologize for the inconvenience";
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.Location = new System.Drawing.Point(397, 176);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(88, 23);
            this.buttonOK.TabIndex = 2;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonCopy
            // 
            this.buttonCopy.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.buttonCopy.Location = new System.Drawing.Point(85, 376);
            this.buttonCopy.Name = "buttonCopy";
            this.buttonCopy.Size = new System.Drawing.Size(88, 23);
            this.buttonCopy.TabIndex = 1;
            this.buttonCopy.Text = "Copy";
            this.buttonCopy.UseVisualStyleBackColor = true;
            this.buttonCopy.Click += new System.EventHandler(this.buttonCopy_Click);
            // 
            // textBox
            // 
            this.textBox.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.textBox.Location = new System.Drawing.Point(4, 205);
            this.textBox.Multiline = true;
            this.textBox.Name = "textBox";
            this.textBox.ReadOnly = true;
            this.textBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox.Size = new System.Drawing.Size(487, 155);
            this.textBox.TabIndex = 0;
            // 
            // buttonEmail
            // 
            this.buttonEmail.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonEmail.Location = new System.Drawing.Point(189, 176);
            this.buttonEmail.Name = "buttonEmail";
            this.buttonEmail.Size = new System.Drawing.Size(88, 23);
            this.buttonEmail.TabIndex = 8;
            this.buttonEmail.Text = "Email Support";
            this.buttonEmail.UseVisualStyleBackColor = true;
            this.buttonEmail.Click += new System.EventHandler(this.buttonEmail_Click);
            // 
            // ErrorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(494, 204);
            this.Controls.Add(this.panelEx);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ErrorForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Load += new System.EventHandler(this.ErrorForm_Load);
            this.panelEx.ResumeLayout(false);
            this.panelEx.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelEx;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCopy;
        private System.Windows.Forms.TextBox textBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label labelHeading2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label labelHeading1;
        private System.Windows.Forms.Button btnDetails;
        private System.Windows.Forms.Label labelError;
        private System.Windows.Forms.Button buttonEmail;
    }
}