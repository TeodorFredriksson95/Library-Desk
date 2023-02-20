using System;
using System.Collections.Generic;

#nullable disable

namespace DBPService.Models
{
    public partial class Order
    {
        public int Id { get; set; }
        public string CustomerUsername { get; set; }
        public int? CustomerId { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerPostalCode { get; set; }
        public string CustomerCity { get; set; }

        public virtual User CustomerUsernameNavigation { get; set; }
    }
}
