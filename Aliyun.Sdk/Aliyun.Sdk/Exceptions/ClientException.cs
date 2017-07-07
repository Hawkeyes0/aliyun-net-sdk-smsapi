using System;
using System.Collections.Generic;
using System.Text;

namespace Aliyuncs.Exceptions
{
    public class ClientException : Exception
    {
        private string v;
        private string errorCode;
        private string errorMessage;
        private string requestId;

        public ClientException(string message, string v) : base(message)
        {
            this.v = v;
        }

        public ClientException(string errorCode, string errorMessage, string requestId)
        {
            this.errorCode = errorCode;
            this.errorMessage = errorMessage;
            this.requestId = requestId;
        }
    }
}
