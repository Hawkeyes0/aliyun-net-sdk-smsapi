using System;
using System.Collections.Generic;
using System.Text;

namespace Aliyuncs.Auth
{
    public interface ISigner
    {
        string SignString(string source, string accessSecret);
        string GetSignerName();
        string GetSignerVersion();
    }
}
