using Aliyuncs.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aliyuncs.Utils
{
    public static class FormatTypeHelper
    {
        public static string MapFromatToAccept(FormatType format)
        {
            switch (format)
            {
                case FormatType.XML:
                    return "application/xml";
                case FormatType.JSON:
                    return "application/json";
                default:
                    return "application/octet-stream";
            }
        }

        public static FormatType MapAcceptToFormat(string accept)
        {
            string acc = accept.ToLower();
            if (acc == "application/xml" || acc == "text/xml")
                return FormatType.XML;
            if (acc == "application/json")
                return FormatType.JSON;

            return FormatType.RAW;
        }
    }
}
