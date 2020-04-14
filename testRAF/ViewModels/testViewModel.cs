using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using testRAF.Models;
using testRAF.Models.Entity;

namespace testRAF.ViewModels
{
    public class testViewModel
    {
        public TheMovieDb Filmler { get; set; }
        public uDetay ud { get; set; }
        public izlenenler iz { get; set; }
        public IEnumerable<Filmler> filmler_list { get; set; }
        public IEnumerable<Yorumlar> Yorumlar { get; set; }
        public IEnumerable<User> Users { get; set; }
        public Roller Roller { get; set; }
    }
}