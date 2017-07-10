using System;
using Aliyuncs;
using Aliyuncs.Transform;
using Aliyun.SmsApi.Transform;

namespace Aliyun.SmsApi.Model
{
    public class SendSmsResponse : AcsResponse
    {
        public string RequestId { get; set; }
        public string BizId { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }

        public override AcsResponse GetInstance(UnmarshallerContext context)
        {
            return SendSmsResponseUnmarshaller.Unmarshall(this, context);
        }
    }
}