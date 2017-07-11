using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aliyuncs.Auth
{
    public static class AcsURLEncoder
    {
        public const string URL_ENCODING = "UTF-8";
        static HashSet<int> dontNeedEncoding = new HashSet<int>();
        const int caseDiff = ('a' - 'A');

        static AcsURLEncoder()
        {
            int i;
            for (i = 'a'; i <= 'z'; i++)
            {
                dontNeedEncoding.Add(i);
            }
            for (i = 'A'; i <= 'Z'; i++)
            {
                dontNeedEncoding.Add(i);
            }
            for (i = '0'; i <= '9'; i++)
            {
                dontNeedEncoding.Add(i);
            }
            dontNeedEncoding.Add(' ');
            dontNeedEncoding.Add('-');
            dontNeedEncoding.Add('_');
            dontNeedEncoding.Add('.');
            dontNeedEncoding.Add('*');
        }

        internal static string PercentEncode(string value)
        {
            return value != null ? Encode(value, URL_ENCODING).Replace("+", "%20")
                    .Replace("*", "%2A").Replace("%7E", "~") : null;
        }

        public static string Encode(string s, string enc = URL_ENCODING)
        {
            bool needToChange = false;
            StringBuilder outsb = new StringBuilder(s.Length);
            Encoding charset;
            List<char> charArrayWriter = new List<char>();

            if (enc == null)
                throw new ArgumentNullException("charsetName");

            try
            {
                charset = Encoding.GetEncoding(enc);
            }
            catch
            {
                throw;
            }

            for (int i = 0; i < s.Length;)
            {
                int c = s[i];
                //System.out.println("Examining character: " + c);
                if (dontNeedEncoding.Contains(c))
                {
                    if (c == ' ')
                    {
                        c = '+';
                        needToChange = true;
                    }
                    //System.out.println("Storing: " + c);
                    outsb.Append((char)c);
                    i++;
                }
                else
                {
                    // convert to external encoding before hex conversion
                    do
                    {
                        charArrayWriter.Add((char)c);
                        /*
                         * If this character represents the start of a Unicode
                         * surrogate pair, then pass in two characters. It's not
                         * clear what should be done if a bytes reserved in the
                         * surrogate pairs range occurs outside of a legal
                         * surrogate pair. For now, just treat it as if it were
                         * any other character.
                         */
                        if (c >= 0xD800 && c <= 0xDBFF)
                        {
                            /*
                              System.out.println(Integer.toHexstring(c)
                              + " is high surrogate");
                            */
                            if ((i + 1) < s.Length)
                            {
                                int d = s[i + 1];
                                /*
                                  System.out.println("\tExamining "
                                  + Integer.toHexstring(d));
                                */
                                if (d >= 0xDC00 && d <= 0xDFFF)
                                {
                                    /*
                                      System.out.println("\t"
                                      + Integer.toHexstring(d)
                                      + " is low surrogate");
                                    */
                                    charArrayWriter.Add((char)d);
                                    i++;
                                }
                            }
                        }
                        i++;
                    } while (i < s.Length && !dontNeedEncoding.Contains((c = s[i])));

                    string str = new string(charArrayWriter.ToArray());
                    byte[] ba = charset.GetBytes(str);
                    for (int j = 0; j < ba.Length; j++)
                    {
                        outsb.Append('%');
                        char ch = ForDigit((ba[j] >> 4) & 0xF, 16);
                        // converting to use uppercase letter as part of
                        // the hex value if ch is a letter.
                        if (char.IsLetter(ch))
                        {
                            ch -= (char)caseDiff;
                        }
                        outsb.Append(ch);
                        ch = ForDigit(ba[j] & 0xF, 16);
                        if (char.IsLetter(ch))
                        {
                            ch -= (char)caseDiff;
                        }
                        outsb.Append(ch);
                    }
                    charArrayWriter.Clear();
                    needToChange = true;
                }
            }

            return (needToChange ? outsb.ToString() : s);
        }

        private static char ForDigit(int digit, int radix)
        {
            if ((digit >= radix) || (digit < 0))
            {
                return '\0';
            }
            if ((radix < 2) || (radix > 36))
            {
                return '\0';
            }
            if (digit < 10)
            {
                return (char)('0' + digit);
            }
            return (char)('a' - 10 + digit);
        }
    }
}
