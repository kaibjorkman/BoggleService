using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static System.Net.HttpStatusCode;
using System.Diagnostics;
using Newtonsoft.Json;
using System.Dynamic;
using static Boggle.DataModels;
using System.Threading;

namespace Boggle
{
    /// <summary>
    /// Provides a way to start and stop the IIS web server from within the test
    /// cases.  If something prevents the test cases from stopping the web server,
    /// subsequent tests may not work properly until the stray process is killed
    /// manually.
    /// </summary>
    public static class IISAgent
    {
        // Reference to the running process
        private static Process process = null;

        /// <summary>
        /// Starts IIS
        /// </summary>
        public static void Start(string arguments)
        {
            if (process == null)
            {
                ProcessStartInfo info = new ProcessStartInfo(Properties.Resources.IIS_EXECUTABLE, arguments);
                info.WindowStyle = ProcessWindowStyle.Minimized;
                info.UseShellExecute = false;
                process = Process.Start(info);
            }
        }

        /// <summary>
        ///  Stops IIS
        /// </summary>
        public static void Stop()
        {
            if (process != null)
            {
                process.Kill();
            }
        }
    }
    [TestClass]
    public class BoggleTests
    {
        /// <summary>
        /// This is automatically run prior to all the tests to start the server
        /// </summary>
        [ClassInitialize()]
        public static void StartIIS(TestContext testContext)
        {
            IISAgent.Start(@"/site:""BoggleService"" /apppool:""Clr4IntegratedAppPool"" /config:""..\..\..\.vs\config\applicationhost.config""");
        }

        /// <summary>
        /// This is automatically run when all tests have completed to stop the server
        /// </summary>
        [ClassCleanup()]
        public static void StopIIS()
        {
            IISAgent.Stop();
        }

        private RestTestClient client = new RestTestClient("http://localhost:60000/BoggleService.svc/");

        /// <summary>
        /// Note that DoGetAsync (and the other similar methods) returns a Response object, which contains
        /// the response Stats and the deserialized JSON response (if any).  See RestTestClient.cs
        /// for details.
        /// </summary>
        [TestMethod]
        public void TestMethod1()
        {
            Response r = client.DoGetAsync("word?index={0}", "-5").Result;
            Assert.AreEqual(Forbidden, r.Status);

            r = client.DoGetAsync("word?index={0}", "5").Result;
            Assert.AreEqual(OK, r.Status);

            string word = (string) r.Data;
            Assert.AreEqual("AAL", word);
        }

        [TestMethod]
        public void RegisterUserTest()
        {
            Username user = new Username();
            user.Nickname = "Joe";

            Response r = client.DoPostAsync("users", user).Result;
            Assert.AreEqual(Created, r.Status);

            user.Nickname = "";
            r = client.DoPostAsync("users", user).Result;
            Assert.AreEqual(Forbidden, r.Status);

            user.Nickname = null;
            r = client.DoPostAsync("users", user).Result;
            Assert.AreEqual(Forbidden, r.Status);
        }

        [TestMethod]
        public void JoinGameTest()
        {
            Username user1 = new Username();
            user1.Nickname = "Joe";
            Response u1 = client.DoPostAsync("users", user1).Result;

            Username user2 = new Username();
            user2.Nickname = "Joe";
            Response u2 = client.DoPostAsync("users", user2).Result;

            Username user3 = new Username();
            user3.Nickname = "Joe";
            Response u3 = client.DoPostAsync("users", user3).Result;

            //normal request accepted
            GameRequest request1 = new GameRequest();
            request1.UserToken = u1.Data.ToString();
            request1.TimeLimit = 50;

            Response r1 = client.DoPostAsync("games", request1).Result;
            Assert.AreEqual(Accepted, r1.Status);

            //user alread in queue
            
            Response r1_5 = client.DoPostAsync("games", request1).Result;
            Assert.AreEqual(Conflict, r1_5.Status);

            //normal request created
            GameRequest request2 = new GameRequest();
            request2.UserToken = u2.Data.ToString();
            request2.TimeLimit = 50;

            Response r2 = client.DoPostAsync("games", request2).Result;
            Assert.AreEqual(Created, r2.Status);

            //user token invalid
            GameRequest request3 = new GameRequest();
            request3.UserToken = "e";
            request3.TimeLimit = 50;

            Response r3 = client.DoPostAsync("games", request3).Result;
            Assert.AreEqual(Forbidden, r3.Status);

            //time limit invalid
            GameRequest request4 = new GameRequest();
            request4.UserToken = u3.Data.ToString();
            request4.TimeLimit = 700;

            Response r4 = client.DoPostAsync("games", request4).Result;
            Assert.AreEqual(Forbidden, r4.Status);
        }

        [TestMethod]
        public void CancelJoinGameTest()
        {
            Username user1 = new Username();
            user1.Nickname = "Joe";
            Response u1 = client.DoPostAsync("users", user1).Result;

            //user not in queue
            CancelGameRequest request3 = new CancelGameRequest();
            request3.UserToken = u1.Data.ToString();

            Response r3 = client.DoPutAsync(request3, "games").Result;
            Assert.AreEqual(Forbidden, r3.Status);

            //normal request accepted
            GameRequest request1 = new GameRequest();
            request1.UserToken = u1.Data.ToString();
            request1.TimeLimit = 50;

            Response r1 = client.DoPostAsync("games", request1).Result;
            Assert.AreEqual(Accepted, r1.Status);

            CancelGameRequest request2 = new CancelGameRequest();
            request2.UserToken = u1.Data.ToString();

            Response r2 = client.DoPutAsync(request2, "games").Result;
            Assert.AreEqual(OK, r2.Status);

            //user token invalid
            CancelGameRequest request4 = new CancelGameRequest();
            request4.UserToken = "e";

            Response r4 = client.DoPutAsync(request4, "games").Result;
            Assert.AreEqual(Forbidden, r4.Status);
        }

        [TestMethod]
        public void PlayWordTest()
        {
            Username user1 = new Username();
            user1.Nickname = "Joe";
            Response u1 = client.DoPostAsync("users", user1).Result;

            Username user2 = new Username();
            user2.Nickname = "Joe";
            Response u2 = client.DoPostAsync("users", user2).Result;

            Username user3 = new Username();
            user3.Nickname = "Joe";
            Response u3 = client.DoPostAsync("users", user3).Result;

            

            //create game
            GameRequest request1 = new GameRequest();
            request1.UserToken = u1.Data.ToString();
            request1.TimeLimit = 50;

            Response r1 = client.DoPostAsync("games", request1).Result;
            Assert.AreEqual(Accepted, r1.Status);

            GameRequest request2 = new GameRequest();
            request2.UserToken = u2.Data.ToString();
            request2.TimeLimit = 50;

            Response r2 = client.DoPostAsync("games", request2).Result;
            Assert.AreEqual(Created, r2.Status);

            //play word

            WordAttempt attempt = new WordAttempt();
            attempt.UserToken = u1.Data.ToString();
            attempt.Word = "hello";

            Response r3 = client.DoPutAsync(attempt, "games/" + r2.Data.ToString()).Result;
            Assert.AreEqual(OK, r3.Status);

            //edge cases
            WordAttempt attempt2 = new WordAttempt();
            attempt2.UserToken = "e";
            attempt2.Word = "dsf";

            Response r4 = client.DoPutAsync(attempt2, "games/" + r2.Data.ToString()).Result;
            Assert.AreEqual(Forbidden, r4.Status);
            
        }

        [TestMethod]
        public void GameState()
        {
            Username user1 = new Username();
            user1.Nickname = "Joe";
            Response u1 = client.DoPostAsync("users", user1).Result;

            Username user2 = new Username();
            user2.Nickname = "Joe";
            Response u2 = client.DoPostAsync("users", user2).Result;

            Username user3 = new Username();
            user3.Nickname = "Joe";
            Response u3 = client.DoPostAsync("users", user3).Result;

            //create game
            GameRequest request1 = new GameRequest();
            request1.UserToken = u1.Data.ToString();
            request1.TimeLimit = 5;

            Response r1 = client.DoPostAsync("games", request1).Result;
            Assert.AreEqual(Accepted, r1.Status);

            GameRequest request2 = new GameRequest();
            request2.UserToken = u2.Data.ToString();
            request2.TimeLimit = 5;

            Response r2 = client.DoPostAsync("games", request2).Result;
            Assert.AreEqual(Created, r2.Status);

            //test different game states
            Response r3 = client.DoGetAsync("games/" + r2.Data.ToString()).Result;
            Assert.AreEqual(OK, r3.Status);

            Response r4 = client.DoGetAsync("games/" + r2.Data.ToString() + "?Brief=yes").Result;
            Assert.AreEqual(OK, r4.Status);

            Response r5 = client.DoGetAsync("games/" + "123" + "?Brief=yes").Result;
            Assert.AreEqual(Forbidden, r5.Status);

            Thread.Sleep(5000);

            Response r6 = client.DoGetAsync("games/" + r2.Data.ToString() + "?Brief=yes").Result;
            Assert.AreEqual(OK, r6.Status);

            Response r7 = client.DoGetAsync("games/" + r2.Data.ToString()).Result;
            Assert.AreEqual(OK, r7.Status);


        }
    }
}
