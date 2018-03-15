using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using SS;
using SSGui;

namespace SpreadsheetGUI
{
    public class Controller
    {
        private ISpreadsheetView window;
        public Spreadsheet spreadsheet;
        

        public Controller(ISpreadsheetView window)
        {
            this.window = window;
            spreadsheet = new Spreadsheet();
            ///Arrow Keys
            window.ArrowKeyLeft += HandleArrowKeyLeft;
            window.ArrowKeyRight += HandleArrowKeyRight;
            window.ArrowKeyUp += HandleArrowKeyUp;
            window.ArrowKeyDown += HandleArrowKeyDown;
            window.Panel.SelectionChanged += SelectionChangedHandler;
            window.EnterKeyPressed += EnterKeyHandler;
            window.FileChosenEvent += FileChosenHandler;
            window.Save += SaveHandler;
            window.Help += HelpHandler;
            window.CloseEvent += CloseHandler;
        }



        public Controller(SpreadsheetWindow window, TextReader reader)
        {
            this.window = window;
            this.spreadsheet = new Spreadsheet(reader, new Regex("^[a-zA-Z]*[1-9][0-9]*$"));
            this.UpdateDependents(spreadsheet.GetNamesOfAllNonemptyCells());
            ///Arrow Keys
            window.ArrowKeyLeft += HandleArrowKeyLeft;
            window.ArrowKeyRight += HandleArrowKeyRight;
            window.ArrowKeyUp += HandleArrowKeyUp;
            window.ArrowKeyDown += HandleArrowKeyDown;
            window.Panel.SelectionChanged += SelectionChangedHandler;
            window.EnterKeyPressed += EnterKeyHandler;
            window.FileChosenEvent += FileChosenHandler;
            window.Help += HelpHandler;
        }
        /// <summary>
        /// Handlse when Arrow key is hit
        /// </summary>
        private void HandleArrowKeyLeft()
        {
            window.Panel.GetSelection(out int col, out int row);

            window.Panel.SetSelection(col - 1, row);
           
        }


        /// <summary>
        /// Handlse when Arrow key is hit
        /// </summary>
        private void HandleArrowKeyRight()
        {
            window.Panel.GetSelection(out int col, out int row);

            window.Panel.SetSelection(col + 1, row);

        }

        /// <summary>
        /// Handlse when Arrow key is hit
        /// </summary>
        private void HandleArrowKeyUp()
        {
            window.Panel.GetSelection(out int col, out int row);

            window.Panel.SetSelection(col, row - 1);

        }

        /// <summary>
        /// Handlse when Arrow key is hit
        /// </summary>
        private void HandleArrowKeyDown()
        {
            window.Panel.GetSelection(out int col, out int row);

            window.Panel.SetSelection(col, row + 1);

        }

        private void EnterKeyHandler(string contents)
        {
            window.Panel.GetSelection(out int col, out int row);

            //make name
            int temp = col + 65;
            char letter = Convert.ToChar(temp);
            string name = letter + (row + 1).ToString();

            //get dependents and value from cell
            try
            {
                HashSet<string> dependents = (HashSet<string>)spreadsheet.SetContentsOfCell(name, contents);
                object value1 = spreadsheet.GetCellValue(name);

                window.Panel.SetValue(col, row, value1.ToString());

                this.UpdateDependents(dependents);
            }
            catch (Exception e)
            {
                string message = e.Message;
                string caption = "Formula Error";
                MessageBoxButtons buttons = MessageBoxButtons.OK;

                MessageBox.Show(message, caption, buttons);
            }
        }

        public void UpdateDependents(IEnumerable<string> set)
        {
            foreach(string cell in set)
            {
                object value = spreadsheet.GetCellValue(cell);
                int col = Char.ToUpper(cell[0]) - 65;
                int row = (int)(Char.GetNumericValue(cell[1]) - 1);

               
                window.Panel.SetValue(col, row, value.ToString());
            }

        }


        private void SelectionChangedHandler(SpreadsheetPanel ss)
        {
            window.Panel.GetSelection(out int col, out int row);

            //make name
            int temp = col + 65;
            char letter = Convert.ToChar(temp);
            string name = letter + (row + 1).ToString();

            window.Contents = spreadsheet.GetCellContents(name).ToString();

            window.Panel.Focus();
        }
        
        private void FileChosenHandler(string filename)
        {
            try
            {
                using (TextReader reader = File.OpenText(filename))
                {
                   
                    window.openNew(reader);
                }
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                string caption = "File Load Error";
                MessageBoxButtons buttons = MessageBoxButtons.OK;

                MessageBox.Show(message, caption, buttons);
            }
        }

        private void SaveHandler(string dest)
        {
           try
            {
                using (TextWriter writer = new StreamWriter(dest))
                {
                    spreadsheet.Save(writer);

                }
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                string caption = "File Save Error";
                MessageBoxButtons buttons = MessageBoxButtons.OK;

                MessageBox.Show(message, caption, buttons);
            }
        }

        private void HelpHandler()
        {
            string message = "This is a standard spreadsheet. All formulas must be proceeded by an equals sign. " +
                "To edit the cell contnents, select the cell, then click and type in your addition followed by a hit of the enter key." +
                " There is a drop down menu with a file, save, and open function. There is a drop down menu with a save, open , and new functions. ";
            
            string caption = "How to Use: ";
            MessageBoxButtons buttons = MessageBoxButtons.OK;

            MessageBox.Show(message, caption, buttons);
        }

        private void CloseHandler(FormClosingEventArgs e)
        {
            if(spreadsheet.Changed)
            {
                string message = "You didn't save! Would you like to continue closing this program?";
                string caption = "Warning:";
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                DialogResult result;

                result = MessageBox.Show(message, caption, buttons);

                if (result == DialogResult.No)
                {
                    e.Cancel = true;
                }
            }
        }
    }
}

