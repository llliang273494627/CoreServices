
namespace NetForms.DSGTest
{
    partial class FrmError
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
            this.lstError = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // lstError
            // 
            this.lstError.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstError.Font = new System.Drawing.Font("Microsoft YaHei UI", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lstError.ForeColor = System.Drawing.Color.Red;
            this.lstError.FormattingEnabled = true;
            this.lstError.ItemHeight = 27;
            this.lstError.Items.AddRange(new object[] {
            "gfdgf",
            "asdfasdfas"});
            this.lstError.Location = new System.Drawing.Point(15, 15);
            this.lstError.Name = "lstError";
            this.lstError.Size = new System.Drawing.Size(582, 378);
            this.lstError.TabIndex = 0;
            // 
            // FrmError
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(612, 408);
            this.Controls.Add(this.lstError);
            this.MaximizeBox = false;
            this.Name = "FrmError";
            this.Padding = new System.Windows.Forms.Padding(15);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "提示";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.FrmError_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox lstError;
    }
}