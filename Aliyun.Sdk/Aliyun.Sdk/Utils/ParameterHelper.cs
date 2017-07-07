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

        public static String getUniqueNonce()
        {
            Guid uuid = Guid.NewGuid();
            return uuid.ToString("N");
        }

        public static String getISO8601Time(DateTime date)
        {
            DateTime nowDate = date;
            if (null == date)
            {
                nowDate = DateTime.UtcNow;
            }
            SimpleDateFormat df = new SimpleDateFormat(FORMAT_ISO8601);
            df.setTimeZone(new SimpleTimeZone(0, TIME_ZONE));

            return df.format(nowDate);
        }

        public static String getRFC2616Date(DateTime date)
        {
            DateTime nowDate = date;
            if (null == date)
            {
                nowDate = DateTime.UtcNow;
            }
            SimpleDateFormat df = new SimpleDateFormat(FORMAT_RFC2616, Locale.ENGLISH);
            df.setTimeZone(new SimpleTimeZone(0, TIME_ZONE));
            return df.format(nowDate);
        }

        public static DateTime parse(String strDate)
        {
            if (null == strDate || "".equals(strDate))
            {
                return null;
            }
            try
            {
                return parseISO8601(strDate);
            }
            catch (ParseException exp)
            {
                return parseRFC2616(strDate);
            }
        }

        public static DateTime parseISO8601(String strDate)
        {
            if (null == strDate || "".equals(strDate))
            {
                return null;
            }
            SimpleDateFormat df = new SimpleDateFormat(FORMAT_ISO8601);
            df.setTimeZone(new SimpleTimeZone(0, TIME_ZONE));
            return df.parse(strDate);
        }

        public static DateTime parseRFC2616(String strDate)
        {
            if (null == strDate || "".equals(strDate) || strDate.length() != FORMAT_RFC2616.length())
            {
                return null;
            }
            SimpleDateFormat df = new SimpleDateFormat(FORMAT_RFC2616, Locale.ENGLISH);
            df.setTimeZone(new SimpleTimeZone(0, TIME_ZONE));
            return df.parse(strDate);
        }

        public static String md5Sum(byte[] buff)
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
