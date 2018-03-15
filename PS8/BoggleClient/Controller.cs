using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BoggleClient
{
    class Controller
    {
        /// <summary>
        /// The view controlled by this Controller
        /// </summary>
        private BoggleView view;

        /// <summary>
        /// For canceling the current operation
        /// </summary>
        private CancellationTokenSource tokenSource;

        /// <summary>
        /// fires every second to get infromation from the server
        /// </summary>
        private System.Windows.Forms.Timer timer;

        /// <summary>
        /// The token of the most recently registered user, or "0" if no user
        /// has ever registered
        /// </summary>
        private string userToken;

        /// <summary>
        /// The token of gme the user joined
        /// </summary>
        private string gameToken;

        /// <summary>
        /// the domain name that the user entered
        /// </summary>
        private string domain;

        /// <summary>
        /// Constructs the controller taking a boggle view gui as a paramater
        /// </summary>
        /// <param name="view"></param>
        public Controller(BoggleView view)
        {
            this.view = view;
            view.IsUserRegistered = false;
            view.IsUserInGame = false;
            view.EnableControls(true);
            view.RegisterUser += Register;
            view.CancelPressed += Cancel;
            view.PlayGame += Play;
            view.CancelGameRequestPressed += CancelGameRequest;
            view.EnterButtonPressed += WordEntered;
            view.LeavePressed += ResetBoggle;
        }

        /// <summary>
        /// Registers a user with the given name
        /// </summary>
        private async void Register(string name, string domain)
        {
            try
            {
                view.EnableControls(false);
                using (HttpClient client = CreateClient(domain))
                {
                    this.domain = domain;
                    // Create the parameter
                    dynamic user = new ExpandoObject();
                    user.Nickname = name;

                    // Compose and send the request.
                    tokenSource = new CancellationTokenSource();
                    StringContent content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync("users", content, tokenSource.Token);

                    // Deal with the response
                    if (response.IsSuccessStatusCode)
                    {
                        String result = await response.Content.ReadAsStringAsync();
                        String userTokenTemp = JsonConvert.DeserializeObject(result).ToString();
                        JObject obj = JObject.Parse(userTokenTemp.ToString()); // it will parse deserialize Json object
                        userToken = obj["UserToken"].ToString(); // now after parsing deserialize Json object you can get individual values by key i.e.
                        view.IsUserRegistered = true;

                        Console.WriteLine(name + " " + userToken);
                    }
                    else
                    {
                        MessageBox.Show("Error registering: " + response.StatusCode + "\n" + response.ReasonPhrase);
                    }
                }
            }
            catch (TaskCanceledException)
            {
            }
            finally
            {
                view.EnableControls(true);
            }
        }

        /// <summary>
        /// Cancels the current registration operation (currently unimplemented)
        /// </summary>
        private void Cancel()
        {
            tokenSource.Cancel();
        }

        /// <summary>
        /// cancells the user request to join a game
        /// </summary>
        private async void CancelGameRequest()
        {
            try
            {
                view.EnableControls(true);
                using (HttpClient client = CreateClient(domain))
                {
                    // Create the parameter
                    dynamic game = new ExpandoObject();
                    game.UserToken = userToken;

                    // Compose and send the request
                    StringContent content = new StringContent(JsonConvert.SerializeObject(game), Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PutAsync("games", content);

                    // Deal with the response
                    if (response.IsSuccessStatusCode)
                    {
                        timer.Stop();
                        view.IsUserInGame = false;
                        view.IsUserPending = false;

                    }
                    else
                    {
                        MessageBox.Show("Error Exiting Game: " + response.StatusCode + "\n" + response.ReasonPhrase);
                    }
                }
            }
            catch (TaskCanceledException)
            {

            }
            finally
            {
                view.EnableControls(true);
            }
        }

        /// <summary>
        /// Trys to join a game on the boggle server with the specified time
        /// </summary>
        private async void Play(string time)
        {
            
            try
            {
                view.EnableControls(false);
                using (HttpClient client = CreateClient(domain))
                {
                    // Create the parameter
                    dynamic game = new ExpandoObject();
                    game.UserToken = userToken;
                    game.TimeLimit = Int32.Parse(time);

                    // Compose and send the request.
                    tokenSource = new CancellationTokenSource();
                    StringContent content = new StringContent(JsonConvert.SerializeObject(game), Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync("games", content, tokenSource.Token);

                    // Deal with the response
                    if (response.IsSuccessStatusCode)
                    {
                        String result = await response.Content.ReadAsStringAsync();
                        String gameTokenTemp = JsonConvert.DeserializeObject(result).ToString();
                        JObject obj = JObject.Parse(gameTokenTemp.ToString()); // it will parse deserialize Json object
                        gameToken = obj["GameID"].ToString(); // now after parsing deserialize Json object you can get individual values by key i.e.


                        view.IsUserPending = true;

                        //set up game update timer
                        timer = new System.Windows.Forms.Timer();
                        timer.Tick += new EventHandler(timerTick);
                        timer.Interval = 1000;//1sec
                        timer.Start();


                    }
                    else
                    {
                        MessageBox.Show("Error Finding Game: " + response.StatusCode + "\n" + response.ReasonPhrase);
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex is TaskCanceledException)
                {

                }

                else if (ex is FormatException)
                {
                    MessageBox.Show("Invalid Time Limit");
                }
            }
            finally
            {
                view.EnableControls(true);
            }
        }

        /// <summary>
        /// fires every second, gets the gamestate from the server and applies it 
        /// where necessary 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timerTick(object sender, EventArgs e)
        {
            if(view.IsUserPending)
            {
                this.SetUp();
            }
            else if(view.IsUserInGame)
            {
                this.Update();
            }
        }

        /// <summary>
        /// when the game starts this sets up the board, stays in stasis while pending
        /// 
        /// also displays final words for both players after the game has finished
        /// </summary>
        private async void SetUp()
        {
            
            try
            {
                
               
                    view.EnableControls(false);
                using (HttpClient client = CreateClient(domain))
                {

                    // Compose and send the GET request.
                    HttpResponseMessage response = await client.GetAsync("games/" + gameToken);

                    // Deal with the response
                    if (response.IsSuccessStatusCode)
                    {
                        String result = await response.Content.ReadAsStringAsync();
                        dynamic items = JsonConvert.DeserializeObject(result);
                        String parsable = items.ToString();
                        JObject obj = JObject.Parse(parsable); // it will parse deserialize Json object
                        String gameState = obj["GameState"].ToString(); // now after parsing deserialize Json object you can get individual values by key i.e.
                        if (gameState == "pending")
                        {
                            view.IsUserPending = true;

                        }
                        else if (gameState == "active")
                        {
                            view.IsUserInGame = true;
                            view.IsUserPending = false;
                            //Set the board and the time
                            String board = obj["Board"].ToString();
                            String TimeLeft = obj["TimeLeft"].ToString();
                            view.startBoard(board);

                            //Get Player One Info
                            JObject playerOne = JObject.Parse(obj["Player1"].ToString());
                            String playerOneName = playerOne["Nickname"].ToString();
                            int scoreOne = (int)playerOne["Score"];


                            //Get Player Two Info
                            JObject playerTwo = JObject.Parse(obj["Player2"].ToString());
                            String playerTwoName = playerTwo["Nickname"].ToString();
                            int scoreTwo = (int)playerTwo["Score"];



                        }
                        //when the game completes get the words both players have entered
                        //and updates the corresponding panels
                        else if (gameState == "completed")
                        {
                            timer.Stop();

                            foreach (dynamic item in items.Player1.WordsPlayed)
                            {
                                view.updateEndOne(item.Word.ToString());
                            }


                            foreach (dynamic item in items.Player2.WordsPlayed)
                            {
                                view.updateEndTwo(item.Word.ToString());
                            }

                            view.IsUserInGame = false;
                            


                        }
                        else
                        {
                            MessageBox.Show("Error Finding Game: " + response.StatusCode + "\n" + response.ReasonPhrase);
                        }
                    }

                }   
            }

            catch (Exception ex)
            {
                if (ex is TaskCanceledException)
                {

                }

                else if (ex is FormatException)
                {
                    MessageBox.Show("Invalid Time Limit");
                }
            }
            finally
            {
                if (!view.IsUserPending)
                {
                    view.EnableControls(true);
                }


            }
        
        
        }
    
        /// <summary>
        /// while a game is running the timer calls this each second to get real time
        /// updates from the server about the game
        /// </summary>
        private async void Update()
        {
            try
            {
   
                view.EnableControls(false);
                using (HttpClient client = CreateClient(domain))
                {

                    // Compose and send the GET request.
                    HttpResponseMessage response = await client.GetAsync("games/" + gameToken + "?Brief=yes");

                    // Deal with the response
                    if (response.IsSuccessStatusCode)
                    {
                        String result = await response.Content.ReadAsStringAsync();
                        dynamic items = JsonConvert.DeserializeObject(result);
                        String parsable = items.ToString();
                        JObject obj = JObject.Parse(parsable); // it will parse deserialize Json object
                        String gameState = obj["GameState"].ToString(); // now after parsing deserialize Json object you can get individual values by key i.e.
                        if (gameState == "active")
                        {


                            String timeLeft = obj["TimeLeft"].ToString();


                            //Get Player One Info
                            JObject playerOne = JObject.Parse(obj["Player1"].ToString());
                            string scoreOne = playerOne["Score"].ToString();


                            //Get Player Two Info
                            JObject playerTwo = JObject.Parse(obj["Player2"].ToString());
                            string scoreTwo = playerTwo["Score"].ToString();

                            view.updateData(timeLeft, scoreOne.ToString(), scoreTwo.ToString());



                        }
                        else if (gameState == "completed")
                        {
                            this.SetUp();

                        }

                        else
                        {
                            MessageBox.Show("Error Finding Game: " + response.StatusCode + "\n" + response.ReasonPhrase);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex is TaskCanceledException)
                {

                }

                else if (ex is FormatException)
                {
                    MessageBox.Show("Invalid Time Limit");
                }
            }
            finally
            {
                view.EnableControls(true);
            }
        }

        /// <summary>
        /// when the user enteres a word send it to the server
        /// </summary>
        /// <param name="word"></param>
        private async void WordEntered(string word)
        {
            try
            {
                view.EnableControls(false);
                using (HttpClient client = CreateClient(domain))
                {
                    // Create the parameter
                    dynamic newWord = new ExpandoObject();
                    newWord.UserToken = userToken;
                    newWord.Word = word;

                    // Compose and send the request
                    StringContent content = new StringContent(JsonConvert.SerializeObject(newWord), Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PutAsync("games/" + gameToken, content);

                    // Deal with the response
                    if (response.IsSuccessStatusCode)
                    {
                        
                        view.newWord(word);
                    }
                    else
                    {
                        MessageBox.Show("Error Entering Word: " + response.StatusCode + "\n" + response.ReasonPhrase);
                    }
                }
            }
            catch (TaskCanceledException)
            {

            }
            finally
            {
                view.EnableControls(true);
            }
        }
        /// <summary>
        /// Creates an HttpClient for communicating with the server.
        /// </summary>
        private static HttpClient CreateClient(string domain)
        {
            // Create a client whose base address is the GitHub server
            HttpClient client = new HttpClient
            {
                BaseAddress = new Uri(domain)
            };  

            // Tell the server that the client will accept this particular type of response data
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Add("Accept", "application/json");

            // There is more client configuration to do, depending on the request.
            return client;
        }

        /// <summary>
        /// reset the view to its normal state
        /// </summary>
        private void ResetBoggle()
        {
            timer.Stop();
            view.IsUserInGame = false;
            view.Reset();
            view.EnableControls(true);
        }
    }
}
