using System;
using System.Collections.Generic;
using System.Text;

namespace Libery_Frontend.SecondModels
{
    public class ProductModel
    {
        public string Image { get; set; } = default;
        public string Name { get; set; } = default;
        public string Info { get; set; } = default;
        public string InfoConcat { get; set; } = default;
        public string Type { get; set; } = default;
        public int? ProId { get; set; } = default;
        public double? UnitPrice { get; set; } = default;
        public int? AuthorID { get; set; } = default;
        public int? DirectorID { get; set; } = default;
        public DateTime? ReleaseDate { get; set; } = default;
        public bool? IsBookable { get; set; } = default;
        public int? CategoryID { get; set; } = default;
        public string ISBN { get; set; } = default;
        public int? Pages { get; set; } = default;
        public string Category { get; set; } = default;
        public string AuthorName { get; set; }

    }

    public class GroupedProducts
    {
        public string ProductType { get; set; } = default;
        public List<Product> Products { get; set; } = new List<Product>();
    }
}
