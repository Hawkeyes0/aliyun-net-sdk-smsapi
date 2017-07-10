using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aliyuncs.Regions
{
    public sealed class LocationConfig
    {
        public const string LOCATION_INNER_ENDPOINT = "location.aliyuncs.com";
        public const string LOCATION_INNER_PRODUCT = "Location";

        public string RegionId { get; } = "cn-hangzhou";
        public string Product { get; } = "Location";
        public string Endpoint { get; } = "location.aliyuncs.com";
    }
}
