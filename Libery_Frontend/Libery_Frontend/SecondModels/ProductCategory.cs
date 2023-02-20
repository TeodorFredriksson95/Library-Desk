using System;
using System.Collections.Generic;

//#nullable disable

namespace Libery_Frontend.SecondModels
{
    public partial class ProductCategory
    {
        public ProductCategory()
        {
            Products = new HashSet<Product>();
        }

        public int Id { get; set; }
        public string Category { get; set; }
        public override string ToString() => $"{Category}";

        public virtual ICollection<Product> Products { get; set; }
    }
}
