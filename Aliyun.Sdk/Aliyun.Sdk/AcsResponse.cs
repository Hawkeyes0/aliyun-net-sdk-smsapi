using System;
using System.Collections.Generic;
using System.Text;
using Aliyuncs.Transform;

namespace Aliyuncs
{
    public abstract class AcsResponse
    {
        public abstract AcsResponse GetInstance(UnmarshallerContext context);
    }
}
