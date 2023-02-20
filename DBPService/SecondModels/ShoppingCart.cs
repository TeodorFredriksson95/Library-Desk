using System;
using System.Collections.Generic;

#nullable disable

namespace DBPService.SecondModels
{
    public partial class ShoppingCart
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int? ProductId { get; set; }
        public DateTime? DateBooked { get; set; }
        public DateTime? ReturnDate { get; set; }

        public virtual Product Product { get; set; }
        public virtual User User { get; set; }
    }
}
