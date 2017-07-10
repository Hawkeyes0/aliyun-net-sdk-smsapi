using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aliyun.SmsApi.Model;
using Aliyuncs;
using Aliyuncs.Transform;

namespace Aliyun.SmsApi.Transform
{
    public class SendSmsResponseUnmarshaller
    {
        internal static AcsResponse Unmarshall(SendSmsResponse sendSmsResponse, UnmarshallerContext context)
        {
            sendSmsResponse.RequestId = context.ResponseMap["SendSmsResponse.RequestId"];
            sendSmsResponse.BizId = context.ResponseMap["SendSmsResponse.BizId"];
            sendSmsResponse.Code = context.ResponseMap["SendSmsResponse.Code"];
            sendSmsResponse.Message = context.ResponseMap["SendSmsResponse.Message"];

            return sendSmsResponse;
        }
    }
}
