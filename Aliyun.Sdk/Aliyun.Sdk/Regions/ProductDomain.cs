using System;
using System.Collections.Generic;
using System.Text;

namespace Aliyuncs.Regions
{
    public class ProductDomain
    {
        public ProductDomain(string product, string domain)
        {
            ProductName = product;
            DomianName = domain;
        }

        public string DomianName { get; internal set; }
        public string ProductName { get; internal set; }
    }
}
