using System;
using System.Collections.Generic;

//#nullable disable

namespace Libery_Frontend.SecondModels
{
    public partial class Product
    {
        public Product()
        {
            OrderDetails = new HashSet<OrderDetail>();
            ShoppingCarts = new HashSet<ShoppingCart>();
        }

        public int Id { get; set; }
        public int? AuthorId { get; set; }
        public int? DirectorId { get; set; }
        public string Isbn { get; set; }
        public int? ProductTypeId { get; set; }
        public int? CategoryId { get; set; }
        public string ProductName { get; set; }
        public string ProductInfo { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public int? BookPages { get; set; }
        public bool? EVersion { get; set; }
        public int? StockValue { get; set; }
        public bool? IsBookable { get; set; }
        public int? IsBookedAmount { get; set; }
        public DateTime? BookedTime { get; set; }
        public double? Price { get; set; }
        public string Image { get; set; }

        public virtual Author Author { get; set; }
        public virtual ProductCategory Category { get; set; }
        public virtual Director Director { get; set; }
        public virtual ProductType ProductType { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<ShoppingCart> ShoppingCarts { get; set; }
    }
}
