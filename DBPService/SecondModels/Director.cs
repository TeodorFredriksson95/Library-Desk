using System;
using System.Collections.Generic;

#nullable disable

namespace DBPService.SecondModels
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

        public virtual ICollection<Product> Products { get; set; }
    }
}
