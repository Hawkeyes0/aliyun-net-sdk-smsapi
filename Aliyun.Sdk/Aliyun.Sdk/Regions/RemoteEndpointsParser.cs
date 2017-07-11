using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aliyuncs.Regions
{
    public class RemoteEndpointsParser : IEndpointsProvider
    {
        public DescribeEndpointServiceImpl DescribeEndpointService { get; private set; }

        internal static IEndpointsProvider InitRemoteEndpointsParser()
        {
            RemoteEndpointsParser parser = new RemoteEndpointsParser();
            parser.DescribeEndpointService = new DescribeEndpointServiceImpl();
            return parser;
        }

        public Endpoint GetEndpoint(string regionId, string product)
        {
            throw new NotImplementedException();
        }
    }
}
