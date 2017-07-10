using Aliyuncs.Auth;
using Aliyuncs.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aliyuncs
{
    public abstract class RpcAcsRequest<T> : AcsRequest<T> where T : AcsResponse
    {
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
    }
}
