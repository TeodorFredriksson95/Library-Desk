using System;
using System.Collections.Generic;

//#nullable disable

namespace Libery_Frontend.SecondModels
{
    public partial class ProductType
    {
        public ProductType()
        {
            Products = new HashSet<Product>();
        }

        public int Id { get; set; }
        public string Type { get; set; }

        public override string ToString() => $"{Type}";

        public virtual ICollection<Product> Products { get; set; }
    }
}
