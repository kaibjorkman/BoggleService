using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.ServiceModel.Web;
using static Boggle.DataModels;
using static System.Net.HttpStatusCode;

namespace Boggle
{
    public class BoggleService : IBoggleService
    {

        //Poor Man's database
        private static readonly object sync = new object();
        private readonly static Dictionary<String, Username> users = new Dictionary<String, Username>();
        private readonly static Dictionary<int, Game> games = new Dictionary<int, Game>();
        private readonly static Queue<PendingGame> pendingGames = new Queue<PendingGame>();


        //new database
        private static string BoggleDB;

        static BoggleService()
        {
            BoggleDB = ConfigurationManager.ConnectionStrings["BoggleDB"].ConnectionString;
        }
        /// <summary>
        /// The most recent call to SetStatus determines the response code used when
        /// an http response is sent.
        /// </summary>
        /// <param name="status"></param>
        private static void SetStatus(HttpStatusCode status)
        {
            WebOperationContext.Current.OutgoingResponse.StatusCode = status;
        }

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

        /// <summary>
        /// This handles a register user request and checks if the username is null and returns
        /// a forbidden response if that is true and returns a userID is not.
        /// </summary>
        /// <param name="nickname"></param>
        /// <returns></returns>
        public UserResponse Register(Username name)
        {
            //check validity 
            if (name.Nickname == null || name.Nickname.Trim().Length == 0 || name.Nickname.Trim().Length > 50)
            {
                SetStatus(Forbidden);
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
                        SetStatus(Created);

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

        public JoinGameResponse JoinGame(GameRequest request)
        {

            string GameID = null;

            //ensure the userID is not null
            if (request.UserToken == null)
            {
                SetStatus(Forbidden);
                return null;
            }

            if (request.TimeLimit < 5 || request.TimeLimit > 120)
            {
                SetStatus(Forbidden);
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
                                SetStatus(Forbidden);
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
                                        SetStatus(Conflict);
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
                            pendingCommand.Parameters.AddWithValue("@Board", board);
                            pendingCommand.Parameters.AddWithValue("@GameID", pendingGameID);

                            int oldTimeLimit = pendingTime;
                            int newTimeLimit = (oldTimeLimit + request.TimeLimit) / 2;
                            pendingCommand.Parameters.AddWithValue("@TimeLimit", newTimeLimit);
                            int currentTime = (int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
                            pendingCommand.Parameters.AddWithValue("@StartTime", currentTime);

                            pendingCommand.ExecuteScalar();

                            JoinGameResponse response = new JoinGameResponse();
                            response.GameID = pendingGameID;



                            trans.Commit();
                            SetStatus(Created);
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

                            SetStatus(Accepted);
                            return response;
                        }


                    }
                }
            }
        
    

        }

        /// <summary>
        /// Demo.  You can delete this.
        /// </summary>
        public string WordAtIndex(int n)
        {
            if (n < 0)
            {
                SetStatus(Forbidden);
                return null;
            }

            string line;
            using (StreamReader file = new System.IO.StreamReader(AppDomain.CurrentDomain.BaseDirectory + "dictionary.txt"))
            {
                while ((line = file.ReadLine()) != null)
                {
                    if (n == 0) break;
                    n--;
                }
            }

            if (n == 0)
            {
                SetStatus(OK);
                return line;
            }
            else
            {
                SetStatus(Forbidden);
                return null;
            }
        }

        /// <summary>
        /// gets the user out of the queue
        /// </summary>
        /// <param name="userID"></param>
        public void CancelQueue(CancelGameRequest userID)
        {
            

            //ensure the userID is not null
            if (userID.UserToken == null)
            {
                SetStatus(Forbidden);
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
                                SetStatus(Forbidden);
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
                    if(isPendingGame)
                    {
                        using (SqlCommand command = new SqlCommand("delete from Games where GameID = @GameID", conn, trans))
                        {
                            command.Parameters.AddWithValue("@GameID", pendingGameID);

                            if(command.ExecuteNonQuery() == 0)
                            {
                                SetStatus(Forbidden);
                                trans.Commit();
                            }
                            else
                            {
                                SetStatus(OK);
                                trans.Commit(); 
                            }
                        }
                    }
                    else
                    {
                        SetStatus(Forbidden);
                        trans.Commit();
                    }
                }
            }
        }

        public PlayWordResponse PlayWord(WordAttempt attempt, string GameID)
        {
            int GameID_int;
            try
            {
                GameID_int = Int32.Parse(GameID);
            }

            catch (Exception ex)
            {
                SetStatus(Forbidden);
                return null;
            }


            //If Word is null or empty or longer than 30 characters when trimmed, responds with response code 403 (Forbidden).
            if (attempt.Word == null || attempt.Word.Length <= 0 || attempt.Word.Length > 30)
            {
                SetStatus(Forbidden);
                return null;
            }

            //instane variables for saving the game info
            string player1 = null;
            string player2 = null;
            BoggleBoard board = null;
            int timeLimit = 0;
            int startTime = 0;
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
                        command1.Parameters.AddWithValue("@GameID", GameID);


                        // This executes a query (i.e. a select statement).  The result is an
                        // SqlDataReader that you can use to iterate through the rows in the response.
                        using (SqlDataReader reader1 = command1.ExecuteReader())
                        {
                            if (reader1.HasRows)
                            {
                                player1 = reader1["Player1"].ToString();
                                player2 = reader1["Player2"].ToString();
                                board = (BoggleBoard)reader1["Board"];
                                timeLimit = (int)reader1["TimeLimit"];
                                startTime = (int)reader1["StatrTime"];


                                //check if the user is in the proper game
                                if (!(attempt.UserToken == player1 || attempt.UserToken == player2))
                                {
                                    SetStatus(Forbidden);
                                    reader1.Close();
                                    trans.Commit();
                                    return null;
                                }

                                //see if the game is still active

                                int currentTime = (int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
                                int timeElapsed = currentTime - startTime;
                                timeLeft = timeLimit - timeElapsed;
                            }
                            else
                            {
                                SetStatus(Forbidden);
                                reader1.Close();
                                trans.Commit();
                                return null;
                            }
                            reader1.Close();
                        }
                    }

                    if (timeLeft <= 0)
                    {
                        SetStatus(Conflict);
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
                            using (SqlCommand command2 = new SqlCommand("select * Words;", conn, trans))
                            {
                                using (SqlDataReader reader1 = command2.ExecuteReader())
                                {
                                    if(reader1.HasRows)
                                    {
                                        while(reader1.Read())
                                        {
                                            if (attempt.Word == reader1["Word"].ToString())
                                            {
                                                string user = (string)reader1["Player1"];
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
                                using (StreamReader words = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "dictionary.txt"))
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



                                if (isActualWord == true)
                                {
                                    if (board.CanBeFormed(attempt.Word))
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
                                        score = -1;
                                    }
                                }

                                else
                                {
                                    //subtract from score
                                    score = -1;
                                }
                            }
                            

                            
                            
                        }
                        else
                        {
                            //PLayer2 played the word
                            isPlayer1 = false;

                            //is the word a duplicate word for player 2
                            using (SqlCommand command2 = new SqlCommand("select * Words;", conn, trans))
                            {
                                using (SqlDataReader reader1 = command2.ExecuteReader())
                                {
                                    if (reader1.HasRows)
                                    {
                                        while (reader1.Read())
                                        {
                                            if (attempt.Word == reader1["Word"].ToString())
                                            {
                                                string user = (string)reader1["Player2"];
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
                                using (StreamReader words = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "dictionary.txt"))
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



                                if (isActualWord == true)
                                {
                                    if (board.CanBeFormed(attempt.Word))
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
                                        score = -1;
                                    }
                                }

                                else
                                {
                                    //subtract from score
                                    score = -1;
                                }
                            }
                        }
                        //Update Words
                        using (SqlCommand command2 = new SqlCommand("insert into Words (Word, GameID, Player, Score) values(@Word, @GameID, @Player, @Score)", conn, trans))
                        {
                            command2.Parameters.AddWithValue("@Word", attempt.Word);
                            command2.Parameters.AddWithValue("@GameID", GameID);
                            command2.Parameters.AddWithValue("@Player", attempt.UserToken);
                            command2.Parameters.AddWithValue("@Score", score);

                            if(command2.ExecuteNonQuery() == 0)
                            {
                                SetStatus(OK);
                                PlayWordResponse response = new PlayWordResponse();
                                response.Score = score;
                                return response;
                            }
                        }
                    }
                }
            }
        }

        public Game GetStatus(string GameID, string brief)
        {
            int GameID_int;
            try
            {
                GameID_int = Int32.Parse(GameID);
            }

            catch(Exception ex)
            {
                SetStatus(Forbidden);
                return null;
            }

            if (!games.ContainsKey(GameID_int))
            {
                SetStatus(Forbidden);
                return null;
            }

            if(games[GameID_int].GameState == "pending")
            {
                Game pendingGame = new Game();
                pendingGame.GameState = "pending";
                SetStatus(OK);
                return pendingGame;
            }

            //check if brief was a paramater or not
            if (brief != "yes")
            {
                SetStatus(OK);

                int timeLimit = games[GameID_int].TimeLimit;
                int startTime = games[GameID_int].StartTime;
                int currentTime = (int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
                int timeElapsed = currentTime - startTime;
                int timeLeft = timeLimit - timeElapsed;

                if (currentTime - startTime >= timeLimit)
                {
                    //game is complete
                    Game gameComplete = new Game();

                    gameComplete.GameState = "completed";
                    gameComplete.TimeLeft = 0;
                    gameComplete.TimeLimit = timeLimit;

                    gameComplete.Board = games[GameID_int].Board;

                    Player1 playerOneComplete = new Player1();
                    gameComplete.Player1 = playerOneComplete;
                    Player2 playerTwoComplete = new Player2();
                    gameComplete.Player2 = playerTwoComplete;

                    playerOneComplete.Score = games[GameID_int].Player1.Score;
                    playerOneComplete.Nickname = games[GameID_int].Player1.Nickname;
                    List<WordPlayed> WordsPlayed = new List<WordPlayed>();
                    playerOneComplete.WordsPlayed = WordsPlayed;
                    playerOneComplete.WordsPlayed = games[GameID_int].Player1.WordsPlayed;

                    playerTwoComplete.Score = games[GameID_int].Player2.Score;
                    playerTwoComplete.Nickname = games[GameID_int].Player2.Nickname;
                    List<WordPlayed> WordsPlayed2 = new List<WordPlayed>();
                    playerTwoComplete.WordsPlayed = WordsPlayed2;
                    playerTwoComplete.WordsPlayed = games[GameID_int].Player2.WordsPlayed;

                    return gameComplete;
                }
                else
                {
                    //game is active
                    Game gameActive = new Game();

                    gameActive.GameState = "active";
                    gameActive.TimeLeft = timeLeft;
                    gameActive.TimeLimit = timeLimit;

                    gameActive.Board = games[GameID_int].Board;

                    Player1 playerOneActive = new Player1();
                    gameActive.Player1 = playerOneActive;
                    Player2 PlayerTwoActive = new Player2();
                    gameActive.Player2 = PlayerTwoActive;

                    playerOneActive.Score = games[GameID_int].Player1.Score;
                    playerOneActive.Nickname = games[GameID_int].Player1.Nickname;
                    
                    PlayerTwoActive.Score = games[GameID_int].Player2.Score;
                    PlayerTwoActive.Nickname = games[GameID_int].Player2.Nickname;
                    

                    return gameActive;

                }
            }
            if(brief == "yes")
            {
                SetStatus(OK);

                int timeLimit = games[GameID_int].TimeLimit;
                int startTime = games[GameID_int].StartTime;
                int currentTime = (int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
                int timeElapsed = currentTime - startTime;
                int timeLeft = timeLimit - timeElapsed;

                if(timeElapsed >= timeLimit)
                {
                    //game is complete
                    Game gameComplete = new Game();

                    gameComplete.GameState = "completed";
                    gameComplete.TimeLeft = 0;

                    Player1 playerOneComplete = new Player1();
                    gameComplete.Player1 = playerOneComplete;
                    Player2 playerTwoComplete = new Player2();
                    gameComplete.Player2 = playerTwoComplete;

                    playerOneComplete.Score = games[GameID_int].Player1.Score;
                    playerTwoComplete.Score = games[GameID_int].Player2.Score;

                    return gameComplete;
                }
                else
                {
                    //game is active
                    Game gameComplete = new Game();

                    gameComplete.GameState = "active";
                    gameComplete.TimeLeft = timeLeft;

                    Player1 playerOneComplete = new Player1();
                    gameComplete.Player1 = playerOneComplete;
                    Player2 playerTwoComplete = new Player2();
                    gameComplete.Player2 = playerTwoComplete;

                    playerOneComplete.Score = games[GameID_int].Player1.Score;
                    playerTwoComplete.Score = games[GameID_int].Player2.Score;

                    return gameComplete;
                }
            }

            return null;

        }
    }
}
