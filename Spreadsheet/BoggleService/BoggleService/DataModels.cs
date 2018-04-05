using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Boggle
{
    public class DataModels
    {
        public class Username
        {
            public string Nickname { get; set; }
        }

        public class UserResponse
        {
            public string UserToken { get; set; }
        }

        public class GameRequest
        {
            public string UserToken { get; set; }
            public int TimeLimit { get; set; }
        }

        public class CancelGameRequest
        {
            public string UserToken { get; set; }
        }

        public class PendingGame
        {
            public string UserToken { get; set; }

            public int GameID { get; set; }
        }

        public class JoinGameResponse
        {
            public int GameID { get; set; }
        }

        [DataContract]
        public class Game
        {
            [DataMember]
            public string GameState { get; set; }

            [DataMember(EmitDefaultValue = false)]
            public string Board { get; set; }

            [DataMember(EmitDefaultValue = false)]
            public BoggleBoard BoggleBoard { get; set; }

            [DataMember(EmitDefaultValue = false)]
            public int TimeLimit { get; set; }

            [DataMember(EmitDefaultValue = false)]
            public int? TimeLeft { get; set; }

            [DataMember(EmitDefaultValue = false)]
            public int StartTime { get; set; }

            [DataMember(EmitDefaultValue = false)]
            public Player1 Player1 { get; set; }

            [DataMember(EmitDefaultValue = false)]
            public Player2 Player2 { get; set; }

        }

        [DataContract]
        public class Player1
        {
            [DataMember(EmitDefaultValue = false)]
            public string Nickname { get; set; }

            [DataMember(EmitDefaultValue = false)]
            public string UserToken { get; set; }

            [DataMember]
            public int Score { get; set; }

            [DataMember(EmitDefaultValue = false)]
            public List<WordPlayed> WordsPlayed { get; set; }
            
        }

        [DataContract]
        public class Player2
        {
            [DataMember(EmitDefaultValue = false)]
            public string Nickname { get; set; }

            [DataMember(EmitDefaultValue = false)]
            public string UserToken { get; set; }

            [DataMember]
            public int Score { get; set; }

            [DataMember(EmitDefaultValue = false)]
            public List<WordPlayed> WordsPlayed { get; set; }

        }


       

        public class WordPlayed
        {
            public string Word { get; set; }

            public int Score { get; set; }

        }

        public class PlayWordResponse
        {
            public int Score { get; set; }
        }

        public class WordAttempt
        {
            public string UserToken { get; set; }
            public string Word { get; set; }
        }
    }
}