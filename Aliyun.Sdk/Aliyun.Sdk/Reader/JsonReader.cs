using Aliyuncs.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aliyuncs.Reader
{
    internal class JsonReader : IReader
    {
        private const int FIRST_POSITION = 0;
        private const int CURRENT_POSITION = 1;
        private const int NEXT_POSITION = 2;

        private static readonly object ARRAY_END_TOKEN = new object();
        private static readonly object OBJECT_END_TOKEN = new object();
        private static readonly object COMMA_TOKEN = new object();
        private static readonly object COLON_TOKEN = new object();

        private CharIterator ct;
        private char c;
        private Dictionary<string, string> map = new Dictionary<string, string>();
        private object token;
        private StringBuilder stringBuffer = new StringBuilder();
        private static Dictionary<char, char> escapes = new Dictionary<char, char>
        {
            {'\\', '\\'},{'/', '/'},{'"', '"'},{'t', '\t'},{'n', '\n'},{'r', '\r'},{'b', '\b'},{'f', '\f'},
        };

        public Dictionary<string, string> Read(string response, string endpoint)
        {
            return Read(response, endpoint, 0);
        }

        private Dictionary<string, string> Read(string response, string endpoint, int start)
        {
            ct = CharIterator.Get(response);
            switch (start)
            {
                case FIRST_POSITION:
                    c = ct.First();
                    break;
                case CURRENT_POSITION:
                    c = ct.Current;
                    break;
                case NEXT_POSITION:
                    c = ct.Next();
                    break;
            }
            ReadJson(endpoint);
            return map;
        }

        private object ReadJson(string baseKey)
        {
            SkipWhiteSpace();
            char ch = c;
            NextChar();
            switch (ch)
            {
                case '{': ProcessObject(baseKey); break;
                case '}': token = OBJECT_END_TOKEN; break;
                case '[':
                    if (c == '"')
                    {
                        ProcessList(baseKey); break;
                    }
                    else
                    {
                        ProcessArray(baseKey); break;
                    }
                case ']': token = ARRAY_END_TOKEN; break;
                case '"': token = ProcessString(); break;
                case ',': token = COMMA_TOKEN; break;
                case ':': token = COLON_TOKEN; break;
                case 't':
                    NextChar(); NextChar(); NextChar();
                    token = true;
                    break;
                case 'n':
                    NextChar(); NextChar(); NextChar();
                    token = null;
                    break;
                case 'f':
                    NextChar(); NextChar(); NextChar(); NextChar();
                    token = false;
                    break;
                default:
                    c = ct.Previous();
                    if (char.IsDigit(c) || c == '-')
                    {
                        token = ProcessNumber();
                    }
                    break;
            }
            return token;
        }

        private void SkipWhiteSpace()
        {
            while (char.IsWhiteSpace(c))
                NextChar();
        }

        private void ProcessObject(string baseKey)
        {
            String key = baseKey + "." + ReadJson(baseKey);
            while (token != OBJECT_END_TOKEN)
            {
                ReadJson(key);
                if (token != OBJECT_END_TOKEN)
                {
                    object obj = ReadJson(key);
                    if (obj is string || obj is bool || obj is long || obj is int || obj is float || obj is double)
                    {
                        map[key] = obj.ToString();
                    }

                    if (ReadJson(key) == COMMA_TOKEN)
                    {
                        key = ReadJson(key).ToString();
                        key = baseKey + "." + key;
                    }
                }
            }
        }

        private void ProcessList(string baseKey)
        {
            object value = ReadJson(baseKey);
            int index = 0;
            while (token != ARRAY_END_TOKEN)
            {
                string key = TrimFromLast(baseKey, ".") + "[" + (index++) + "]";
                map[key] = value.ToString();
                if (ReadJson(baseKey) == COMMA_TOKEN)
                {
                    value = ReadJson(baseKey);
                }
            }
            map[TrimFromLast(baseKey, ".") + ".Length"] = index.ToString();
        }

        private void ProcessArray(string baseKey)
        {
            int index = 0;
            string preKey = baseKey.Substring(0, baseKey.LastIndexOf("."));
            string key = preKey + "[" + index + "]";
            Object value = ReadJson(key);

            while (token != ARRAY_END_TOKEN)
            {
                map[preKey + ".Length"] = (index + 1).ToString();
                if (value is string)
                {
                    map[key] = value.ToString();
                }
                if (ReadJson(baseKey) == COMMA_TOKEN)
                {
                    key = preKey + "[" + (++index) + "]";
                    value = ReadJson(key);
                }
            }
        }

        private object ProcessString()
        {
            stringBuffer.Length = 0;
            while (c != '"')
            {
                if (c == '\\')
                {
                    NextChar();
                    Object value = escapes[c];
                    if (value != null)
                    {
                        AddChar((char)value);
                    }
                }
                else
                {
                    AddChar();
                }
            }
            NextChar();
            return stringBuffer.ToString();
        }

        private void AddChar() => AddChar(c);

        private void AddChar(char ch)
        {
            stringBuffer.Append(ch);
            NextChar();
        }

        private char NextChar()
        {
            ct.Next();
            return ct.Current;
        }

        private object ProcessNumber()
        {
            stringBuffer.Length = 0;
            if ('-' == c)
            {
                AddChar();
            }
            AddDigits();
            if ('.' == c)
            {
                AddChar();
                AddDigits();
            }
            if ('e' == c || 'E' == c)
            {
                AddChar();
                if ('+' == c || '-' == c)
                {
                    AddChar();
                }
                AddDigits();
            }
            return stringBuffer.ToString();
        }

        private void AddDigits()
        {
            while (char.IsDigit(c))
            {
                AddChar();
            }
        }

        public static string TrimFromLast(string str, string stripString)
        {
            int pos = str.LastIndexOf(stripString);
            if (pos > -1)
            {
                return str.Substring(0, pos);
            }
            else
            {
                return str;
            }
        }
    }
}