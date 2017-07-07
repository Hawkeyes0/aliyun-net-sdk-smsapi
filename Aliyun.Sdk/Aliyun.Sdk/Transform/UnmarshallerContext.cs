using System;
using System.Collections.Generic;
using System.Text;
using Aliyuncs.Http;

namespace Aliyuncs.Transform
{
    public class UnmarshallerContext
    {
        public HttpResponse HttpResponse { get; internal set; }
        public Dictionary<string, string> ResponseMap { get; internal set; }
        public int HttpStatus { get; internal set; }

        internal Dictionary<string, string> getResponseMap()
        {
            throw new NotImplementedException();
        }
    }
}
