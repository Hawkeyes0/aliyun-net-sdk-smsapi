using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Aliyuncs.Utils
{
    public static class ParameterHelper
    {
        private const String TIME_ZONE = "GMT";
        private const String FORMAT_ISO8601 = "yyyy-MM-dd'T'HH:mm:ss'Z'";
        private const String FORMAT_RFC2616 = "EEE, dd MMM yyyy HH:mm:ss zzz";

        public static String GetUniqueNonce()
        {
            Guid uuid = Guid.NewGuid();
            return uuid.ToString("N");
        }

        public static String GetISO8601Time(DateTime date)
        {
            DateTime nowDate = date;
            if (null == date)
            {
                nowDate = DateTime.UtcNow;
            }

            return nowDate.ToString(FORMAT_ISO8601);
        }

        public static String GetRFC2616Date(DateTime date)
        {
            DateTime nowDate = date;
            if (null == date)
            {
                nowDate = DateTime.UtcNow;
            }
            return nowDate.ToString(FORMAT_RFC2616);
        }

        public static DateTime? Parse(String strDate)
        {
            if (null == strDate || "".Equals(strDate))
            {
                return null;
            }
            try
            {
                return ParseISO8601(strDate);
            }
            catch
            {
                return ParseRFC2616(strDate);
            }
        }

        public static DateTime? ParseISO8601(String strDate)
        {
            if (null == strDate || "".Equals(strDate))
            {
                return null;
            }
            return DateTime.Parse(strDate);
        }

        public static DateTime? ParseRFC2616(String strDate)
        {
            if (null == strDate || "".Equals(strDate) || strDate.Length != FORMAT_RFC2616.Length)
            {
                return null;
            }
            return DateTime.Parse(strDate);
        }

        public static String Md5Sum(byte[] buff)
        {
            try
            {
                MD5 md = new MD5CryptoServiceProvider();
                byte[] messageDigest = md.ComputeHash(buff);
                return Convert.ToBase64String(messageDigest);
            }
            catch
            {
            }
            return null;
        }
    }
}
