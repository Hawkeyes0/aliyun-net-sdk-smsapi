using Aliyuncs.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;

namespace Aliyuncs.Http
{
    public class HttpResponse : HttpRequest
    {
        public int Status { get; internal set; }
        public bool IsSuccess => 200 <= Status && 300 > Status;

        public HttpResponse(String strUrl) : base(strUrl)
        {
        }

        public HttpResponse()
        {
        }

        public override void SetContent(byte[] content, string encoding, FormatType format)
        {
            Content = content;
            Encoding = encoding;
            ContentType = format;
        }

        internal static HttpResponse GetResponse(HttpRequest request)
        {
            HttpWebRequest httpConn = request.GetHttpConnection();
            HttpWebResponse resp = null;
            HttpResponse response = new HttpResponse(httpConn.RequestUri.ToString());

            try
            {
                resp = (HttpWebResponse)httpConn.GetResponseAsync().Result;
                ParseHttpConn(response, resp);
                return response;
            }
            catch (Exception e)
            {
                Trace.TraceError(e.Message);
                if (e.InnerException is WebException)
                {
                    WebException we = e.InnerException as WebException;
                    resp = we.Response as HttpWebResponse;
                    ParseHttpConn(response, resp);
                    return response;
                }
                return null;
            }
            finally
            {
            }
        }

        private static void ParseHttpConn(HttpResponse response, HttpWebResponse httpConn)
        {
            byte[] buff = ReadContent(httpConn.GetResponseStream());
            response.Status = (int)httpConn.StatusCode;
            foreach (string name in httpConn.Headers.Keys)
            {
                response.Headers[name] = httpConn.Headers[name];
            }

            string type = response.Headers[CONTENT_TYPE];
            if (!string.IsNullOrEmpty(type) && buff != null)
            {
                response.Encoding = "UTF-8";
                string[] split = type.Split(';');
                response.ContentType = FormatTypeHelper.MapAcceptToFormat(split[0].Trim());
                if (split.Length > 1 && split[1].Contains("="))
                {
                    string[] codings = split[1].Split('=');
                    response.Encoding = codings[1].Trim().ToUpper();
                }
            }
            response.SetContent(buff, response.Encoding, response.ContentType);
        }

        private static byte[] ReadContent(Stream content)
        {
            List<byte> lst = new List<byte>();

            byte[] buff = new byte[1024];
            int len = 0;
            while ((len = content.Read(buff, 0, buff.Length)) > 0)
            {
                for (int i = 0; i < len; i++)
                {
                    lst.Add(buff[i]);
                }
            }

            return lst.ToArray();
        }
    }
}
