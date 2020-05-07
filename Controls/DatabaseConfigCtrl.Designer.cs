namespace RandREng.Controls
{
    partial class DatabaseConfigCtrl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ultraLabel1 = new System.Windows.Forms.Label();
            this.comboDatabases = new System.Windows.Forms.ComboBox();
            this.cbDontShowAgain = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // ultraLabel1
            // 
            this.ultraLabel1.Location = new System.Drawing.Point(35, 17);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(149, 23);
            this.ultraLabel1.TabIndex = 0;
            this.ultraLabel1.Text = "Select The Active Database";
            // 
            // comboDatabases
            // 
            this.comboDatabases.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboDatabases.FormattingEnabled = true;
            this.comboDatabases.Location = new System.Drawing.Point(16, 60);
            this.comboDatabases.Name = "comboDatabases";
            this.comboDatabases.Size = new System.Drawing.Size(184, 21);
            this.comboDatabases.TabIndex = 1;
            // 
            // cbDontShowAgain
            // 
            this.cbDontShowAgain.AutoSize = true;
            this.cbDontShowAgain.Location = new System.Drawing.Point(16, 102);
            this.cbDontShowAgain.Name = "cbDontShowAgain";
            this.cbDontShowAgain.Size = new System.Drawing.Size(111, 17);
            this.cbDontShowAgain.TabIndex = 3;
            this.cbDontShowAgain.Text = "Don\'t Show Again";
            this.cbDontShowAgain.UseVisualStyleBackColor = true;
            // 
            // DatabaseConfigCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cbDontShowAgain);
            this.Controls.Add(this.comboDatabases);
            this.Controls.Add(this.ultraLabel1);
            this.Name = "DatabaseConfigCtrl";
            this.Size = new System.Drawing.Size(216, 144);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label ultraLabel1;
        private System.Windows.Forms.ComboBox comboDatabases;
        private System.Windows.Forms.CheckBox cbDontShowAgain;
    }
}
