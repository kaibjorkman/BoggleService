using SSGui;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpreadsheetGUI
{
    public interface ISpreadsheetView
    {
        //Key Evenets
        event Action ArrowKeyLeft;
        event Action ArrowKeyRight;
        event Action ArrowKeyUp;
        event Action ArrowKeyDown;
        event Action<string> EnterKeyPressed;
        event Action SelectionChanged;
        event Action<string> FileChosenEvent;
        event Action<string> Save;
        event Action Help;
        event Action<FormClosingEventArgs> CloseEvent;
        SpreadsheetPanel Panel { get; set; }
        string Contents { set; }

        void openNew(TextReader reader);
    }
}

