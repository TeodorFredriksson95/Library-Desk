using System;
using System.Collections.Generic;

#nullable disable

namespace Backend.Models
{
    public partial class ProductCategory
    {
        public ProductCategory()
        {
            Products = new HashSet<Product>();
        }

        public int Id { get; set; }
        public string Category { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
