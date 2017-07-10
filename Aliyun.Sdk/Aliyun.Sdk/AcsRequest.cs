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
        public string RegionId { get; internal set; }
        public string Product { get; internal set; }
        public string LocationProduct { get; internal set; }
        public string EndpointType { get; internal set; }
        public FormatType AcceptFormat { get; internal set; }
        public string Version { get; internal set; }
        public string ActionName { get; internal set; }
        public string SecurityToken { get; internal set; }
        protected ISignatureComposer Composer { get; set; }
        public ProtocolType protocol = ProtocolType.HTTP;
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

        internal HttpRequest SignRequest(ISigner signer, Credential credential, FormatType format, ProductDomain domain)
        {
            throw new NotImplementedException();
        }

        public abstract Type GetResponseClass();

        protected void PutQueryParameter(String name, String value)
        {
            QueryParameters[name] = value;
        }
    }
}
