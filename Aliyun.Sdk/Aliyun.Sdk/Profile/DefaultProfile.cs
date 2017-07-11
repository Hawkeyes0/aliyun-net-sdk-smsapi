using Aliyuncs.Auth;
using Aliyuncs.Regions;
using System;
using System.Collections.Generic;
using System.Text;
using Aliyuncs.Http;

namespace Aliyuncs.Profile
{
    public class DefaultProfile : IClientProfile
    {
        public static DefaultProfile Profile { get; private set; } = new DefaultProfile();
        public ISigner Signer { get; set; } = ShaHmac1Singleton.INSTANCE;
        public FormatType Format { get; set; }
        public string RegionId { get; set; }
        public Credential Credential { get; set; }

        private static List<Endpoint> _endpoints = new List<Endpoint>();

        private IEndpointsProvider iendpoints = null;
        private IEndpointsProvider remoteProvider = null;
        private ICredentialProvider icredential = null;
        private LocationConfig locationConfig = new LocationConfig();

        private DefaultProfile()
        {
            locationConfig = new LocationConfig();
            iendpoints = new InternalEndpointsParser();
            remoteProvider = RemoteEndpointsParser.InitRemoteEndpointsParser();
        }

        private DefaultProfile(string region, Credential creden)
        {
            iendpoints = new InternalEndpointsParser();
            remoteProvider = RemoteEndpointsParser.InitRemoteEndpointsParser();
            Credential = creden;
            RegionId = region;
            locationConfig = new LocationConfig();
        }

        public static DefaultProfile GetProfile(string regionId, string accessKeyId, string secret)
        {
            Credential creden = new Credential(accessKeyId, secret);
            Profile = new DefaultProfile(regionId, creden);
            return Profile;
        }

        public static void AddEndpoint(string endpointName, string regionId, string product, string domain)
        {
            if (null == _endpoints)
            {
                _endpoints = Profile.GetEndpoints(regionId, product);
            }
            Endpoint endpoint = FindEndpointByRegionId(regionId);
            if (null == endpoint)
            {
                AddEndpoint_(endpointName, regionId, product, domain);
            }
            else
            {
                UpdateEndpoint(regionId, product, domain, endpoint);
            }
        }

        private static void UpdateEndpoint(string regionId, string product, string domain, Endpoint endpoint)
        {
            HashSet<string> regionIds = endpoint.RegionIds;
            regionIds.Add(regionId);

            List<ProductDomain> productDomains = endpoint.ProductDomains;
            ProductDomain productDomain = FindProductDomain(productDomains, product);
            if (null == productDomain)
            {
                ProductDomain newProductDomain = new ProductDomain(product, domain);
                productDomains.Add(newProductDomain);
            }
            else
            {
                productDomain.DomianName = domain;
            }
        }

        private static ProductDomain FindProductDomain(List<ProductDomain> productDomains, string product)
        {
            foreach (ProductDomain productDomain in productDomains)
            {
                if (productDomain.ProductName == product)
                {
                    return productDomain;
                }
            }
            return null;
        }

        private static void AddEndpoint_(string endpointName, string regionId, string product, string domain)
        {
            HashSet<string> regions = new HashSet<string>
            {
                regionId
            };
            List<ProductDomain> productDomains = new List<ProductDomain>
            {
                new ProductDomain(product, domain)
            };
            Endpoint endpoint = new Endpoint(endpointName, regions, productDomains);
            _endpoints.Add(endpoint);
        }

        private static Endpoint FindEndpointByRegionId(string regionId)
        {
            if (_endpoints.Count == 0)
            {
                return null;
            }
            foreach (Endpoint endpoint in _endpoints)
            {
                if (endpoint.RegionIds.Contains(regionId))
                {
                    return endpoint;
                }
            }
            return null;
        }

        public List<Endpoint> GetEndpoints(string product, string regionId, string locationProduct, string endpointType)
        {
            if (_endpoints.Count == 0)
            {
                Endpoint endpoint = iendpoints.GetEndpoint(regionId, product);
                if (endpoint != null)
                {
                    _endpoints.Add(endpoint);
                }
            }

            return _endpoints;
        }

        public List<Endpoint> GetEndpoints(string regionId, string product)
        {
            throw new NotImplementedException();
        }
    }
}
