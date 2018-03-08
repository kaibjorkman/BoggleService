namespace SpreadsheetGUI
{
    partial class SpreadsheetWIndow
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
            this.spreadsheetPanel3 = new SSGui.SpreadsheetPanel();
            this.SuspendLayout();
            // 
            // spreadsheetPanel3
            // 
            this.spreadsheetPanel3.Location = new System.Drawing.Point(12, 12);
            this.spreadsheetPanel3.Name = "spreadsheetPanel3";
            this.spreadsheetPanel3.Size = new System.Drawing.Size(1251, 611);
            this.spreadsheetPanel3.TabIndex = 0;
            // 
            // SpreadsheetWIndow
            // 
            this.ClientSize = new System.Drawing.Size(1251, 618);
            this.Controls.Add(this.spreadsheetPanel3);
            this.Name = "SpreadsheetWIndow";
            this.Load += new System.EventHandler(this.SpreadsheetWIndow_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private SSGui.SpreadsheetPanel spreadsheetPanel1;
        private System.Windows.Forms.Button button1;
        private SSGui.SpreadsheetPanel spreadsheetPanel2;
        private SSGui.SpreadsheetPanel spreadsheetPanel3;
    }
}

