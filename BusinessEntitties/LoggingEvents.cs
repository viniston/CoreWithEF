using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessEntitties
{
    public class LoggingEvents
    {
        /* Range 1000 - 1999 are informational */
        public const int GetAll = 1000;
        public const int Get = 1001;
        public const int Create = 1002;
        public const int Update = 1003;
        public const int Delete = 1004;
        public const int Start = 1005;
        public const int End = 1006;
        public const int ItemCount = 1007;
        public const int Run = 1008;
        public const int ActionStarted = 1009;
        public const int ActionCompletedNormally = 1010;
        public const int ActionCancelled = 1011;
        public const int ActionStopped = 1012;
        public const int ActionPaused = 1013;
        public const int Successful = 1014;

        /* Range 2000 - 2999 are warnings */
        public const int Unsuccessful = 2000;

        //public const int FileNotCreated = 2002;
        public const int Empty = 2003;

        /* Range 3000 - 3999 are Errors */
        public const int UnknownExceptionOccurred = 3000;
        public const int UnparsableRecordFound = 3004;
    }
}
