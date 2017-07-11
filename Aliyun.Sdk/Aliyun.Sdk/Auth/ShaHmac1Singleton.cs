using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aliyuncs.Auth
{
    public class ShaHmac1Singleton
    {
        public static ISigner INSTANCE { get; internal set; } = new ShaHmac1();
    }
}
