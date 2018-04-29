using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Services.Constants
{
    public static class LoggingEvents
    {
        /// <summary>
        /// Logging events regarding Authorization (Codes from 1000 to 1999)
        /// </summary>
        public static class AuthorizationEvents
        {
            public const int RegisterError = 1001;
            public const int LoginError = 1002;
            public const int JWTGenerationError = 1003;
            public const int LogoffError = 1004;
        }

        /// <summary>
        /// Logging events regarding Generic Service operations (Codes from 2000 to 2099)
        /// </summary>
        public static class GenericServiceEvents
        {
            public const int LookupError = 2001;
        }

        /// <summary>
        /// Logging events regarding Custom Service operations (Codes from 2100 to 2999) <br>
        /// UserService events (Codes from 2100 - 2199) <br>
        /// LobbyService events (Codes from 2200 - 2299) <br>
        /// LobbyJoinRequestService (Codes from 2300 - 2399) <br>
        /// </summary>
        public static class CustomServiceEvents
        {
            public const int UserLookupError = 2101;
            public const int LobbyCreateError = 2201;
            public const int LobbyCreateInformation = 2202;
            public const int LobbyUpdateError = 2203;
            public const int LobbyUpdateInformation = 2204;
            public const int LobbyUpdateEntryNotFound = 2205;
            public const int LobbyDeleteEntryNotFound = 2206;
            public const int LobbyDeleteInformation = 2206;
            public const int LobbyDeleteError = 2207;
            public const int LobbyJoinRequestCreateError = 2301;
            public const int LobbyJoinRequestCreateInformation = 2202;
            public const int LobbyJoinRequestUpdateError = 2203;
            public const int LobbyJoinRequestUpdateInformation = 2204;
            public const int LobbyJoinRequestUpdateEntryNotFound = 2205;
            public const int LobbyJoinRequestDeleteEntryNotFound = 2206;
            public const int LobbyJoinRequestDeleteInformation = 2206;
            public const int LobbyJoinRequestDeleteError = 2207;
        }
    }
}
