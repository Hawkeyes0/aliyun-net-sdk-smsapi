using System;
using System.Collections.Generic;
using System.Text;

namespace Aliyuncs.Regions
{
    public class Endpoint
    {
        public Endpoint(string endpointName, HashSet<string> regions, List<ProductDomain> productDomains)
        {
            Name = endpointName;
            RegionIds = regions;
            ProductDomains = productDomains;
        }

        public HashSet<string> RegionIds { get; internal set; }
        public List<ProductDomain> ProductDomains { get; internal set; }
        public string Name { get; internal set; }

        internal static ProductDomain FindProductDomain(string regionId, string product, List<Endpoint> endpoints)
        {
            if (null == regionId || null == product || null == endpoints)
            {
                return null;
            }
            foreach (Endpoint endpoint in endpoints)
            {
                if (endpoint.RegionIds.Contains(regionId))
                {
                    ProductDomain domain = FindProductDomainByProduct(endpoint.ProductDomains, product);
                    return domain;
                }
            }
            return null;
        }

        private static ProductDomain FindProductDomainByProduct(List<ProductDomain> productDomains, string product)
        {
            if (null == productDomains)
            {
                return null;
            }
            foreach (ProductDomain productDomain in productDomains)
            {
                if (product.Equals(productDomain.ProductName, StringComparison.CurrentCultureIgnoreCase))
                {
                    return productDomain;
                }
            }
            return null;
        }
    }
}
