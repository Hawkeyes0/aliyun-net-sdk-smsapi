using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aliyuncs.Transform;

namespace Aliyuncs
{
    public class AcsErrorUnmarshaller
    {
        internal static AcsResponse Unmarshall(AcsError error, UnmarshallerContext context)
        {
            Dictionary<string, string> map = context.ResponseMap;
            error.StatusCode = context.HttpStatus;
            error.RequestId = map["Error.RequestId"];
            error.ErrorCode = map["Error.Code"];
            error.ErrorMessage = map["Error.Message"];

            return error;
        }
    }
}
