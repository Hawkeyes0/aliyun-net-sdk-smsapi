using Aliyuncs.Auth;
using Aliyuncs.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aliyuncs.Regions;

namespace Aliyuncs
{
    public abstract class RpcAcsRequest<T> : AcsRequest<T> where T : AcsResponse
    {
        public new string Version
        {
            get { return base.Version; }
            set
            {
                base.Version = value;
                PutQueryParameter(nameof(Version), value);
            }
        }

        public new string ActionName
        {
            get { return base.ActionName; }
            set
            {
                base.ActionName = value;
                PutQueryParameter("Action", value);
            }
        }

        public new string SecurityToken
        {
            get { return base.SecurityToken; }
            set
            {
                base.SecurityToken = value;
                PutQueryParameter(nameof(SecurityToken), value);
            }
        }

        public new FormatType AcceptFormat
        {
            get { return base.AcceptFormat; }
            set
            {
                base.AcceptFormat = value;
                PutQueryParameter("Format", value.ToString());
            }
        }

        public RpcAcsRequest(String product) : base(product)
        {
            Initialize();
        }

        public RpcAcsRequest(String product, String version) : base(product)
        {
            Version = version;
            Initialize();
        }

        public RpcAcsRequest(String product, String version, String action) : base(product)
        {
            Version = version;
            ActionName = action;
            Initialize();
        }

        public RpcAcsRequest(String product, String version, String action, String locationProduct) : base(product)
        {
            Version = version;
            ActionName = action;
            LocationProduct = locationProduct;
            Initialize();
        }

        public RpcAcsRequest(String product, String version, String action, String locationProduct, String endpointType) : base(product)
        {
            Version = version;
            ActionName = action;
            LocationProduct = locationProduct;
            EndpointType = endpointType;
            Initialize();
        }
        private void Initialize()
        {
            Method = MethodType.GET;
            AcceptFormat = FormatType.XML;
            Composer = RpcSignatureComposer.Composer;
        }

        public override HttpRequest SignRequest(ISigner signer, Credential credential, FormatType format, ProductDomain domain)
        {
            Dictionary<String, String> imutableMap = new Dictionary<String, String>(QueryParameters);
            if (null != signer && null != credential)
            {
                String accessKeyId = credential.AccessKeyId;
                String accessSecret = credential.AccessSecret;
                imutableMap = Composer.RefreshSignParameters(QueryParameters, signer, accessKeyId, format);
                imutableMap["RegionId"] = RegionId;
                String strToSign = Composer.ComposeStringToSign(Method, null, signer, imutableMap, null, null);
                String signature = signer.SignString(strToSign, accessSecret + "&");
                imutableMap["Signature"] = signature;
            }
            Url = ComposeUrl(domain.DomianName, imutableMap);
            return this;
        }

        private string ComposeUrl(string endpoint, Dictionary<string, string> queries)
        {
            Dictionary<string, string> mapQueries = (queries == null) ? QueryParameters : queries;
            StringBuilder urlBuilder = new StringBuilder("");
            bool hasQuery = false;
            urlBuilder.Append(Protocol);
            urlBuilder.Append("://").Append(endpoint);
            if (!hasQuery)
            {
                urlBuilder.Append("/?");
                hasQuery = true;
            }
            string query = ConcatQueryString(mapQueries);
            return urlBuilder.Append(query).ToString();
        }
    }
}
