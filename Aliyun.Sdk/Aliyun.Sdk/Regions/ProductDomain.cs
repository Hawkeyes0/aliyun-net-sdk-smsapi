using System;
using System.Collections.Generic;
using System.Text;

namespace Aliyuncs.Regions
{
    public class ProductDomain
    {
        private string product;
        private string domain;

        public ProductDomain(string product, string domain)
        {
            this.product = product;
            this.domain = domain;
        }

        public string DomianName { get; internal set; }
        public string ProductName { get; internal set; }
    }
}
