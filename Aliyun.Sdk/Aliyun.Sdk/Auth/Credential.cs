using System;
using System.Collections.Generic;
using System.Text;

namespace Aliyuncs.Auth
{
    public class Credential
    {
        public string AccessKeyId { get; }
        public string AccessSecret { get; }
        public DateTime RefreshDate { get; }

        public Credential(string keyId, string secret)
        {
            AccessKeyId = keyId;
            AccessSecret = secret;
            RefreshDate = DateTime.Now;
        }
    }
}
