using System;
using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestSmsApi
{
    [TestClass]
    public class XmlTest
    {
        [TestMethod]
        public void TestXpath()
        {
            string xml = @"<root>
  <node></node>
  <node></node>
  <node></node>
  <node></node>
  <node></node>
  <node></node>
</root>";
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);

            XmlNode root = doc.LastChild;

            var list = root.SelectNodes(root.ChildNodes[0].Name);

            Assert.AreNotEqual(null, list);
            Assert.IsTrue(list.Count == 6);
        }
    }
}
