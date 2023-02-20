using System;
using System.Collections.Generic;

//#nullable disable

namespace Libery_Frontend.Models
{
    public partial class Director
    {
        public Director()
        {
            Products = new HashSet<Product>();
        }

        public int Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public DateTime? Birthday { get; set; }
        public override string ToString() => $"{Firstname} {Lastname}";


        public virtual ICollection<Product> Products { get; set; }
    }
}
