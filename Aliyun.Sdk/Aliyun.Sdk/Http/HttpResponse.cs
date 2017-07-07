using System;
using System.Collections.Generic;
using System.Text;

namespace Aliyuncs.Http
{
    public class HttpResponse
    {
        public int Status { get; internal set; }
        public bool IsSuccess => 200 <= Status && 300 > Status;

        public FormatType ContentType { get; internal set; }
        public byte[] Content { get; internal set; }
        public Encoding Encoding { get; internal set; }

        internal static HttpResponse GetResponse(HttpRequest httpRequest)
        {
            throw new NotImplementedException();
        }
    }
}
