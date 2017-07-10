using System;
using System.Collections.Generic;
using System.Text;

namespace Aliyuncs.Regions
{
    public class Endpoint
    {
        private string endpointName;
        private HashSet<string> regions;
        private List<ProductDomain> productDomains;

        public Endpoint(string endpointName, HashSet<string> regions, List<ProductDomain> productDomains)
        {
            this.endpointName = endpointName;
            this.regions = regions;
            this.productDomains = productDomains;
        }

        public HashSet<string> RegionIds { get; internal set; }
        public List<ProductDomain> ProductDomains { get; internal set; }

        internal static ProductDomain FindProductDomain(string regionId, string product, List<Endpoint> endpoints)
        {
            throw new NotImplementedException();
        }
    }
}
