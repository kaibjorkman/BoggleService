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
        public event Action<string, string> RegisterUser;
        public event Action CancelPressed;
        public event Action<string> PlayGame;
        public event Action CancelGameRequestPressed;
        public event Action<string> EnterButtonPressed;
        public event Action LeavePressed;

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
            RegisterUser?.Invoke(nameBox.Text.Trim(), domainBox.Text.Trim());
        }

        /// <summary>
        /// sets the usability and visibility of the controls
        /// </summary>
        /// <param name="state"></param>
        public void EnableControls(bool state)
        {
            //Registration Controls
            RegisterButton.Enabled = state && nameBox.Text.Length > 0 && domainBox.Text.Length > 0 && !IsUserRegistered;
            RegisterButton.Visible = state && !IsUserRegistered;
            RegisterLabel.Visible = state && !IsUserRegistered;
            UsernameLabel.Visible = state;
            nameBox.Enabled = state && !IsUserRegistered;
            nameBox.Visible = state;

            //Cancel Controls
            cancelButton.Visible =  !state && !IsUserRegistered;
            cancelButton.Enabled = !state && !IsUserRegistered;

            //EnterGameControls
            PlayBoggleLabel.Visible = state && IsUserRegistered;
            TimeLimitLabel.Visible = state && IsUserRegistered;
            timeBox.Visible = state && IsUserRegistered;
            timeBox.Enabled = state && IsUserRegistered && !IsUserInGame;
            playButton.Visible = state && IsUserRegistered && !IsUserInGame;
            playButton.Enabled = state && timeBox.Text.Length > 0 && IsUserRegistered && !IsUserInGame;
            LeaveGameButton.Visible = !state && IsUserRegistered && IsUserPending;
            leaveButton.Enabled = state && IsUserRegistered;
        }

        /// <summary>
        /// when text is typed in name box fire enable controlls
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void nameBox_TextChanged(object sender, EventArgs e)
        {
            EnableControls(true);
        }

        private void BoggleView_Load(object sender, EventArgs e)
        {
                                            
        }

        /// <summary>
        /// when cancel button clicked fire cancel pressed event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cancelButton_Click(object sender, EventArgs e)
        {
            CancelPressed?.Invoke();
        }

        /// <summary>
        /// when play button is clicked fire play game event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void playButton_Click(object sender, EventArgs e)
        {
            PlayGame?.Invoke(timeBox.Text.Trim());
        }

        /// <summary>
        /// when the text is changed for the time box call the 
        /// enable controls method
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timeBox_TextChanged(object sender, EventArgs e)
        {
            EnableControls(true);
        }

        /// <summary>
        /// when the cancel game button is pressed fire a cancel game request event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelGameRequest_Click(object sender, EventArgs e)
        {
            CancelGameRequestPressed?.Invoke();
        }

        /// <summary>
        /// fills up the boggle board with the letters we get from the server
        /// </summary>
        /// <param name="board"></param>
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

        /// <summary>
        /// when the enter button is pressed fire the enter button pressed event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void enterButton_Click(object sender, EventArgs e)
        {
            EnterButtonPressed?.Invoke(textBoxEnteredWords.Text.Trim());
        }

        /// <summary>
        /// updates the list of words that the user has entered during a game
        /// </summary>
        /// <param name="word"></param>
        public void newWord(string word)
        {

            currentList.BeginUpdate();

            currentList.Items.Add(word);

            currentList.EndUpdate();
        }

        /// <summary>
        /// updates the scores for player one and two throughout the game
        /// </summary>
        /// <param name="time"></param>
        /// <param name="scoreOne"></param>
        /// <param name="scoreTwo"></param>
        public void updateData(string time, string scoreOne, string scoreTwo)
        {
            totalScorePlayer1.Text = scoreOne;
            totalScorePlayer2.Text = scoreTwo;
            timeLeft.Text = time;

        }

        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {

        }

        /// <summary>
        /// fills up the final words for player one
        /// </summary>
        /// <param name="word"></param>
        public void updateEndOne(string word)
        {
            finalWordsPlayer1.BeginUpdate();

            finalWordsPlayer1.Items.Add(word);

            finalWordsPlayer1.EndUpdate();
        }

        /// <summary>
        /// fills up the final words for player 2
        /// </summary>
        /// <param name="word"></param>
        public void updateEndTwo(string word)
        {
            finalWordsPlayer2.BeginUpdate();

            finalWordsPlayer2.Items.Add(word);

            finalWordsPlayer2.EndUpdate();
        }

        /// <summary>
        /// resets the view back to its default state
        /// </summary>
        public void Reset()
        {
            //update times and scores
            totalScorePlayer1.Text = "0";
            totalScorePlayer2.Text = "0";
            timeLeft.Text = "00";

            //Reset board
            label1.Text = "Boggle";
            label2.Text = "Boggle";
            label3.Text = "Boggle";
            label4.Text = "Boggle";
            label5.Text = "Boggle";
            label6.Text = "Boggle";
            label7.Text = "Boggle";
            label8.Text = "Boggle";
            label9.Text = "Boggle";
            label10.Text = "Boggle";
            label11.Text = "Boggle";
            label12.Text = "Boggle";
            label13.Text = "Boggle";
            label14.Text = "Boggle";
            label15.Text = "Boggle";
            label16.Text = "Boggle";

            currentList.Items.Clear();
            finalWordsPlayer1.Items.Clear();
            finalWordsPlayer2.Items.Clear();

            
        }

        /// <summary>
        /// when the leave button is pressed fire the leave pressed event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void leaveButton_Click(object sender, EventArgs e)
        {
            LeavePressed?.Invoke();
        }

        /// <summary>
        /// when the help button is pressed open the help window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void helpButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Welcome to Boggle! Enter a domain and username to register, " +
                "then enter your desired game time anywhere from 5 to 120 seconds. " +
                "You will be put into a queue until you find a game, once a game is found " +
                "the board will automatically update and you can start finding words! " +
                "You can click the leave button at any time to exit the game.");
        }
    }
}
