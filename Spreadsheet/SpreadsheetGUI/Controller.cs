using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SS;
using SSGui;

namespace SpreadsheetGUI
{
    class Controller
    {
        private SpreadsheetWindow window;
        private Spreadsheet spreadsheet;

        public Controller(SpreadsheetWindow window)
        {
            this.window = window;
            spreadsheet = new Spreadsheet();
            ///Arrow Keys
            window.ArrowKeyLeft += HandleArrowKeyLeft;
            window.ArrowKeyRight += HandleArrowKeyRight;
            window.ArrowKeyUp += HandleArrowKeyUp;
            window.ArrowKeyDown += HandleArrowKeyDown;
            window.CellContentsChanged += ContentsChanged;
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

        private void ContentsChanged(string contents)
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
            }
            catch (Exception)
            {
                window.Panel.SetValue(col, row, contents);
            }
        }

        private void WriteLine(string v)
        {
            throw new NotImplementedException();
        }
    }
    }

