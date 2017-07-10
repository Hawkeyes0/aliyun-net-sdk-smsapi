using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aliyuncs.Auth
{
    public class RpcSignatureComposer : ISignatureComposer
    {
        public static ISignatureComposer Composer { get; internal set; }
    }
}
