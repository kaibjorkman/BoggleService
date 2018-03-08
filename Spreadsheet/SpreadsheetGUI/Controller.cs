using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadsheetGUI
{
    class Controller
    {
        private SpreadsheetWindow window;

        public Controller(SpreadsheetWindow window)
        {
            this.window = window;
            window.ArrowKeyLeft += HandleArrowKeyLeft;
        }


        private void HandleArrowKeyLeft()
        {
            window.GetSelection(out int col, out int row);

            window.SetSelection();
        }
    }
}
