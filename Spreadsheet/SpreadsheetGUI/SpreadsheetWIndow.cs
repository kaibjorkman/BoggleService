using SSGui;
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
        public event Action ArrowKeyRight;
        public event Action ArrowKeyUp;
        public event Action ArrowKeyDown;
        public event Action<string> CellContentsChanged;

        //Fields
        public SpreadsheetPanel Panel
        {
            get { return spreadsheetPanel; }

            set { spreadsheetPanel = value; }
        }

        
        

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void spreadsheetPanel6_Load(object sender, EventArgs e)
        {

        }

        
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
            
            switch (e.KeyCode)
            {
                case Keys.Left:
                    ArrowKeyLeft();
                    e.Handled = true;
                    break;          
                case Keys.Right:
                    ArrowKeyRight();
                    e.Handled = true;
                    break;
                case Keys.Up:
                    ArrowKeyUp();
                    e.Handled = true;
                    break;
                case Keys.Down:
                    ArrowKeyDown();
                    e.Handled = true;
                    break;
                   
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            CellContentsChanged(textBox1.Text);
        }
    }

        
    }

