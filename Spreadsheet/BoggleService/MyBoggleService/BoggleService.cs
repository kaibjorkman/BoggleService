using Boggle;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using static Boggle.DataModels;
using static System.Net.HttpStatusCode;

namespace Boggle
{
    public class BoggleService
    {

        //new database
        private static string BoggleDB;

        static BoggleService()
        {
            string dbFolder = System.IO.Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            string connectionString = String.Format(@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = {0}\BoggleDB.mdf; Integrated Security = True", dbFolder);

            //BoggleDB = "C:\\Users\\Kai Bjorkman\\Source\\Repos\\Server\\Spreadsheet\\BoggleService\\MyBoggleService\\BoggleDB.mdf";
            BoggleDB = connectionString;
        }
        /**
        /// <summary>
        /// Returns a Stream version of index.html.
        /// </summary>
        /// <returns></returns>
        public Stream API()
        {
            SetStatus(OK);
            WebOperationContext.Current.OutgoingResponse.ContentType = "text/html";
            return File.OpenRead(AppDomain.CurrentDomain.BaseDirectory + "index.html");
        }
    **/
        /// <summary>
        /// This handles a register user request and checks if the username is null and returns
        /// a forbidden response if that is true and returns a userID is not.
        /// </summary>
        /// <param name="nickname"></param>
        /// <returns></returns>
        public UserResponse Register(Username name, out HttpStatusCode status)
        {
            //check validity 
            if (name.Nickname == null || name.Nickname.Trim().Length == 0 || name.Nickname.Trim().Length > 50)
            {
                status = HttpStatusCode.Forbidden;
                return null;
            }

            //put the user in the database
            using (SqlConnection conn = new SqlConnection(BoggleDB))
            {
                // Connections must be opened
                conn.Open();

                // Database commands should be executed within a transaction.  When commands 
                // are executed within a transaction, either all of the commands will succeed
                // or all will be canceled.  You don't have to worry about some of the commands
                // changing the DB and others failing.
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    // An SqlCommand executes a SQL statement on the database.  In this case it is an
                    // insert statement.  The first parameter is the statement, the second is the
                    // connection, and the third is the transaction.  
                    //
                    // Note that I use symbols like @UserID as placeholders for values that need to appear
                    // in the statement.  You will see below how the placeholders are replaced.  You may be
                    // tempted to simply paste the values into the string, but this is a BAD IDEA that violates
                    // a cardinal rule of DB Security 101.  By using the placeholder approach, you don't have
                    // to worry about escaping special characters and you don't have to worry about one form
                    // of the SQL injection attack.
                    using (SqlCommand command =
                        new SqlCommand("insert into Users (UserID, Nickname) values(@UserID, @Nickname)",
                                        conn,
                                        trans))
                    {
                        // We generate the userID to use.
                        string userID = Guid.NewGuid().ToString();

                        // This is where the placeholders are replaced.
                        command.Parameters.AddWithValue("@UserID", userID);
                        command.Parameters.AddWithValue("@Nickname", name.Nickname.Trim());


                        // This executes the command within the transaction over the connection.  The number of rows
                        // that were modified is returned.
                        if (command.ExecuteNonQuery() != 1)
                        {
                            throw new Exception("Query failed unexpectedly");
                        }
                        status = HttpStatusCode.Created;

                        // Immediately before each return that appears within the scope of a transaction, it is
                        // important to commit the transaction.  Otherwise, the transaction will be aborted and
                        // rolled back as soon as control leaves the scope of the transaction. 
                        trans.Commit();
                        UserResponse response = new UserResponse();
                        response.UserToken = userID;
                        return response;
                    }
                }


            }
        }

        public JoinGameResponse JoinGame(GameRequest request, out HttpStatusCode status)
        {

            string GameID = null;

            //ensure the userID is not null
            if (request.UserToken == null)
            {
                status = HttpStatusCode.Forbidden;
                return null;
            }

            if (request.TimeLimit < 5 || request.TimeLimit > 120)
            {
                status = HttpStatusCode.Forbidden;
                return null;
            }

            //check if the user is in the database
            using (SqlConnection conn = new SqlConnection(BoggleDB))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {

                    // Here, the SqlCommand is a select query.  We are interested in whether item.UserID exists in
                    // the Users table.
                    using (SqlCommand command1 = new SqlCommand("select UserID from Users where UserID = @UserID", conn, trans))
                    {
                        command1.Parameters.AddWithValue("@UserID", request.UserToken);

                        // This executes a query (i.e. a select statement).  The result is an
                        // SqlDataReader that you can use to iterate through the rows in the response.
                        using (SqlDataReader reader1 = command1.ExecuteReader())
                        {
                            // In this we don't actually need to read any data; we only need
                            // to know whether a row was returned.
                            if (!reader1.HasRows)
                            {
                                status = HttpStatusCode.Forbidden;
                                reader1.Close();
                                trans.Commit();
                                return null;
                            }
                            reader1.Close();
                        }
                    }
                    string pendingUser = null;
                    string pendingGameID = null;
                    int pendingTime = 0; ;
                    bool pending = false;
                    //check if there is a pending game
                    using (SqlCommand command = new SqlCommand("select GameID,Player1,TimeLimit from Games where Player2 is null", conn, trans))
                    {
                        using (SqlDataReader reader2 = command.ExecuteReader())
                        {
                            if (reader2.HasRows)
                            {
                                pending = true;
                                //Pending Game Avalible
                                while (reader2.Read())
                                {

                                    pendingUser = reader2["Player1"].ToString();
                                    pendingGameID = reader2["GameID"].ToString();
                                    pendingTime = (int)reader2["TimeLimit"];
                                    //check if the user is already in a pending game
                                    if (pendingUser == request.UserToken)
                                    {
                                        status = HttpStatusCode.Conflict;
                                        reader2.Close();
                                        trans.Commit();
                                        return null;
                                    }

                                }
                            }
                            reader2.Close();

                        }
                    }
                    if (pending)
                    {
                        using (SqlCommand pendingCommand = new SqlCommand("update Games set Player2 = @Player2, Board = @Board, TimeLimit = @TimeLimit, StartTime = @StartTime where GameID = @GameID", conn, trans))
                        {
                            pendingCommand.Parameters.AddWithValue("@Player2", request.UserToken);
                            BoggleBoard board = new BoggleBoard();
                            pendingCommand.Parameters.AddWithValue("@Board", board.ToString());
                            pendingCommand.Parameters.AddWithValue("@GameID", pendingGameID);

                            int oldTimeLimit = pendingTime;
                            int newTimeLimit = (oldTimeLimit + request.TimeLimit) / 2;
                            pendingCommand.Parameters.AddWithValue("@TimeLimit", newTimeLimit);
                            DateTime currentTime = DateTime.UtcNow;
                            pendingCommand.Parameters.AddWithValue("@StartTime", currentTime);

                            pendingCommand.ExecuteScalar();

                            JoinGameResponse response = new JoinGameResponse();
                            response.GameID = pendingGameID;



                            trans.Commit();
                            status = HttpStatusCode.Created;
                            return response;
                        }
                    }
                    else
                    {


                        //create new game
                        using (SqlCommand newCommand = new SqlCommand("insert into Games (Player1, TimeLimit) output inserted.GameID values (@Player1, @TimeLimit)", conn, trans))
                        {
                            newCommand.Parameters.AddWithValue("@Player1", request.UserToken);
                            newCommand.Parameters.AddWithValue("@TimeLimit", request.TimeLimit);



                            GameID = newCommand.ExecuteScalar().ToString();

                            JoinGameResponse response = new JoinGameResponse();
                            response.GameID = GameID;



                            trans.Commit();

                            status = HttpStatusCode.Accepted;
                            return response;
                        }


                    }
                }
            }



        }

        /// <summary>
        /// Demo.  You can delete this.
        /// </summary>
        public string WordAtIndex(int n, out HttpStatusCode status)
        {
            if (n < 0)
            {
                status = HttpStatusCode.Forbidden;
                return null;
            }

            string line;
            //changed path file
            using (StreamReader file = new System.IO.StreamReader("dictionary.txt"))
            {
                while ((line = file.ReadLine()) != null)
                {
                    if (n == 0) break;
                    n--;
                }
            }

            if (n == 0)
            {
                status = HttpStatusCode.OK;
                return line;
            }
            else
            {
                status = HttpStatusCode.Forbidden;
                return null;
            }
        }

        /// <summary>
        /// gets the user out of the queue
        /// </summary>
        /// <param name="userID"></param>
        public void CancelQueue(CancelGameRequest userID, out HttpStatusCode status)
        {


            //ensure the userID is not null
            if (userID.UserToken == null)
            {
                status = HttpStatusCode.Forbidden;
                return;
            }



            //check if the user is in the database
            using (SqlConnection conn = new SqlConnection(BoggleDB))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {

                    // Here, the SqlCommand is a select query.  We are interested in whether item.UserID exists in
                    // the Users table.
                    using (SqlCommand command1 = new SqlCommand("select UserID from Users where UserID = @UserID", conn, trans))
                    {
                        command1.Parameters.AddWithValue("@UserID", userID.UserToken);

                        // This executes a query (i.e. a select statement).  The result is an
                        // SqlDataReader that you can use to iterate through the rows in the response.
                        using (SqlDataReader reader1 = command1.ExecuteReader())
                        {
                            // In this we don't actually need to read any data; we only need
                            // to know whether a row was returned.
                            if (!reader1.HasRows)
                            {
                                status = HttpStatusCode.Forbidden;
                                reader1.Close();
                                trans.Commit();
                                return;
                            }
                            reader1.Close();
                        }
                    }

                    bool isPendingGame = false;
                    int pendingGameID = 0;
                    //check if there is a pending game
                    using (SqlCommand command = new SqlCommand("select GameID,Player1,TimeLimit from Games where Player2 is null", conn, trans))
                    {
                        using (SqlDataReader reader2 = command.ExecuteReader())
                        {
                            if (reader2.HasRows)
                            {
                                //Pending Game Avalible
                                while (reader2.Read())
                                {
                                    string pendingUser = reader2["Player1"].ToString();
                                    pendingGameID = (int)reader2["GameID"];

                                    //check if the user is already in a pending game
                                    if (pendingUser == userID.UserToken)
                                    {
                                        isPendingGame = true;
                                        break;
                                    }
                                }
                            }
                            reader2.Close();
                        }
                    }

                    //if the user is in a pending game, remove it
                    if (isPendingGame)
                    {
                        using (SqlCommand command = new SqlCommand("delete from Games where GameID = @GameID", conn, trans))
                        {
                            command.Parameters.AddWithValue("@GameID", pendingGameID);

                            if (command.ExecuteNonQuery() == 0)
                            {
                                status = HttpStatusCode.Forbidden;
                                trans.Commit();
                            }
                            else
                            {
                                status = HttpStatusCode.OK;
                                trans.Commit();
                            }
                        }
                    }
                    else
                    {
                        status = HttpStatusCode.Forbidden;
                        trans.Commit();
                    }
                }
            }
        }

        public PlayWordResponse PlayWord(WordAttempt attempt, string GameID, out HttpStatusCode status)
        {
            int GameID_int;
            try
            {
                GameID_int = Int32.Parse(GameID);
            }

            catch (Exception ex)
            {
                status = HttpStatusCode.Forbidden;
                return null;
            }


            //If Word is null or empty or longer than 30 characters when trimmed, responds with response code 403 (Forbidden).
            if (attempt.Word == null || attempt.Word.Length <= 0 || attempt.Word.Length > 30)
            {
                status = HttpStatusCode.Forbidden;
                return null;
            }

            //instane variables for saving the game info
            string player1 = null;
            string player2 = null;
            string board = null;
            int timeLimit = 0;
            DateTime startTime = new DateTime();
            int score = 0;
            int timeLeft = 0;
            bool isPlayer1 = false;
            bool isDuplicate = false;

            //Checks if user is in game
            using (SqlConnection conn = new SqlConnection(BoggleDB))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {

                    // Here, the SqlCommand is a select query.  We are interested in whether item.UserID exists in
                    // the Games.
                    using (SqlCommand command1 = new SqlCommand("select Player2, Player1, TimeLimit, StartTime, Board from Games where GameID = @GameID;", conn, trans))
                    {
                        command1.Parameters.AddWithValue("@GameID", GameID_int);


                        // This executes a query (i.e. a select statement).  The result is an
                        // SqlDataReader that you can use to iterate through the rows in the response.
                        using (SqlDataReader reader1 = command1.ExecuteReader())
                        {
                            if (reader1.HasRows)
                            {
                                while (reader1.Read())
                                {
                                    player1 = reader1["Player1"].ToString();
                                    player2 = reader1["Player2"].ToString();
                                    if (player2.Length == 0 || player2 == null)
                                    {
                                        //The player is in a pending game
                                        status = HttpStatusCode.Conflict;
                                        reader1.Close();
                                        trans.Commit();
                                        return null;
                                    }
                                    board = reader1["Board"].ToString();
                                    timeLimit = (int)reader1["TimeLimit"];
                                    startTime = (DateTime)reader1["StartTime"];
                                }

                                //check if the user is in the proper game
                                if (!(attempt.UserToken == player1 || attempt.UserToken == player2))
                                {
                                    status = HttpStatusCode.Forbidden;
                                    reader1.Close();
                                    trans.Commit();
                                    return null;
                                }

                                //see if the game is still active

                                DateTime currentTime = DateTime.UtcNow;
                                int timeElapsed = (int)(currentTime - startTime).TotalSeconds;
                                timeLeft = timeLimit - timeElapsed;
                            }
                            else
                            {
                                status = HttpStatusCode.Forbidden;
                                reader1.Close();
                                trans.Commit();
                                return null;
                            }
                            reader1.Close();
                        }
                    }

                    if (timeLeft <= 0)
                    {
                        status = HttpStatusCode.Conflict;
                        trans.Commit();
                        return null;
                    }
                    else
                    {
                        //checks which player played the word
                        if (player1 == attempt.UserToken)
                        {
                            isPlayer1 = true;

                            //is the word a duplicate word for player 1
                            using (SqlCommand command2 = new SqlCommand("select * from Words;", conn, trans))
                            {
                                using (SqlDataReader reader1 = command2.ExecuteReader())
                                {
                                    if (reader1.HasRows)
                                    {
                                        while (reader1.Read())
                                        {
                                            if (attempt.Word == reader1["Word"].ToString())
                                            {
                                                string user = (string)reader1["Player"];
                                                if (user == attempt.UserToken)
                                                {
                                                    isDuplicate = true;
                                                    score = 0;
                                                    break;
                                                }
                                            }
                                        }
                                    }

                                    reader1.Close();
                                }

                            }
                            if (!isDuplicate)
                            {
                                bool isActualWord = false;
                                //check if the word is in the dictionary
                                using (StreamReader words = new StreamReader("dictionary.txt"))
                                {
                                    string validWord;
                                    isActualWord = false;
                                    while ((validWord = words.ReadLine()) != null)
                                    {
                                        if (attempt.Word.ToUpper() == validWord)
                                        {
                                            isActualWord = true;
                                            break;
                                        }
                                    }
                                    words.Close();
                                }

                                //create boggle board  object to check words against
                                BoggleBoard boggleBoard = new BoggleBoard(board);
                                if (isActualWord == true)
                                {
                                    if (boggleBoard.CanBeFormed(attempt.Word))
                                    {
                                        //check the score of word
                                        if (attempt.Word.Trim().Length < 3)
                                        {
                                            score = 0;
                                        }
                                        else
                                        {
                                            switch (attempt.Word.Length)
                                            {
                                                case 3:
                                                    score = 1;
                                                    break;
                                                case 4:
                                                    score = 1;
                                                    break;
                                                case 5:
                                                    score = 2;
                                                    break;
                                                case 6:
                                                    score = 3;
                                                    break;
                                                case 7:
                                                    score = 5;
                                                    break;
                                                case 8:
                                                    score = 11;
                                                    break;
                                                default:
                                                    score = 11;
                                                    break;

                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (attempt.Word.Trim().Length < 3)
                                        {
                                            score = 0;
                                        }
                                        else
                                        {
                                            score = -1;
                                        }

                                    }
                                }

                                else
                                {
                                    if (attempt.Word.Trim().Length < 3)
                                    {
                                        score = 0;
                                    }
                                    else
                                    {
                                        score = -1;
                                    }
                                }
                            }




                        }
                        else
                        {
                            //PLayer2 played the word
                            isPlayer1 = false;

                            //is the word a duplicate word for player 2
                            using (SqlCommand command2 = new SqlCommand("select * from Words;", conn, trans))
                            {
                                using (SqlDataReader reader1 = command2.ExecuteReader())
                                {
                                    if (reader1.HasRows)
                                    {
                                        while (reader1.Read())
                                        {
                                            if (attempt.Word == reader1["Word"].ToString())
                                            {
                                                string user = (string)reader1["Player"];
                                                if (user == attempt.UserToken)
                                                {
                                                    isDuplicate = true;
                                                    score = 0;
                                                    break;
                                                }
                                            }
                                        }
                                    }

                                    reader1.Close();
                                }

                            }
                            if (!isDuplicate)
                            {
                                bool isActualWord = false;
                                //check if the word is in the dictionary
                                using (StreamReader words = new StreamReader("dictionary.txt"))
                                {
                                    string validWord;
                                    isActualWord = false;
                                    while ((validWord = words.ReadLine()) != null)
                                    {
                                        if (attempt.Word.ToUpper() == validWord)
                                        {
                                            isActualWord = true;
                                            break;
                                        }
                                    }
                                    words.Close();
                                }


                                BoggleBoard boggleBoard = new BoggleBoard(board);
                                if (isActualWord == true)
                                {
                                    if (boggleBoard.CanBeFormed(attempt.Word))
                                    {
                                        //check the score of word
                                        if (attempt.Word.Trim().Length < 3)
                                        {
                                            score = 0;
                                        }
                                        else
                                        {
                                            switch (attempt.Word.Length)
                                            {
                                                case 3:
                                                    score = 1;
                                                    break;
                                                case 4:
                                                    score = 1;
                                                    break;
                                                case 5:
                                                    score = 2;
                                                    break;
                                                case 6:
                                                    score = 3;
                                                    break;
                                                case 7:
                                                    score = 5;
                                                    break;
                                                case 8:
                                                    score = 11;
                                                    break;
                                                default:
                                                    score = 11;
                                                    break;

                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (attempt.Word.Trim().Length < 3)
                                        {
                                            score = 0;
                                        }
                                        else
                                        {
                                            score = -1;
                                        }
                                    }
                                }

                                else
                                {
                                    //subtract from score
                                    if (attempt.Word.Trim().Length < 3)
                                    {
                                        score = 0;
                                    }
                                    else
                                    {
                                        score = -1;
                                    }
                                }
                            }
                        }
                        //Update Words
                        using (SqlCommand command2 = new SqlCommand("insert into Words (Word, GameID, Player, Score) values(@Word, @GameID, @Player, @Score)", conn, trans))
                        {
                            command2.Parameters.AddWithValue("@Word", attempt.Word);
                            command2.Parameters.AddWithValue("@GameID", GameID_int);
                            command2.Parameters.AddWithValue("@Player", attempt.UserToken);
                            command2.Parameters.AddWithValue("@Score", score);

                            if (command2.ExecuteNonQuery() == 0)
                            {
                                status = HttpStatusCode.Forbidden;
                                trans.Commit();
                                return null;
                            }
                            else
                            {
                                status = HttpStatusCode.OK;
                                trans.Commit();
                                PlayWordResponse response = new PlayWordResponse();
                                response.Score = score;
                                return response;
                            }
                        }
                    }
                }
            }
        }

        public Game GetStatus(string GameID, string brief, out HttpStatusCode status)
        {
            int GameID_int;
            try
            {
                GameID_int = Int32.Parse(GameID);
            }

            catch (Exception ex)
            {
                status = HttpStatusCode.Forbidden;
                return null;
            }
            string player1 = null;
            string player2 = null;
            string board = null;
            int timeLimit = 0;
            DateTime startTime = new DateTime();
            int score = 0;
            int timeLeft = 0;
            //Checks if user is in game
            using (SqlConnection conn = new SqlConnection(BoggleDB))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {

                    // Here, the SqlCommand is a select query.  We are interested in whether item.UserID exists in
                    // the Games.
                    using (SqlCommand command1 = new SqlCommand("select Player2, Player1, TimeLimit, StartTime, Board from Games where GameID = @GameID", conn, trans))
                    {
                        command1.Parameters.AddWithValue("@GameID", GameID_int);


                        // This executes a query (i.e. a select statement).  The result is an
                        // SqlDataReader that you can use to iterate through the rows in the response.
                        using (SqlDataReader reader1 = command1.ExecuteReader())
                        {
                            if (reader1.HasRows)
                            {
                                while (reader1.Read())
                                {
                                    player1 = reader1["Player1"].ToString();
                                    player2 = reader1["Player2"].ToString();
                                    if (player2.Length == 0 || player2 == null)
                                    {
                                        //The player is in a pending game
                                        Game pendingGame = new Game();
                                        pendingGame.GameState = "pending";
                                        reader1.Close();
                                        trans.Commit();
                                        status = HttpStatusCode.OK;
                                        return pendingGame;
                                    }
                                    board = reader1["Board"].ToString();
                                    timeLimit = (int)reader1["TimeLimit"];
                                    startTime = (DateTime)reader1["StartTime"];
                                }


                                //calculate the time left in the game
                                DateTime currentTime = DateTime.UtcNow;
                                int timeElapsed = (int)(currentTime - startTime).TotalSeconds;
                                timeLeft = timeLimit - timeElapsed;
                            }
                            else
                            {
                                status = HttpStatusCode.Forbidden;
                                reader1.Close();
                                trans.Commit();
                                return null;
                            }
                            reader1.Close();
                        }
                    }

                    if (timeLeft > 0)
                    {
                        //active Game
                        if (brief == "yes")
                        {
                            //build brief active game object
                            Game gameActive = new Game();

                            gameActive.GameState = "active";
                            gameActive.TimeLeft = timeLeft;


                            Player1 playerOneActive = new Player1();
                            gameActive.Player1 = playerOneActive;
                            Player2 playerTwoActive = new Player2();
                            gameActive.Player2 = playerTwoActive;

                            int player1Score = 0;
                            int player2Score = 0;

                            using (SqlCommand command2 = new SqlCommand("select * from Words where GameID = @GameID", conn, trans))
                            {
                                command2.Parameters.AddWithValue("@GameID", GameID_int);

                                using (SqlDataReader reader1 = command2.ExecuteReader())
                                {
                                    if (reader1.HasRows)
                                    {
                                        //there are words to report
                                        while (reader1.Read())
                                        {
                                            string user = reader1["Player"].ToString();
                                            string word = reader1["Word"].ToString();
                                            int wordScore = (int)reader1["Score"];
                                            if (user == player1)
                                            {
                                                //player 1's word
                                                player1Score += wordScore;
                                            }
                                            else
                                            {
                                                //player 2's word
                                                player2Score += wordScore;
                                            }
                                        }
                                        reader1.Close();
                                    }

                                    else
                                    {
                                        //there are no words to report
                                        reader1.Close();

                                    }
                                }
                            }

                            //set the total Scores
                            playerOneActive.Score = player1Score;

                            playerTwoActive.Score = player2Score;



                            status = HttpStatusCode.OK;
                            trans.Commit();
                            return gameActive;



                        }

                        else
                        {
                            //build non brief active game object
                            //game is active
                            Game gameActive = new Game();

                            gameActive.GameState = "active";
                            gameActive.TimeLeft = timeLeft;
                            gameActive.TimeLimit = timeLimit;

                            gameActive.Board = board;

                            Player1 playerOneActivee = new Player1();
                            gameActive.Player1 = playerOneActivee;
                            Player2 playerTwoActive = new Player2();
                            gameActive.Player2 = playerTwoActive;

                            int player1Score = 0;
                            int player2Score = 0;

                            using (SqlCommand command2 = new SqlCommand("select * from Words where GameID = @GameID", conn, trans))
                            {
                                command2.Parameters.AddWithValue("@GameID", GameID_int);

                                using (SqlDataReader reader1 = command2.ExecuteReader())
                                {
                                    if (reader1.HasRows)
                                    {
                                        //there are words to report
                                        while (reader1.Read())
                                        {
                                            string user = reader1["Player"].ToString();
                                            string word = reader1["Word"].ToString();
                                            int wordScore = (int)reader1["Score"];
                                            if (user == player1)
                                            {
                                                //player 1's word

                                                player1Score += wordScore;
                                            }
                                            else
                                            {
                                                //player 2's word

                                                player2Score += wordScore;
                                            }
                                        }
                                        reader1.Close();
                                    }

                                    else
                                    {
                                        //there are no words to report
                                        reader1.Close();

                                    }
                                }
                            }

                            //set the total Scores
                            playerOneActivee.Score = player1Score;

                            playerTwoActive.Score = player2Score;

                            using (SqlCommand command2 = new SqlCommand("select Nickname from Users where UserID = @UserID", conn, trans))
                            {
                                command2.Parameters.AddWithValue("@UserID", player1);

                                playerOneActivee.Nickname = command2.ExecuteScalar().ToString();


                            }
                            using (SqlCommand command2 = new SqlCommand("select Nickname from Users where UserID = @UserID", conn, trans))
                            {
                                command2.Parameters.AddWithValue("@UserID", player2);

                                playerTwoActive.Nickname = command2.ExecuteScalar().ToString();


                            }

                            status = HttpStatusCode.OK;
                            trans.Commit();
                            return gameActive;
                        }

                    }

                    else
                    {
                        //completed game
                        if (brief == "yes")
                        {
                            //build brief completed game object
                            //game is complete
                            Game gameComplete = new Game();

                            gameComplete.GameState = "completed";
                            gameComplete.TimeLeft = 0;


                            Player1 playerOneComplete = new Player1();
                            gameComplete.Player1 = playerOneComplete;
                            Player2 playerTwoComplete = new Player2();
                            gameComplete.Player2 = playerTwoComplete;

                            int player1Score = 0;
                            int player2Score = 0;

                            using (SqlCommand command2 = new SqlCommand("select * from Words where GameID = @GameID", conn, trans))
                            {
                                command2.Parameters.AddWithValue("@GameID", GameID_int);

                                using (SqlDataReader reader1 = command2.ExecuteReader())
                                {
                                    if (reader1.HasRows)
                                    {
                                        //there are words to report
                                        while (reader1.Read())
                                        {
                                            string user = reader1["Player"].ToString();
                                            string word = reader1["Word"].ToString();
                                            int wordScore = (int)reader1["Score"];
                                            if (user == player1)
                                            {
                                                //player 1's word
                                                player1Score += wordScore;
                                            }
                                            else
                                            {
                                                //player 2's word
                                                player2Score += wordScore;
                                            }
                                        }
                                        reader1.Close();
                                    }

                                    else
                                    {
                                        //there are no words to report
                                        reader1.Close();

                                    }
                                }
                            }

                            //set the total Scores
                            playerOneComplete.Score = player1Score;

                            playerTwoComplete.Score = player2Score;



                            status = HttpStatusCode.OK;
                            trans.Commit();
                            return gameComplete;
                        }

                        else
                        {
                            //build non brief completed game object
                            //game is complete
                            Game gameComplete = new Game();

                            gameComplete.GameState = "completed";
                            gameComplete.TimeLeft = 0;
                            gameComplete.TimeLimit = timeLimit;

                            gameComplete.Board = board;

                            Player1 playerOneComplete = new Player1();
                            gameComplete.Player1 = playerOneComplete;
                            Player2 playerTwoComplete = new Player2();
                            gameComplete.Player2 = playerTwoComplete;

                            int player1Score = 0;
                            int player2Score = 0;
                            List<WordPlayed> WordsPlayed = new List<WordPlayed>();
                            playerOneComplete.WordsPlayed = WordsPlayed;
                            List<WordPlayed> WordsPlayed2 = new List<WordPlayed>();
                            playerTwoComplete.WordsPlayed = WordsPlayed2;
                            using (SqlCommand command2 = new SqlCommand("select * from Words where GameID = @GameID", conn, trans))
                            {
                                command2.Parameters.AddWithValue("@GameID", GameID_int);

                                using (SqlDataReader reader1 = command2.ExecuteReader())
                                {
                                    if (reader1.HasRows)
                                    {
                                        //there are words to report
                                        while (reader1.Read())
                                        {
                                            string user = reader1["Player"].ToString();
                                            string word = reader1["Word"].ToString();
                                            int wordScore = (int)reader1["Score"];
                                            if (user == player1)
                                            {
                                                //player 1's word
                                                WordPlayed wordPlayed1 = new WordPlayed();
                                                wordPlayed1.Word = word;
                                                wordPlayed1.Score = wordScore;
                                                WordsPlayed.Add(wordPlayed1);
                                                player1Score += wordScore;
                                            }
                                            else
                                            {
                                                //player 2's word
                                                WordPlayed wordPlayed2 = new WordPlayed();
                                                wordPlayed2.Word = word;
                                                wordPlayed2.Score = wordScore;
                                                WordsPlayed2.Add(wordPlayed2);
                                                player2Score += wordScore;
                                            }
                                        }
                                        reader1.Close();
                                    }

                                    else
                                    {
                                        //there are no words to report
                                        reader1.Close();

                                    }
                                }
                            }

                            //set the total Scores
                            playerOneComplete.Score = player1Score;

                            playerTwoComplete.Score = player2Score;

                            using (SqlCommand command2 = new SqlCommand("select Nickname from Users where UserID = @UserID", conn, trans))
                            {
                                command2.Parameters.AddWithValue("@UserID", player1);

                                playerOneComplete.Nickname = command2.ExecuteScalar().ToString();


                            }
                            using (SqlCommand command2 = new SqlCommand("select Nickname from Users where UserID = @UserID", conn, trans))
                            {
                                command2.Parameters.AddWithValue("@UserID", player2);

                                playerTwoComplete.Nickname = command2.ExecuteScalar().ToString();


                            }

                            status = HttpStatusCode.OK;
                            trans.Commit();
                            return gameComplete;
                        }
                    }


                }
            }
        }
    }
}