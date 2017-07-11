using System;
using System.Xml;
using System.Collections.Generic;
using Aliyuncs.Utils;

namespace Aliyuncs.Reader
{
    internal class XmlReader : IReader
    {
        Dictionary<string, string> map = new Dictionary<string, string>();

        public Dictionary<string, string> Read(string response, string endpoint)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(response);
            XmlNode root = doc.LastChild;
            Read(root, endpoint, false);

            return map;
        }

        private void Read(XmlNode element, string path, bool appendPath)
        {
            path = BuildPath(element, path, appendPath);
            var childElements = GetChildNodes(element);

            if (childElements.Count == 0)
            {
                map[path] = element.InnerText;
                return;
            }

            var listElements = SelectNodes(element, childElements[0].Name);
            if (listElements.Count > 1 && childElements.Count == listElements.Count)
            {
                ElementsAsList(childElements, path);
            }
            else if (listElements.Count == 1 && childElements.Count == 1)
            {
                ElementsAsList(listElements, path);
                Read(childElements[0], path, true);
            }
            else
            {
                foreach (XmlNode childElement in childElements)
                {
                    Read(childElement, path, true);
                }
            }
        }

        private List<XmlNode> SelectNodes(XmlNode element, string name)
        {
            List<XmlNode> list = new List<XmlNode>();
            foreach (XmlNode n in element.SelectNodes(name))
            {
                if (n is XmlElement)
                    list.Add(n);
            }
            return list;
        }

        private List<XmlNode> GetChildNodes(XmlNode element)
        {
            List<XmlNode> list = new List<XmlNode>();
            foreach(XmlNode n in element.ChildNodes)
            {
                if (n is XmlElement)
                    list.Add(n);
            }
            return list;
        }

        private void ElementsAsList(List<XmlNode> listElements, string path)
        {
            map[path + ".Length"] = listElements.Count.ToString();
            for (int i = 0; i < listElements.Count; i++)
            {
                Read(listElements[i], path + "[" + i + "]", false);
            }
        }

        private string BuildPath(XmlNode element, string path, bool appendPath)
        {
            return appendPath ? path + "." + element.Name : path;
        }
    }
}