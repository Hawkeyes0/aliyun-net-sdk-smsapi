using Aliyuncs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aliyuncs.Auth;
using Aliyuncs.Http;
using Aliyuncs.Regions;

namespace Aliyun.SmsApi.Model
{
    public class SendSmsRequest : RpcAcsRequest<SendSmsResponse>
    {
        private string outId;

        private string signName;

        private long? ownerId;

        private long? resourceOwnerId;

        private string templateCode;

        private string phoneNumbers;

        private string resourceOwnerAccount;

        private string templateParam;

        public string OutId
        {
            get { return outId; }
            set
            {
                outId = value;
                PutQueryParameter(nameof(OutId), value);
            }
        }

        public string SignName
        {
            get { return signName; }
            set
            {
                signName = value;
                PutQueryParameter(nameof(SignName), value);
            }
        }

        public long? OwnerId
        {
            get { return ownerId; }
            set
            {
                ownerId = value;
                PutQueryParameter(nameof(OwnerId), value.ToString());
            }
        }

        public long? ResourceOwnerId
        {
            get { return resourceOwnerId; }
            set
            {
                resourceOwnerId = value;
                PutQueryParameter(nameof(ResourceOwnerId), value.ToString());
            }
        }

        public string TemplateCode
        {
            get { return templateCode; }
            set
            {
                templateCode = value;
                PutQueryParameter(nameof(TemplateCode), value);
            }
        }

        public string PhoneNumbers
        {
            get { return phoneNumbers; }
            set
            {
                phoneNumbers = value;
                PutQueryParameter(nameof(PhoneNumbers), value);
            }
        }

        public string ResourceOwnerAccount
        {
            get { return resourceOwnerAccount; }
            set
            {
                resourceOwnerAccount = value;
                PutQueryParameter(nameof(ResourceOwnerAccount), value);
            }
        }

        public string TemplateParam
        {
            get { return templateParam; }
            set
            {
                templateParam = value;
                PutQueryParameter(nameof(TemplateParam), value);
            }
        }

        public SendSmsRequest() : base("Dysmsapi", "2017-05-25", "SendSms")
        { }

        public override Type GetResponseClass()
        {
            return typeof(SendSmsResponse);
        }
    }
}
