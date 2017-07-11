using Aliyuncs.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Cache;
using System.Threading;

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
                if (null != Content || FormatType.UNKNOWN != _contentType)
                    Headers["CONTENT_TYPE"] = GetContentTypeValue(_contentType, Encoding);
                else
                    Headers.Remove("CONTENT_TYPE");
            }
        }

        public HttpWebRequest GetHttpConnection()
        {
            if (string.IsNullOrEmpty(Url))
                return null;

            string uri;
            string[] uriArr = null;
            if (Method == MethodType.POST && Content == null)
            {
                uriArr = Url.Split('?');
                uri = uriArr[0];
            }
            else
            {
                uri = Url;
            }
            HttpWebRequest conn = WebRequest.CreateHttp(uri);
            conn.Method = Method.ToString();
            conn.CachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore);
            conn.Timeout = ConnectTimeout;
            conn.ReadWriteTimeout = ReadTimeout;
            foreach (string name in Headers.Keys)
            {
                conn.Headers[name] = Headers[name];
            }

            if (Headers.ContainsKey(CONTENT_TYPE) && !string.IsNullOrEmpty(Headers[CONTENT_TYPE]))
                conn.ContentType = Headers[CONTENT_TYPE];
            else
            {
                string ct = GetContentTypeValue(ContentType, Encoding);
                if (!string.IsNullOrEmpty(ct))
                {
                    conn.ContentType = ct;
                }
            }

            if(Method == MethodType.POST && uriArr != null && uriArr.Length == 2)
            {
                byte[] buff = System.Text.Encoding.UTF8.GetBytes(uriArr[1]);
                conn.GetRequestStream().Write(buff, 0, buff.Length);
            }

            return conn;
        }

        public MethodType Method { get; set; }
        public byte[] Content { get; protected set; }
        public Dictionary<string, string> Headers { get; set; }
        public int ConnectTimeout { get; set; } = Timeout.Infinite;
        public int ReadTimeout { get; set; } = Timeout.Infinite;

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

        public virtual void SetContent(byte[] content, string encoding, FormatType format)
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
            String strMd5 = ParameterHelper.Md5Sum(content);
            if (FormatType.UNKNOWN != format)
            {
                ContentType = format;
            }
            else
            {
                ContentType = FormatType.RAW;
            }
            Headers.Add(CONTENT_MD5, strMd5);
            Headers.Add(CONTENT_LENGTH, contentLen);
            Headers.Add(CONTENT_TYPE, GetContentTypeValue(ContentType, encoding));
        }

        private string GetContentTypeValue(FormatType contentType, string encoding)
        {
            if (FormatType.UNKNOWN != contentType && null != encoding)
            {
                return FormatTypeHelper.MapFromatToAccept(contentType) +
                        ";charset=" + encoding.ToLower();
            }
            else if (FormatType.UNKNOWN != contentType)
            {
                return FormatTypeHelper.MapFromatToAccept(contentType);
            }
            return null;
        }
    }
}
