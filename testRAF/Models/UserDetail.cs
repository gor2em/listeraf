using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace testRAF.Models
{
    public class UserDetail
    {
        public int DetId { get; set; }
        public int UserId { get; set; }
        public int topIzlenen { get; set; }
        public int topBegenilen { get; set; }
        public int topDahaSonraIzle { get; set; }
        public int topYorum { get; set; }
    }
}