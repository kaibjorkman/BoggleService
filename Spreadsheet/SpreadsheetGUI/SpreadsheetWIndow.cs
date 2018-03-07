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
    public partial class SpreadsheetWIndow : Form
    {
        public SpreadsheetWIndow()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void spreadsheetPanel1_Load(object sender, EventArgs e)
        {

        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // SpreadsheetWIndow
            // 
            this.ClientSize = new System.Drawing.Size(274, 229);
            this.Name = "SpreadsheetWIndow";
            this.Load += new System.EventHandler(this.SpreadsheetWIndow_Load);
            this.ResumeLayout(false);

        }

        private void SpreadsheetWIndow_Load(object sender, EventArgs e)
        {

        }
    }
}
