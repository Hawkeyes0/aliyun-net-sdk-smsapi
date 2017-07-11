using Aliyuncs.Http;
using System;
using System.Collections.Generic;
using System.Text;
using Aliyuncs.Auth;
using Aliyuncs.Regions;

namespace Aliyuncs
{
    public abstract class AcsRequest<T> : HttpRequest where T : AcsResponse
    {
        private string _locationProduct;
        private string _endpointType;
        private string _securityToken;

        public string RegionId { get; internal set; }
        public string Product { get; internal set; }
        public string LocationProduct
        {
            get { return _locationProduct; }
            internal set
            {
                _locationProduct = value;
                PutQueryParameter("ServiceCode", _locationProduct);
            }
        }
        public string EndpointType
        {
            get { return _endpointType; }
            internal set
            {
                _endpointType = value;
                PutQueryParameter("Type", value);
            }
        }
        public FormatType AcceptFormat { get; internal set; }
        public string Version { get; internal set; }
        public string ActionName { get; internal set; }
        public string SecurityToken
        {
            get { return _securityToken; }
            internal set
            {
                _securityToken = value;
                PutQueryParameter(nameof(SecurityToken), value);
            }
        }
        protected ISignatureComposer Composer { get; set; }
        public ProtocolType Protocol = ProtocolType.HTTP;
        public Dictionary<string, string> QueryParameters { get; private set; } = new Dictionary<string, string>();
        public Dictionary<string, string> DomainParameters { get; private set; } = new Dictionary<string, string>();

        public AcsRequest(String product) : base(null)
        {
            Headers["x-sdk-client"] = "Java/2.0.0";
            Product = product;
        }

        public AcsRequest(String product, String version) : base(null)
        {
            Product = product;
            Version = version;
        }

        public abstract HttpRequest SignRequest(ISigner signer, Credential credential, FormatType format, ProductDomain domain);

        public abstract Type GetResponseClass();

        protected void PutQueryParameter(String name, String value)
        {
            QueryParameters[name] = value;
        }

        public static string ConcatQueryString(Dictionary<String, String> parameters)
        {
            if (null == parameters)
                return null;

            StringBuilder urlBuilder = new StringBuilder("");
            foreach (var entry in parameters)
            {
                string key = entry.Key;
                string val = entry.Value;
                urlBuilder.Append(AcsURLEncoder.Encode(key));
                if (val != null)
                {
                    urlBuilder.Append("=").Append(AcsURLEncoder.Encode(val));
                }
                urlBuilder.Append("&");
            }

            int strIndex = urlBuilder.Length;
            if (parameters.Count > 0)
                urlBuilder.Remove(strIndex - 1, 1);

            return urlBuilder.ToString();
        }

    }
}
