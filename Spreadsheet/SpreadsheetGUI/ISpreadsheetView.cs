using SSGui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadsheetGUI
{
    interface ISpreadsheetView
    {
        //Key Evenets
        event Action ArrowKeyLeft;
        event Action ArrowKeyRight;
        event Action ArrowKeyUp;
        event Action ArrowKeyDown;
        event Action<string> CellContentsChanged;
        SpreadsheetPanel Panel { get; set; }

       

    }
}

