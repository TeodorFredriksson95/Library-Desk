using System;
using System.Collections.Generic;

#nullable disable

namespace Backend.Models
{
    public partial class OrderDetail
    {
        public int Id { get; set; }
        public string OrderId { get; set; }
        public int? ProductId { get; set; }
        public double? UnitPrice { get; set; }
        public int? Quantity { get; set; }

        public virtual User Order { get; set; }
        public virtual Product Product { get; set; }
    }
}
