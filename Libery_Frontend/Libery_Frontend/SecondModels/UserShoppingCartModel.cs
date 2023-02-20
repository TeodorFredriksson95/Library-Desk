using System;
using System.Collections.Generic;
using System.Text;

namespace Libery_Frontend.SecondModels
{
    public class UserShoppingCartModel
    {
        public string ProductName { get; set; }
        public DateTime? DateToReturn { get; set; }
        public DateTime? BookedDate { get; set; }
        public int? ProductID { get; set; }
        public string ProductType { get; set; }
        public string Username { get; set; }

    }
}
