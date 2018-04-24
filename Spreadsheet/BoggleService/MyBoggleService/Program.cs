using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static Boggle.DataModels;
using CustomNetworking;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace Boggle
{
    class Program
    {
        private SSListener server;
        public static void Main(string[] args)
        {
            new Program();
            Console.ReadLine();

        }

        public Program()
        {
            server = new SSListener(60000, Encoding.UTF8);
            server.Start();
            server.BeginAcceptSS(ConnectionMade, server);
        }

        private void ConnectionMade(SS ss, object payload)
        {
            
            server.BeginAcceptSS(ConnectionMade, null);
            new RequestHandler(ss);
        }

        /// <summary>
        /// This class wraps a StringSocket and deals with the request it is transmitting
        /// </summary>
        private class RequestHandler
        {
            // the socket making the request
            private SS ss;

            // the first line from the socket or null if not read yet
            private string firstLine;

            // the value of the Content-Length header or zero if no such header seen yet
            private int contentLength;

            // Matches the first line of a "make user" request
            private static readonly Regex makeUserPattern = new Regex(@"^POST /BoggleService.svc/users HTTP");

            // Matches the first line of a "join game" request
            private static readonly Regex joinGamePattern = new Regex(@"^POST /BoggleService.svc/games HTTP");

            // Matches the first line of a "cancel join" request
            private static readonly Regex cancelJoinPattern = new Regex(@"^PUT /BoggleService.svc/games HTTP");

            // Matches the first line of a "play word" request
            private static readonly Regex playWordPattern = new Regex(@"^PUT /BoggleService.svc/games/([a-z]*[A-Z]*\d*) HTTP");

            // Matches the first line of a "game status" request
            private static readonly Regex briefStatusPattern = new Regex(@"^GET /BoggleService.svc/games/([a-z]*[A-Z]*\d*)\?brief=([a-z]+) HTTP");

            // Matches the first line of a "game status" request
            private static readonly Regex gameStatusPattern = new Regex(@"^GET /BoggleService.svc/games/([a-z]*[A-Z]*\d*) HTTP");




            // Matches a content-length header and extracts the integer
            private static readonly Regex contentLengthPattern = new Regex(@"^content-length: (\d+)", RegexOptions.IgnoreCase);

            /// <summary>
            /// Builds the request handler and begins recieving lines
            /// </summary>
            /// <param name="ss"></param>
            public RequestHandler(SS ss)
            {
                this.ss = ss;
                contentLength = 0;
                ss.BeginReceive(ReadLines, null);
            }
            /// <summary>
            /// Reads one line at a time until all the informaiton 
            /// has been extracted. Then lets ProcessRequest Finish Up.
            /// </summary>
            /// <param name="line"></param>
            /// <param name="p"></param>
            private void ReadLines(String line, object p)
            {
                if (line.Trim().Length == 0 && contentLength > 0)
                {
                    ss.BeginReceive(ProcessRequest, null, contentLength);
                }
                else if (line.Trim().Length == 0)
                {
                    ProcessRequest(null);
                }
                else if (firstLine != null)
                {
                    Match m = contentLengthPattern.Match(line);
                    if (m.Success)
                    {
                        contentLength = int.Parse(m.Groups[1].ToString());
                    }
                    ss.BeginReceive(ReadLines, null);
                }
                else
                {
                    firstLine = line;
                    ss.BeginReceive(ReadLines, null);
                }
                    

            }

            /// <summary>
            /// Handles the rquest after all relevant information has been parsed out.
            /// </summary>
            /// <param name="line"></param>
            /// <param name="p"></param>
            private void ProcessRequest (string line, object p = null)
            {
                
                // This handles "make user" requests
                if(makeUserPattern.IsMatch(firstLine))
                {
                    Username n = new Username();
                    if(line != null)
                    {
                        n = JsonConvert.DeserializeObject<Username>(line);
                    }
                   else
                    {
                        n.Nickname = null;
                    }
                    UserResponse user = new BoggleService().Register(n, out HttpStatusCode status);
                    String result = "HTTP/1.1 " + (int)status + " " + status + "\r\n";
                    if((int)status / 100 == 2)
                    {
                        string res = JsonConvert.SerializeObject(user);
                        result += "Content-Lenth: " + Encoding.UTF8.GetByteCount(res) + "\r\n\r\n";
                        result += res;
                    }
                    else
                    {
                        result += "\r\n";
                    }
                    ss.BeginSend(result, (x, y) => { ss.Shutdown(System.Net.Sockets.SocketShutdown.Both); }, null);
                }

                // This handles "join game" requests
                else if (joinGamePattern.IsMatch(firstLine))
                {
                    GameRequest gr = JsonConvert.DeserializeObject<GameRequest>(line);
                    JoinGameResponse game = new BoggleService().JoinGame(gr, out HttpStatusCode status);
                    String result = "HTTP/1.1 " + (int)status + " " + status + "\r\n";
                    if ((int)status / 100 == 2)
                    {
                        string res = JsonConvert.SerializeObject(game);
                        result += "Content-Lenth: " + Encoding.UTF8.GetByteCount(res) + "\r\n\r\n";
                        result += res;
                    }
                    else
                    {
                        result += "\r\n";
                    }
                    ss.BeginSend(result, (x, y) => { ss.Shutdown(System.Net.Sockets.SocketShutdown.Both); }, null);
                }

                // This handles "cancel join" requests
                else if (cancelJoinPattern.IsMatch(firstLine))
                {
                    CancelGameRequest cgr = JsonConvert.DeserializeObject<CancelGameRequest>(line);
                    new BoggleService().CancelQueue(cgr, out HttpStatusCode status);
                    String result = "HTTP/1.1 " + (int)status + " " + status + "\r\n\r\n";
                    
                    ss.BeginSend(result, (x, y) => { ss.Shutdown(System.Net.Sockets.SocketShutdown.Both); }, null);
                }

                // This handles "play word" requests
                else if (playWordPattern.IsMatch(firstLine))
                {
                    String GameID = null;
                    WordAttempt word;

                    if(line != null)
                    {
                        word = JsonConvert.DeserializeObject<WordAttempt>(line);

                        Match m = playWordPattern.Match(firstLine);
                        if (m.Success)
                        {
                            GameID = m.Groups[1].ToString();//get the Game ID
                        }
                    }
                    else
                    {
                        Match m = playWordPattern.Match(firstLine);
                        if (m.Success)
                        {
                            GameID = m.Groups[0].ToString();//get the Game ID
                            word = new WordAttempt();
                        }
                        word = new WordAttempt();
                    }

                   

                    PlayWordResponse wordResponse = new BoggleService().PlayWord(word, GameID, out HttpStatusCode status);
                    String result = "HTTP/1.1 " + (int)status + " " + status + "\r\n";
                    if ((int)status / 100 == 2)
                    {
                        string res = JsonConvert.SerializeObject(wordResponse);
                        result += "Content-Lenth: " + Encoding.UTF8.GetByteCount(res) + "\r\n\r\n";
                        result += res;
                    }
                    else
                    {
                        result += "\r\n";
                    }
                    ss.BeginSend(result, (x, y) => { ss.Shutdown(System.Net.Sockets.SocketShutdown.Both); }, null);
                }

                // This handles "game status" requests
                else if (briefStatusPattern.IsMatch(firstLine))
                {
                    String GameID = null;
                    String brief = null;
                    Game game;
                    if (line != null)
                    {
                        game = JsonConvert.DeserializeObject<Game>(line);
                        Match m = briefStatusPattern.Match(line);
                        if (m.Success)
                        {
                            GameID = m.Groups[0].ToString();//get the Game ID
                            brief = m.Groups[1].ToString();//get brief
                        }
                    }
                    else
                    {
                        //game = JsonConvert.DeserializeObject<Game>(firstLine);
                        Match m = briefStatusPattern.Match(firstLine);
                        if (m.Success)
                        {
                            GameID = m.Groups[1].ToString();//get the Game ID
                            brief = m.Groups[2].ToString();//get brief
                        }
                    }
                    
                    
                    game = new BoggleService().GetStatus(GameID, brief, out HttpStatusCode status);
                    String result = "HTTP/1.1 " + (int)status + " " + status + "\r\n";
                    if ((int)status / 100 == 2)
                    {
                        string res = JsonConvert.SerializeObject(game);
                        result += "Content-Lenth: " + Encoding.UTF8.GetByteCount(res) + "\r\n\r\n";
                        result += res;
                    }
                    else
                    {
                        result += "\r\n";
                    }
                    ss.BeginSend(result, (x, y) => { ss.Shutdown(System.Net.Sockets.SocketShutdown.Both); }, null);
                }

                // This handles "game status" requests
                else if (gameStatusPattern.IsMatch(firstLine))
                {
                    String GameID = null;
                    String brief = null;
                    Game game;
                    if (line != null)
                    {
                        game = JsonConvert.DeserializeObject<Game>(line);
                        Match m = briefStatusPattern.Match(line);
                        if (m.Success)
                        {
                            GameID = m.Groups[0].ToString();//get the Game ID
                          
                        }
                    }
                    else
                    {
                        //game = JsonConvert.DeserializeObject<Game>(firstLine);
                        Match m = briefStatusPattern.Match(firstLine);
                        if (m.Success)
                        {
                            GameID = m.Groups[1].ToString();//get the Game ID
                            brief = m.Groups[2].ToString();//get brief
                        }
                    }
                    game = new BoggleService().GetStatus(GameID, brief, out HttpStatusCode status);
                    String result = "HTTP/1.1 " + (int)status + " " + status + "\r\n";
                    if ((int)status / 100 == 2)
                    {
                        string res = JsonConvert.SerializeObject(game);
                        result += "Content-Lenth: " + Encoding.UTF8.GetByteCount(res) + "\r\n\r\n";
                        result += res;
                    }
                    else
                    {
                        result += "\r\n";
                    }
                    ss.BeginSend(result, (x, y) => { ss.Shutdown(System.Net.Sockets.SocketShutdown.Both); }, null);
                }


            }
        }
    }
}
