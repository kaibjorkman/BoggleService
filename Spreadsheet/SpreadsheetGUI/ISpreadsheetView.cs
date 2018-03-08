using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadsheetGUI
{
    interface ISpreadsheetView
    {
        event Action ArrowKeyLeft;

        void SetSelection();

        void GetSelection();
    }
}
