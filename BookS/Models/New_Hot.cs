using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookS.Models
{
    public class New_Hot
    {
        public IEnumerable<BookS.Models.CT_DON_HANG> banChay { get; set; }
        public IEnumerable<BookS.Models.DEVICE> moi { get; set; }
    }
}