using System;
using System.Collections.Generic;
using System.Text;
using Aliyuncs.Auth;
using Aliyuncs.Http;
using Aliyuncs.Regions;

namespace Aliyuncs.Profile
{
    public interface IClientProfile
    {
        ISigner Signer { get; set; }
        FormatType Format { get; set; }
        string RegionId { get; set; }
        Credential Credential { get; set; }

        List<Endpoint> GetEndpoints(string product, string regionId, string locationProduct, string endpointType);
        List<Endpoint> GetEndpoints(string regionId, string product);
    }
}
