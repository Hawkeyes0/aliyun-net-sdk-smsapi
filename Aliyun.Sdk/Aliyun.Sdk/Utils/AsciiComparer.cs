using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aliyuncs.Utils
{
    public class AsciiComparer : IComparer
    {
        public int Compare(object x, object y)
        {
            string sx = x as string;
            string sy = y as string;

            if (string.IsNullOrEmpty(sx) && string.IsNullOrEmpty(sy))
                return 1;
            if (string.IsNullOrEmpty(sx))
                return -1;
            if (string.IsNullOrEmpty(sy))
                return 1;

            int len = Math.Min(sx.Length, sy.Length);
            int result = 0;

            for(int i = 0; i < len; i++)
            {
                result = sx[i] - sy[i];
                if (result != 0)
                    return result;
            }
            return result;
        }
    }
}
