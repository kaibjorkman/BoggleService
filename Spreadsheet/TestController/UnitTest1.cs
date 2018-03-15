using System;
using System.IO;
using System.Windows.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpreadsheetGUI;

namespace TestController
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            SpreadsheetViewStub stub = new SpreadsheetViewStub();
            
            Controller controller = new Controller(stub);
          

            string s = "A1";
            FormClosingEventArgs f = null;

            stub.FireArrowDown();
            stub.FireArrowLeft();
            stub.FireArrowRight();
            stub.FireArrowUp();
            stub.FireCloseEvent(f);
            stub.FireEnterKeyPressed(s);
            stub.FireFileChosenEvent(s);
            stub.FireHelp();
            stub.FireSave(s);
           

            
        }
    }
}
