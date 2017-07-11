using System;
using System.Collections.Generic;
using System.Text;

namespace Aliyuncs.Reader
{
    public interface IReader
    {
        Dictionary<string,string> Read(string response, string endpoint);
    }
}
