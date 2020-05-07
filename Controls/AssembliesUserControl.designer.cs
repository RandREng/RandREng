namespace RandREng.Controls
{
    partial class AssembliesUserControl
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
            this.panelExContent = new System.Windows.Forms.Panel();
            this.listBoxAssemblies = new System.Windows.Forms.ListBox();
            this.panelExContent.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelExContent
            // 
            this.panelExContent.Controls.Add(this.listBoxAssemblies);
            this.panelExContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelExContent.Location = new System.Drawing.Point(0, 0);
            this.panelExContent.Name = "panelExContent";
            this.panelExContent.Size = new System.Drawing.Size(250, 131);
            this.panelExContent.TabIndex = 7;
            // 
            // listBoxAssemblies
            // 
            this.listBoxAssemblies.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxAssemblies.FormattingEnabled = true;
            this.listBoxAssemblies.HorizontalScrollbar = true;
            this.listBoxAssemblies.Location = new System.Drawing.Point(0, 0);
            this.listBoxAssemblies.Name = "listBoxAssemblies";
            this.listBoxAssemblies.Size = new System.Drawing.Size(250, 131);
            this.listBoxAssemblies.TabIndex = 0;
            // 
            // AssembliesUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelExContent);
            this.Name = "AssembliesUserControl";
            this.Size = new System.Drawing.Size(250, 131);
            this.panelExContent.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelExContent;
        private System.Windows.Forms.ListBox listBoxAssemblies;
    }
}
