using System.Collections.Generic;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Web;
using static Boggle.DataModels;

namespace Boggle
{
    [ServiceContract]
    public interface IBoggleService
    {
        /// <summary>
        /// Sends back index.html as the response body.
        /// </summary>
        [WebGet(UriTemplate = "/api")]
        Stream API();

        /// <summary>
        /// Returns the nth word from dictionary.txt.  If there is
        /// no nth word, responds with code 403. This is a demo;
        /// you can delete it.
        /// </summary>
        [WebGet(UriTemplate = "/word?index={n}")]
        string WordAtIndex(int n);

        /// <summary>
        /// Registers a new user.
        /// If nickname or is empty after trimming, responds with status code Forbidden.
        /// Otherwise, creates a user, returns the user's token, and responds with status code Created. 
        /// </summary>
        [WebInvoke(Method = "POST", UriTemplate = "/users")]
        UserResponse Register(Username name);

        /// <summary>
        /// if the player is in a queue it will cancel the queue and send back 200 OK, else will send back 403 Forbidden
        /// </summary>
        [WebInvoke(Method = "PUT", UriTemplate = "/games")]
        void CancelQueue(CancelGameRequest userID);

        /// <summary>
        /// IF the user token is invalid or the time limit is below 5 or above 120 reponds with 403(Forbidden)
        /// Otherwise if the usertoken is already in use it responds with 409
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [WebInvoke(Method = "POST", UriTemplate = "/games")]
        JoinGameResponse JoinGame(GameRequest request);
           
        /// <summary>
        /// gets a word from the user and checks to see if it is part of the boggle board and returns the score
        /// </summary>
        [WebInvoke(Method = "PUT", UriTemplate = "/games/{GameID}")]
        PlayWordResponse PlayWord(WordAttempt attempt, string GameID);

       /// <summary>
       /// If GameID is invalid, responds with status 403 (Forbidden).
       /// Otherwise, returns information about the game named by GameID as 
       /// illustrated below.Note that the information returned depends on whether "Brief=yes" was 
       /// included as a parameter as well as on the state of the game. Responds with status code 200 (OK). 
       /// Note: The Board and Words are not case sensitive.
       /// </summary>
        [WebGet(UriTemplate = "/games/{GameID}?Brief={brief}")]
        Game GetStatus(string GameID, string brief);

    }
}
