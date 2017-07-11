using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aliyuncs.Utils
{
    public class CharIterator
    {
        public char Current => target[Position];
        public int Position { get; private set; } = 0;

        private readonly string target;

        public CharIterator(string target)
        {
            this.target = target;
        }

        internal static CharIterator Get(string target)
        {
            return new CharIterator(target);
        }

        internal char Next()
        {
            return target[++Position];
        }

        internal char First()
        {
            return target[(Position = 0)];
        }

        internal char Previous()
        {
            return target[--Position];
        }
    }
}
