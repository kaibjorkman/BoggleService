using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpreadsheetGUI
{
    public partial class SpreadsheetWindow : Form, ISpreadsheetView

    {
        public SpreadsheetWindow()
        {
            InitializeComponent();
        }

        //All events
        public event Action ArrowKeyLeft;

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void spreadsheetPanel6_Load(object sender, EventArgs e)
        {

        }

        /**
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // SpreadsheetWIndow
            // 
            this.ClientSize = new System.Drawing.Size(500, 700);
            this.Name = "SpreadsheetWIndow";
            this.Load += new System.EventHandler(this.SpreadsheetWIndow_Load);
            this.ResumeLayout(false);

        }
    **/
        private void SpreadsheetWIndow_Load(object sender, EventArgs e)
        {

        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {

        }

        //Event Handlers

        /// <summary>
        /// Handles all Key Strokes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SpreadsheetWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Left)
            {
                ArrowKeyLeft?.Invoke();
            }
        }

        
    }
}
