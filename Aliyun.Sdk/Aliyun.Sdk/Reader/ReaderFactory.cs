using System;
using System.Collections.Generic;
using System.Text;
using Aliyuncs.Http;

namespace Aliyuncs.Reader
{
    class ReaderFactory
    {
        internal static IReader CreateInstance(FormatType format)
        {
            if (FormatType.JSON == format)
                return new JsonReader();
            if (FormatType.XML == format)
                return new XmlReader();

            return null;
        }
    }
}
