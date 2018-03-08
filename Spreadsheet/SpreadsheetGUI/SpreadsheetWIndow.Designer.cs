namespace SpreadsheetGUI
{
    partial class SpreadsheetWindow : ISpreadsheetView
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
            this.spreadsheetPanel6 = new SSGui.SpreadsheetPanel();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.SuspendLayout();
            // 
            // spreadsheetPanel6
            // 
            this.spreadsheetPanel6.Location = new System.Drawing.Point(-3, -1);
            this.spreadsheetPanel6.Margin = new System.Windows.Forms.Padding(2);
            this.spreadsheetPanel6.Name = "spreadsheetPanel6";
            this.spreadsheetPanel6.Size = new System.Drawing.Size(492, 583);
            this.spreadsheetPanel6.TabIndex = 0;
            this.spreadsheetPanel6.Load += new System.EventHandler(this.spreadsheetPanel6_Load);
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            // 
            // SpreadsheetWindow
            // 
            this.AccessibleRole = System.Windows.Forms.AccessibleRole.Link;
            this.ClientSize = new System.Drawing.Size(500, 578);
            this.Controls.Add(this.spreadsheetPanel6);
            this.Name = "SpreadsheetWindow";
            this.Load += new System.EventHandler(this.SpreadsheetWIndow_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SpreadsheetWindow_KeyDown);
            this.ResumeLayout(false);

        }

        #endregion
        private SSGui.SpreadsheetPanel spreadsheetPanel6;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
    }
}

