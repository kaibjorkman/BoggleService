using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BoggleClient
{
    public partial class BoggleView : Form, IBoggleView
    {
        //Events
        public event Action<string> RegisterUser;

        //Constructor
        public BoggleView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Is the user currently registered?
        /// </summary>
        public bool IsUserRegistered { get; set; }

        /// <summary>
        /// if this button is pressed register the user
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RegisterButton_Click(object sender, EventArgs e)
        {
            RegisterUser?.Invoke(nameBox.Text.Trim());
        }

        public void EnableControls(bool state)
        {
            RegisterButton.Enabled = state && nameBox.Text.Length > 0;

            cancelButton.Enabled = !state;
        }

        private void nameBox_TextChanged(object sender, EventArgs e)
        {
            EnableControls(true);
        }
    }
}
