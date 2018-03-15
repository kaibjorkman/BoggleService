using SSGui;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
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
        public event Action SelectionChanged;
        public event Action<string> EnterKeyPressed;
        public event Action<string> Save;
        public event Action<string> FileChosenEvent;
        public event Action Help;
        public event Action<FormClosingEventArgs> CloseEvent;

        //Fields

        /// <summary>
        /// panel interface
        /// </summary>
        public SpreadsheetPanel Panel
        {
            get { return spreadsheetPanel; }

            set { spreadsheetPanel = value; }
        }

        /// <summary>
        /// contents of selected cell
        /// </summary>
        public string Contents
        {
            set { textBox1.Text = value.ToString(); }
        }

        /// <summary>
        /// loads the window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// loads spreadsheet panel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void spreadsheetPanel6_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// loads a new spreadsheet window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SpreadsheetWIndow_Load(object sender, EventArgs e)
        {
          
        }

        /// <summary>
        /// loads the background worker
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                case Keys.Enter:
                    EnterKeyPressed(textBox1.Text);
                    break;
                   
            }
        }

        /// <summary>
        /// when new is clicked a new window is opened
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SpreadsheetApplicationContext.GetContext().RunNew();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.Yes || result == DialogResult.OK)
            {
                if (FileChosenEvent != null)
                {
                    FileChosenEvent(openFileDialog1.FileName);
                }
            }
        }

        public void openNew(TextReader reader)
        {
            SpreadsheetApplicationContext.GetContext().RunSaved(reader);
        }

        public void openNew()
        {
            throw new NotImplementedException();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = saveFileDialog1.ShowDialog();
            if (result == DialogResult.Yes || result == DialogResult.OK)
            {
                Save(saveFileDialog1.FileName);
                
            }
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Help();
        }

        private void SpreadsheetWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            CloseEvent(e);
        }
    }

        
    }

