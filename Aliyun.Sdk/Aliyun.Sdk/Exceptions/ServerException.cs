using System;
using System.Collections.Generic;
using System.Text;

namespace Aliyuncs.Exceptions
{
    public class ServerException : Exception
    {
        private string errorMessage;
        private string requestId;

        public ServerException(string message, string errorMessage, string requestId) : base(message)
        {
            this.errorMessage = errorMessage;
            this.requestId = requestId;
        }
    }
}
