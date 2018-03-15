using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoggleClient
{
    interface IBoggleView
    {
        /// <summary>
        /// fires if a user tries to register for some boggle
        /// </summary>
        event Action<string, string> RegisterUser;

        /// <summary>
        /// fires when a user trys to join a game
        /// </summary>
        event Action<string> PlayGame;

        /// <summary>
        /// Fires when the registration must be canceled.
        /// </summary>
        event Action CancelPressed;

        /// <summary>
        /// Fires when a user cancels a request to join a game
        /// </summary>
        event Action CancelGameRequestPressed;

        event Action<string> EnterButtonPressed;

        event Action LeavePressed;

        /// <summary>
        /// If state == true, enables all controls that are normally enabled; disables Cancel.
        /// If state == false, disables all controls; enables Cancel.
        /// </summary>
        void EnableControls(bool state);

        /// <summary>
        /// gets the word that the user entered
        /// </summary>
        /// <param name="word"></param>
        void newWord(string word);

        /// <summary>
        /// Is the user currently registered?
        /// </summary>
        bool IsUserRegistered { get; set; }

        /// <summary>
        /// Is the user currently in game?
        /// </summary>
        bool IsUserInGame { get; set; }

        /// <summary>
        /// Is the user currently looking for a playing partner
        /// </summary>
        bool IsUserPending { get; set; }

        /// <summary>
        /// gets the board from the server
        /// </summary>
        /// <param name="board"></param>
        void startBoard(string board);

        /// <summary>
        /// gets the time and scores from the server
        /// </summary>
        /// <param name="time"></param>
        /// <param name="scoreOne"></param>
        /// <param name="scoreTwo"></param>
        void updateData(string time, string scoreOne, string scoreTwo);

        /// <summary>
        /// allows the controller to update player one's final word list
        /// </summary>
        /// <param name="word"></param>
        void updateEndOne(string word);

        /// <summary>
        /// allows the controller to update player one's final word list
        /// </summary>
        /// <param name="word"></param>
        void updateEndTwo(string word);
    }
}
