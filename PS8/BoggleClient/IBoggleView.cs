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
        /// If state == true, enables all controls that are normally enabled; disables Cancel.
        /// If state == false, disables all controls; enables Cancel.
        /// </summary>
        void EnableControls(bool state);

        /// <summary>
        /// Is the user currently registered?
        /// </summary>
        bool IsUserRegistered { get; set; }
    }
}
