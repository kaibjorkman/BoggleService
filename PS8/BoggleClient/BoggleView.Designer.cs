namespace BoggleClient
{
    partial class BoggleView
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BoggleView));
            this.nameBox = new System.Windows.Forms.TextBox();
            this.RegisterLabel = new System.Windows.Forms.Label();
            this.UsernameLabel = new System.Windows.Forms.Label();
            this.RegisterButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.PlayBoggleLabel = new System.Windows.Forms.Label();
            this.TimeLimitLabel = new System.Windows.Forms.Label();
            this.playButton = new System.Windows.Forms.Button();
            this.timeBox = new System.Windows.Forms.TextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.LeaveGameButton = new System.Windows.Forms.Button();
            this.label19 = new System.Windows.Forms.Label();
            this.currentWords = new System.Windows.Forms.Label();
            this.player2 = new System.Windows.Forms.Label();
            this.currentList = new System.Windows.Forms.ListBox();
            this.finalWordsPlayer2 = new System.Windows.Forms.ListBox();
            this.currentScore = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.totalScorePlayer1 = new System.Windows.Forms.Label();
            this.totalScorePlayer2 = new System.Windows.Forms.Label();
            this.finalWordsPlayer1 = new System.Windows.Forms.ListBox();
            this.newWordLabel = new System.Windows.Forms.Label();
            this.textBoxEnteredWords = new System.Windows.Forms.TextBox();
            this.enterButton = new System.Windows.Forms.Button();
            this.timeLabel = new System.Windows.Forms.Label();
            this.timeLeft = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // nameBox
            // 
            this.nameBox.Location = new System.Drawing.Point(396, 94);
            this.nameBox.Margin = new System.Windows.Forms.Padding(4);
            this.nameBox.Name = "nameBox";
            this.nameBox.Size = new System.Drawing.Size(132, 31);
            this.nameBox.TabIndex = 0;
            this.nameBox.TextChanged += new System.EventHandler(this.nameBox_TextChanged);
            // 
            // RegisterLabel
            // 
            this.RegisterLabel.AutoSize = true;
            this.RegisterLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RegisterLabel.Location = new System.Drawing.Point(316, 31);
            this.RegisterLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.RegisterLabel.Name = "RegisterLabel";
            this.RegisterLabel.Size = new System.Drawing.Size(293, 37);
            this.RegisterLabel.TabIndex = 1;
            this.RegisterLabel.Text = "Register As a User:";
            // 
            // UsernameLabel
            // 
            this.UsernameLabel.AutoSize = true;
            this.UsernameLabel.Location = new System.Drawing.Point(261, 98);
            this.UsernameLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.UsernameLabel.Name = "UsernameLabel";
            this.UsernameLabel.Size = new System.Drawing.Size(116, 25);
            this.UsernameLabel.TabIndex = 2;
            this.UsernameLabel.Text = "Username:";
            // 
            // RegisterButton
            // 
            this.RegisterButton.Location = new System.Drawing.Point(556, 88);
            this.RegisterButton.Margin = new System.Windows.Forms.Padding(4);
            this.RegisterButton.Name = "RegisterButton";
            this.RegisterButton.Size = new System.Drawing.Size(135, 46);
            this.RegisterButton.TabIndex = 3;
            this.RegisterButton.Text = "Register";
            this.RegisterButton.UseVisualStyleBackColor = true;
            this.RegisterButton.Click += new System.EventHandler(this.RegisterButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(699, 88);
            this.cancelButton.Margin = new System.Windows.Forms.Padding(4);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(148, 46);
            this.cancelButton.TabIndex = 4;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // PlayBoggleLabel
            // 
            this.PlayBoggleLabel.AutoSize = true;
            this.PlayBoggleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PlayBoggleLabel.Location = new System.Drawing.Point(362, 171);
            this.PlayBoggleLabel.Name = "PlayBoggleLabel";
            this.PlayBoggleLabel.Size = new System.Drawing.Size(195, 37);
            this.PlayBoggleLabel.TabIndex = 6;
            this.PlayBoggleLabel.Text = "Play Boggle:";
            // 
            // TimeLimitLabel
            // 
            this.TimeLimitLabel.AutoSize = true;
            this.TimeLimitLabel.Location = new System.Drawing.Point(198, 245);
            this.TimeLimitLabel.Name = "TimeLimitLabel";
            this.TimeLimitLabel.Size = new System.Drawing.Size(179, 25);
            this.TimeLimitLabel.TabIndex = 7;
            this.TimeLimitLabel.Text = "Game Time Limit:";
            // 
            // playButton
            // 
            this.playButton.Location = new System.Drawing.Point(556, 234);
            this.playButton.Name = "playButton";
            this.playButton.Size = new System.Drawing.Size(168, 46);
            this.playButton.TabIndex = 8;
            this.playButton.Text = "Join Game";
            this.playButton.UseVisualStyleBackColor = true;
            this.playButton.Click += new System.EventHandler(this.playButton_Click);
            // 
            // timeBox
            // 
            this.timeBox.Location = new System.Drawing.Point(396, 245);
            this.timeBox.Name = "timeBox";
            this.timeBox.Size = new System.Drawing.Size(100, 31);
            this.timeBox.TabIndex = 9;
            this.timeBox.TextChanged += new System.EventHandler(this.timeBox_TextChanged);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(30, 306);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(832, 809);
            this.pictureBox1.TabIndex = 10;
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(61, 390);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(135, 42);
            this.label1.TabIndex = 11;
            this.label1.Text = "Boggle";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(259, 390);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(135, 42);
            this.label2.TabIndex = 12;
            this.label2.Text = "Boggle";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(448, 390);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(135, 42);
            this.label3.TabIndex = 13;
            this.label3.Text = "Boggle";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(658, 390);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(135, 42);
            this.label4.TabIndex = 14;
            this.label4.Text = "Boggle";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(61, 589);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(135, 42);
            this.label5.TabIndex = 15;
            this.label5.Text = "Boggle";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(259, 589);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(135, 42);
            this.label6.TabIndex = 16;
            this.label6.Text = "Boggle";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(448, 589);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(135, 42);
            this.label7.TabIndex = 17;
            this.label7.Text = "Boggle";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(646, 589);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(135, 42);
            this.label8.TabIndex = 18;
            this.label8.Text = "Boggle";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(61, 807);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(135, 42);
            this.label9.TabIndex = 19;
            this.label9.Text = "Boggle";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(256, 807);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(135, 42);
            this.label10.TabIndex = 20;
            this.label10.Text = "Boggle";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(448, 807);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(135, 42);
            this.label11.TabIndex = 21;
            this.label11.Text = "Boggle";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(646, 807);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(135, 42);
            this.label12.TabIndex = 22;
            this.label12.Text = "Boggle";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(49, 980);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(135, 42);
            this.label13.TabIndex = 23;
            this.label13.Text = "Boggle";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(247, 980);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(135, 42);
            this.label14.TabIndex = 24;
            this.label14.Text = "Boggle";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.Location = new System.Drawing.Point(448, 980);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(135, 42);
            this.label15.TabIndex = 25;
            this.label15.Text = "Boggle";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.Location = new System.Drawing.Point(646, 980);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(135, 42);
            this.label16.TabIndex = 26;
            this.label16.Text = "Boggle";
            // 
            // LeaveGameButton
            // 
            this.LeaveGameButton.Location = new System.Drawing.Point(750, 237);
            this.LeaveGameButton.Name = "LeaveGameButton";
            this.LeaveGameButton.Size = new System.Drawing.Size(183, 46);
            this.LeaveGameButton.TabIndex = 27;
            this.LeaveGameButton.Text = "Exit Queue";
            this.LeaveGameButton.UseVisualStyleBackColor = true;
            this.LeaveGameButton.Click += new System.EventHandler(this.CancelGameRequest_Click);
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(374, 1196);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(138, 25);
            this.label19.TabIndex = 30;
            this.label19.Text = "Final Scores:";
            // 
            // currentWords
            // 
            this.currentWords.AutoSize = true;
            this.currentWords.Location = new System.Drawing.Point(897, 405);
            this.currentWords.Name = "currentWords";
            this.currentWords.Size = new System.Drawing.Size(74, 25);
            this.currentWords.TabIndex = 31;
            this.currentWords.Text = "Words";
            // 
            // player2
            // 
            this.player2.AutoSize = true;
            this.player2.Location = new System.Drawing.Point(474, 1243);
            this.player2.Name = "player2";
            this.player2.Size = new System.Drawing.Size(97, 25);
            this.player2.TabIndex = 32;
            this.player2.Text = "Player 2:";
            // 
            // currentList
            // 
            this.currentList.FormattingEnabled = true;
            this.currentList.ItemHeight = 25;
            this.currentList.Location = new System.Drawing.Point(902, 444);
            this.currentList.Name = "currentList";
            this.currentList.Size = new System.Drawing.Size(90, 329);
            this.currentList.TabIndex = 33;
            // 
            // finalWordsPlayer2
            // 
            this.finalWordsPlayer2.FormattingEnabled = true;
            this.finalWordsPlayer2.ItemHeight = 25;
            this.finalWordsPlayer2.Location = new System.Drawing.Point(467, 1283);
            this.finalWordsPlayer2.Name = "finalWordsPlayer2";
            this.finalWordsPlayer2.Size = new System.Drawing.Size(142, 354);
            this.finalWordsPlayer2.TabIndex = 34;
            // 
            // currentScore
            // 
            this.currentScore.AutoSize = true;
            this.currentScore.Location = new System.Drawing.Point(890, 776);
            this.currentScore.Name = "currentScore";
            this.currentScore.Size = new System.Drawing.Size(138, 25);
            this.currentScore.TabIndex = 35;
            this.currentScore.Text = "current score";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(261, 1243);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(97, 25);
            this.label21.TabIndex = 36;
            this.label21.Text = "Player 1:";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(103, 1243);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(128, 25);
            this.label22.TabIndex = 37;
            this.label22.Text = "Total Score:";
            // 
            // totalScorePlayer1
            // 
            this.totalScorePlayer1.AutoSize = true;
            this.totalScorePlayer1.Location = new System.Drawing.Point(374, 1243);
            this.totalScorePlayer1.Name = "totalScorePlayer1";
            this.totalScorePlayer1.Size = new System.Drawing.Size(24, 25);
            this.totalScorePlayer1.TabIndex = 38;
            this.totalScorePlayer1.Text = "0";
            // 
            // totalScorePlayer2
            // 
            this.totalScorePlayer2.AutoSize = true;
            this.totalScorePlayer2.Location = new System.Drawing.Point(585, 1243);
            this.totalScorePlayer2.Name = "totalScorePlayer2";
            this.totalScorePlayer2.Size = new System.Drawing.Size(24, 25);
            this.totalScorePlayer2.TabIndex = 39;
            this.totalScorePlayer2.Text = "0";
            // 
            // finalWordsPlayer1
            // 
            this.finalWordsPlayer1.FormattingEnabled = true;
            this.finalWordsPlayer1.ItemHeight = 25;
            this.finalWordsPlayer1.Location = new System.Drawing.Point(300, 1283);
            this.finalWordsPlayer1.Name = "finalWordsPlayer1";
            this.finalWordsPlayer1.Size = new System.Drawing.Size(136, 354);
            this.finalWordsPlayer1.TabIndex = 40;
            // 
            // newWordLabel
            // 
            this.newWordLabel.AutoSize = true;
            this.newWordLabel.Location = new System.Drawing.Point(219, 1122);
            this.newWordLabel.Name = "newWordLabel";
            this.newWordLabel.Size = new System.Drawing.Size(117, 25);
            this.newWordLabel.TabIndex = 41;
            this.newWordLabel.Text = "New Word:";
            // 
            // textBoxEnteredWords
            // 
            this.textBoxEnteredWords.Location = new System.Drawing.Point(342, 1122);
            this.textBoxEnteredWords.Name = "textBoxEnteredWords";
            this.textBoxEnteredWords.Size = new System.Drawing.Size(100, 31);
            this.textBoxEnteredWords.TabIndex = 42;
            // 
            // enterButton
            // 
            this.enterButton.Location = new System.Drawing.Point(453, 1122);
            this.enterButton.Name = "enterButton";
            this.enterButton.Size = new System.Drawing.Size(181, 49);
            this.enterButton.TabIndex = 43;
            this.enterButton.Text = "Enter";
            this.enterButton.UseVisualStyleBackColor = true;
            this.enterButton.Click += new System.EventHandler(this.enterButton_Click);
            // 
            // timeLabel
            // 
            this.timeLabel.AutoSize = true;
            this.timeLabel.Location = new System.Drawing.Point(711, 1134);
            this.timeLabel.Name = "timeLabel";
            this.timeLabel.Size = new System.Drawing.Size(107, 25);
            this.timeLabel.TabIndex = 44;
            this.timeLabel.Text = "Time Left:";
            // 
            // timeLeft
            // 
            this.timeLeft.AutoSize = true;
            this.timeLeft.Location = new System.Drawing.Point(875, 1134);
            this.timeLeft.Name = "timeLeft";
            this.timeLeft.Size = new System.Drawing.Size(36, 25);
            this.timeLeft.TabIndex = 45;
            this.timeLeft.Text = "00";
            // 
            // BoggleView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1090, 1713);
            this.Controls.Add(this.timeLeft);
            this.Controls.Add(this.timeLabel);
            this.Controls.Add(this.enterButton);
            this.Controls.Add(this.textBoxEnteredWords);
            this.Controls.Add(this.newWordLabel);
            this.Controls.Add(this.finalWordsPlayer1);
            this.Controls.Add(this.totalScorePlayer2);
            this.Controls.Add(this.totalScorePlayer1);
            this.Controls.Add(this.label22);
            this.Controls.Add(this.label21);
            this.Controls.Add(this.currentScore);
            this.Controls.Add(this.finalWordsPlayer2);
            this.Controls.Add(this.currentList);
            this.Controls.Add(this.player2);
            this.Controls.Add(this.currentWords);
            this.Controls.Add(this.label19);
            this.Controls.Add(this.LeaveGameButton);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.timeBox);
            this.Controls.Add(this.playButton);
            this.Controls.Add(this.TimeLimitLabel);
            this.Controls.Add(this.PlayBoggleLabel);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.RegisterButton);
            this.Controls.Add(this.UsernameLabel);
            this.Controls.Add(this.RegisterLabel);
            this.Controls.Add(this.nameBox);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "BoggleView";
            this.Text = "Boggle";
            this.Load += new System.EventHandler(this.BoggleView_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox nameBox;
        private System.Windows.Forms.Label RegisterLabel;
        private System.Windows.Forms.Label UsernameLabel;
        private System.Windows.Forms.Button RegisterButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Label PlayBoggleLabel;
        private System.Windows.Forms.Label TimeLimitLabel;
        private System.Windows.Forms.Button playButton;
        private System.Windows.Forms.TextBox timeBox;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Button LeaveGameButton;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label currentWords;
        private System.Windows.Forms.Label player2;
        private System.Windows.Forms.ListBox currentList;
        private System.Windows.Forms.ListBox finalWordsPlayer2;
        private System.Windows.Forms.Label currentScore;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label totalScorePlayer1;
        private System.Windows.Forms.ListBox finalWordsPlayer1;
        private System.Windows.Forms.Label totalScorePlayer2;
        private System.Windows.Forms.Label newWordLabel;
        private System.Windows.Forms.TextBox textBoxEnteredWords;
        private System.Windows.Forms.Button enterButton;
        private System.Windows.Forms.Label timeLabel;
        private System.Windows.Forms.Label timeLeft;
    }
}

