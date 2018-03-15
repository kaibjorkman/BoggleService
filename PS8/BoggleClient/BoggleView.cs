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
        public event Action CancelPressed;
        public event Action<string> PlayGame;
        public event Action CancelGameRequestPressed;
        public event Action<string> EnterButtonPressed;

        //Constructor
        public BoggleView()
        {
            InitializeComponent();
        }


        /// <summary>
        /// Is the user currently registered?
        /// </summary>
        public bool IsUserRegistered { get; set; }
        public bool IsUserInGame { get; set; }
        public bool IsUserPending { get; set; }

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
            //Registration Controls
            RegisterButton.Enabled = state && nameBox.Text.Length > 0 && !IsUserRegistered;
            RegisterButton.Visible = state && !IsUserRegistered;
            RegisterLabel.Visible = state && !IsUserRegistered;
            UsernameLabel.Visible = state;
            nameBox.Enabled = state && !IsUserRegistered;
            nameBox.Visible = state;

            //Cancel Controls
            cancelButton.Visible =  !state;
            cancelButton.Enabled = !state;

            //EnterGameControls
            PlayBoggleLabel.Visible = state && IsUserRegistered;
            TimeLimitLabel.Visible = state && IsUserRegistered;
            timeBox.Visible = state && IsUserRegistered;
            timeBox.Enabled = state && IsUserRegistered && !IsUserInGame;
            playButton.Visible = state && IsUserRegistered && !IsUserInGame;
            playButton.Enabled = state && timeBox.Text.Length > 0 && IsUserRegistered && !IsUserInGame;
            LeaveGameButton.Visible = state && IsUserRegistered && !IsUserInGame;
        }

        private void nameBox_TextChanged(object sender, EventArgs e)
        {
            EnableControls(true);
        }

        private void BoggleView_Load(object sender, EventArgs e)
        {
                                            
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            CancelPressed?.Invoke();
        }

        private void playButton_Click(object sender, EventArgs e)
        {
            PlayGame?.Invoke(timeBox.Text.Trim());
        }

        private void timeBox_TextChanged(object sender, EventArgs e)
        {
            EnableControls(true);
        }

        private void CancelGameRequest_Click(object sender, EventArgs e)
        {
            CancelGameRequestPressed?.Invoke();
        }

        public void startBoard(string board)
        {
            for(int i = 0; i < board.Length; i++)
            {
                if(i == 0)
                {
                    label1.Text = board[i].ToString();
                }
                else if(i == 1)
                {
                    label2.Text = board[i].ToString();
                }
                else if (i == 2)
                {
                    label3.Text = board[i].ToString();
                }
                else if (i == 3)
                {
                    label4.Text = board[i].ToString();
                }
                else if (i == 4)
                {
                    label5.Text = board[i].ToString();
                }
                else if (i == 5)
                {
                    label6.Text = board[i].ToString();
                }
                else if (i == 6)
                {
                    label7.Text = board[i].ToString();
                }
                else if (i == 7)
                {
                    label8.Text = board[i].ToString();
                }
                else if (i == 8)
                {
                    label9.Text = board[i].ToString();
                }
                else if (i == 9)
                {
                    label10.Text = board[i].ToString();
                }
                else if (i == 10)
                {
                    label11.Text = board[i].ToString();
                }
                else if (i == 11)
                {
                    label12.Text = board[i].ToString();
                }
                else if (i == 12)
                {
                    label13.Text = board[i].ToString();
                }
                else if (i == 13)
                {
                    label14.Text = board[i].ToString();
                }
                else if (i == 14)
                {
                    label15.Text = board[i].ToString();
                }
                else if (i == 15)
                {
                    label16.Text = board[i].ToString();
                }
            }
        }

        private void enterButton_Click(object sender, EventArgs e)
        {
            EnterButtonPressed?.Invoke(textBoxEnteredWords.Text.Trim());
        }

        public void newWord(string word)
        {

            currentList.BeginUpdate();

            currentList.Items.Add(word);

            currentList.EndUpdate();
        }

        public void updateData(string time, string scoreOne, string scoreTwo)
        {
            totalScorePlayer1.Text = scoreOne;
            totalScorePlayer2.Text = scoreTwo;
            timeLeft.Text = time;

        }
    }
}
