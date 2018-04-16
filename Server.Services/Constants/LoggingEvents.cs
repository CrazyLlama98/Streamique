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
    }
}
