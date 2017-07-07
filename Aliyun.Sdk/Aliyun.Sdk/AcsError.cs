using System;
using System.Collections.Generic;
using System.Text;
using Aliyuncs.Transform;

namespace Aliyuncs
{
    public class AcsError : AcsResponse
    {
        public int StatusCode { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public string RequestId { get; set; }

        public override AcsResponse GetInstance(UnmarshallerContext context)
        {
            return AcsErrorUnmarshaller.Unmarshall(this, context);
        }
    }
}
