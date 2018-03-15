using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SSGui;
using SpreadsheetGUI;

namespace TestController
{
    class SpreadsheetViewStub : ISpreadsheetView
    {
        public SpreadsheetPanel Panel
        {
            get;
            set;

        }
        public string Contents
        {
            get;
            set;
        }

        public bool calledOpenNew
        {
            get; private set;
        }

        public void FireArrowLeft()
        {
            if(ArrowKeyLeft != null)
            {
                ArrowKeyLeft();
            }
        }

        public void FireArrowRight()
        {
            if (ArrowKeyRight != null)
            {
                ArrowKeyRight();
            }
        }
        public void FireArrowUp()
        {
            if (ArrowKeyUp != null)
            {
                ArrowKeyUp();
            }
        }

        public void FireArrowDown()
        {
            if (ArrowKeyDown != null)
            {
                ArrowKeyDown();
            }
        }

        public void FireEnterKeyPressed(string s)
        {
            if (EnterKeyPressed != null)
            {
                EnterKeyPressed(s);
            }
        }

        public void FireFileChosenEvent(string s)
        {
            if (FileChosenEvent != null)
            {
                FileChosenEvent(s);
            }
        }
        public void FireSave(string s)
        {
            if (Save != null)
            {
                Save(s);
            }
        }
        public void FireHelp()
        {
            if (Help != null)
            {
                Help();
            }
        }
        public void FireCloseEvent(FormClosingEventArgs f)
        {
            if (CloseEvent != null)
            {
                CloseEvent(f);
            }
        }


        public event Action ArrowKeyLeft;
        public event Action ArrowKeyRight;
        public event Action ArrowKeyUp;
        public event Action ArrowKeyDown;
        public event Action<string> EnterKeyPressed;
        public event Action<string> FileChosenEvent;
        public event Action<string> Save;
        public event Action Help;
        public event Action<FormClosingEventArgs> CloseEvent;

        public void openNew(TextReader reader)
        {
            calledOpenNew = true;
        }


    }
}
