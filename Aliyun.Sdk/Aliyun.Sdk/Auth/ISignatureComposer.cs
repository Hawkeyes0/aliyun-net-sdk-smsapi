using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aliyuncs.Http;

namespace Aliyuncs.Auth
{
    public interface ISignatureComposer
    {
        string ComposeStringToSign(MethodType method, string uriPattern, ISigner signer, Dictionary<string, string> queries, Dictionary<string, string> headers, Dictionary<string, string> paths);
        Dictionary<string, string> RefreshSignParameters(Dictionary<string, string> queryParameters, ISigner signer, string accessKeyId, FormatType format);
    }
}
