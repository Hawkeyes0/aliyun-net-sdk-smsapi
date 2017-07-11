using System;
using System.Security.Cryptography;
using System.Text;

namespace Aliyuncs.Auth
{
    internal class ShaHmac1 : ISigner
    {
        private const string AGLORITHM_NAME = "HmacSHA1";
        public string GetSignerName()
        {
            return "HMAC-SHA1";
        }

        public string GetSignerVersion()
        {
            return "1.0";
        }

        public string SignString(string source, string accessSecret)
        {
            Encoding enc = Encoding.GetEncoding(AcsURLEncoder.URL_ENCODING);
            byte[] key = enc.GetBytes(accessSecret);
            HMACSHA1 mac = new HMACSHA1(key);
            byte[] signData = mac.ComputeHash(enc.GetBytes(source));
            return Convert.ToBase64String(signData);
        }
    }
}