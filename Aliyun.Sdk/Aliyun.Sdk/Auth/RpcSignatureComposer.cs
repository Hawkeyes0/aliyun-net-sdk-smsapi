using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aliyuncs.Http;
using Aliyuncs.Utils;

namespace Aliyuncs.Auth
{
    public class RpcSignatureComposer : ISignatureComposer
    {
        private const char SEPARATOR = '&';

        public static ISignatureComposer Composer { get; } = new RpcSignatureComposer();

        public string ComposeStringToSign(MethodType method, string uriPattern, ISigner signer, Dictionary<string, string> queries, Dictionary<string,string> headers, Dictionary<string,string> paths)
        {
            string[] sortedKeys = queries.Keys.ToArray();
            Array.Sort(sortedKeys, new AsciiComparer());
            StringBuilder canonicalizedQuerystring = new StringBuilder();
            try
            {
                foreach (string key in sortedKeys)
                {
                    canonicalizedQuerystring.Append("&")
                    .Append(AcsURLEncoder.PercentEncode(key)).Append("=")
                    .Append(AcsURLEncoder.PercentEncode(queries[key]));
                }

                StringBuilder stringToSign = new StringBuilder();
                stringToSign.Append(method.ToString());
                stringToSign.Append(SEPARATOR);
                stringToSign.Append(AcsURLEncoder.PercentEncode("/"));
                stringToSign.Append(SEPARATOR);
                stringToSign.Append(AcsURLEncoder.PercentEncode(
                        canonicalizedQuerystring.ToString().Substring(1)));

                return stringToSign.ToString();
            }
            catch
            {
                throw new Exception("UTF-8 encoding is not supported.");
            }
        }

        public Dictionary<string, string> RefreshSignParameters(Dictionary<string, string> parameters, ISigner signer, string accessKeyId, FormatType format)
        {
            Dictionary<string, string> immutableMap = new Dictionary<string, string>(parameters);
            immutableMap["Timestamp"] = ParameterHelper.GetISO8601Time(null);
            immutableMap["SignatureMethod"] = signer.GetSignerName();
            immutableMap["SignatureVersion"] = signer.GetSignerVersion();
            immutableMap["SignatureNonce"] = ParameterHelper.GetUniqueNonce();
            immutableMap["AccessKeyId"] = accessKeyId;
            if (FormatType.UNKNOWN != format)
                immutableMap["Format"] = format.ToString();
            return immutableMap;
        }
    }
}
