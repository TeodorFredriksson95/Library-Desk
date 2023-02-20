using System;
using System.Collections.Generic;
using System.Text;

namespace Libery_Frontend.Models
{
    public partial class MetaStatistic
    {
        public int Id { get; set; }
        public string PageId { get; set; }
        public string PageName { get; set; }
        public long Visits { get; set; }
        public double Mean { get; set; }
    }
}
