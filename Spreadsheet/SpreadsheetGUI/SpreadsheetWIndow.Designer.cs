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
            this.spreadsheetPanel = new SSGui.SpreadsheetPanel();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // spreadsheetPanel
            // 
            this.spreadsheetPanel.Location = new System.Drawing.Point(-3, -3);
            this.spreadsheetPanel.Margin = new System.Windows.Forms.Padding(2);
            this.spreadsheetPanel.Name = "spreadsheetPanel";
            this.spreadsheetPanel.Size = new System.Drawing.Size(492, 583);
            this.spreadsheetPanel.TabIndex = 0;
            this.spreadsheetPanel.Load += new System.EventHandler(this.spreadsheetPanel6_Load);
            this.spreadsheetPanel.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SpreadsheetWindow_KeyDown);
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(98, 591);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(223, 26);
            this.textBox1.TabIndex = 1;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 597);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(78, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "Contents:";
            // 
            // SpreadsheetWindow
            // 
            this.AccessibleRole = System.Windows.Forms.AccessibleRole.Link;
            this.ClientSize = new System.Drawing.Size(500, 700);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.spreadsheetPanel);
            this.Name = "SpreadsheetWindow";
            this.Load += new System.EventHandler(this.SpreadsheetWIndow_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SpreadsheetWindow_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private SSGui.SpreadsheetPanel spreadsheetPanel;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
    }
}

