using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Aliyuncs.Utils
{
    internal class XmlChildNodes : XmlNodeList
    {
        private XmlNode container;

        public XmlChildNodes(XmlNode container)
        {
            this.container = container;
        }

        public override int Count
        {
            get
            {
                int c = 0;
                for (XmlNode n = container.FirstChild; n != null; n = n.NextSibling)
                {
                    if (n is XmlElement)
                        c++;
                }
                return c;
            }
        }

        public override IEnumerator GetEnumerator()
        {
            if (container.FirstChild == null)
            {
                return new EmptyEnumerator();
            }
            else
            {
                return new XmlChildEnumerator(container);
            }
        }

        public override XmlNode Item(int i)
        {
            // Out of range indexes return a null XmlNode
            if (i < 0)
                return null;
            for (XmlNode n = container.FirstChild; n != null; n = n.NextSibling, i--)
            {
                if(!(n is XmlElement))
                {
                    i++;
                    continue;
                }
                if (i == 0)
                    return n;
            }
            return null;
        }
    }

    internal sealed class EmptyEnumerator : IEnumerator
    {
        bool IEnumerator.MoveNext()
        {
            return false;
        }

        void IEnumerator.Reset()
        {
        }

        object IEnumerator.Current
        {
            get
            {
                throw new InvalidOperationException("无效的操作。");
            }
        }
    }

    internal sealed class XmlChildEnumerator : IEnumerator
    {
        internal XmlNode container;
        internal XmlNode child;
        internal bool isFirst;

        internal XmlChildEnumerator(XmlNode container)
        {
            this.container = container;
            child = container.FirstChild;
            isFirst = true;

            while (!(child is XmlElement) && child != null)
            {
                child = child.NextSibling;
            }
        }

        bool IEnumerator.MoveNext()
        {
            return MoveNext();
        }

        internal bool MoveNext()
        {
            if (isFirst)
            {
                child = container.FirstChild;
                isFirst = false;
            }
            else if (child != null)
            {
                child = child.NextSibling;
            }

            return child != null;
        }

        void IEnumerator.Reset()
        {
            isFirst = true;
            child = container.FirstChild;
            while (!(child is XmlElement) && child != null)
            {
                child = child.NextSibling;
            }
        }

        object IEnumerator.Current
        {
            get
            {
                return Current;
            }
        }

        internal XmlNode Current
        {
            get
            {
                if (isFirst || child == null)
                    throw new InvalidOperationException("无效的操作。");

                return child;
            }
        }
    }
}
