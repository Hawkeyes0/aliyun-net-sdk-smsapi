using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aliyuncs.Regions
{
    public interface IEndpointsProvider
    {
        Endpoint GetEndpoint(string regionId, string product);
    }
}
