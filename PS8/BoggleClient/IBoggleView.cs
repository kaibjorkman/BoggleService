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
        event Action<string> RegisterUser;

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

        /// <summary>
        /// If state == true, enables all controls that are normally enabled; disables Cancel.
        /// If state == false, disables all controls; enables Cancel.
        /// </summary>
        void EnableControls(bool state);

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

        void startBoard(string board);

        void updateData(string time, string scoreOne, string scoreTwo);
    }
}
