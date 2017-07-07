using System;
using System.Collections.Generic;
using System.Text;

namespace Aliyuncs.Http
{
    public class HttpRequest
    {
        protected const String CONTENT_TYPE = "Content-Type";
        protected const String CONTENT_MD5 = "Content-MD5";
        protected const String CONTENT_LENGTH = "Content-Length";

        private FormatType _contentType;

        public string Url { get; protected set; }
        public string Encoding { get; set; }
        public FormatType ContentType
        {
            get { return _contentType; }
            set
            {
                _contentType = value;
                if (null != Content || null != _contentType)
                    Headers["CONTENT_TYPE"] = GetContentTypeValue(_contentType, Encoding);
                else
                    Headers.Remove("CONTENT_TYPE");
            }
        }
        public MethodType Method { get; set; }
        public byte[] Content { get; protected set; }
        public Dictionary<string, string> Headers { get; set; }
        public int ConnectTimeout { get; set; }
        public int ReadTimeout { get; set; }

        public HttpRequest(String strUrl)
        {
            Url = strUrl;
            Headers = new Dictionary<string, string>();
        }

        public HttpRequest(String strUrl, Dictionary<string, string> tmpHeaders)
        {
            Url = strUrl;
            if (null != tmpHeaders)
                Headers = tmpHeaders;
        }

        public HttpRequest()
        {
        }

        public void SetContent(byte[] content, string encoding, FormatType format)
        {
            if (null == content)
            {
                Headers.Remove(CONTENT_MD5);
                Headers.Add(CONTENT_LENGTH, "0");
                Headers.Remove(CONTENT_TYPE);
                ContentType = FormatType.UNKNOWN;
                Content = null;
                Encoding = null;
                return;
            }
            Content = content;
            Encoding = encoding;
            String contentLen = $"{content.Length}";
            String strMd5 = ParameterHelper.md5Sum(content);
            if (null != format)
            {
                ContentType = format;
            }
            else
            {
                ContentType = FormatType.RAW;
            }
            Headers.Add(CONTENT_MD5, strMd5);
            Headers.Add(CONTENT_LENGTH, contentLen);
            Headers.Add(CONTENT_TYPE, getContentTypeValue(ContentType, encoding));
        }
    }
}
