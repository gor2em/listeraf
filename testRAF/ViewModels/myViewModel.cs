using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using testRAF.Models.Entity;
using testRAF.Models;
namespace testRAF.ViewModels
{
    public class myViewModel
    {
        public User user { get; set; }
        //public LoginModel loginModel { get; set; }
        public TheMovieDb filmler { get; set; }
        public resimler resim { get; set; }
        public uDetay UD { get; set; }
        public IEnumerable<uDetay> udetay { get; set; }

        public listeler list { get; set; }
        public IEnumerable<listeler> listeler { get; set; }

        public IEnumerable<izlenenler> izlediklerim { get; set; }
        public IEnumerable<begenilenler> begendiklerim { get; set; }
        public IEnumerable<dahasonraizle> dahasonraizle { get; set; }
        public IEnumerable<Yorumlar> yorumlarim { get; set; }

        public IEnumerable<Filmler> pop_filmler { get; set; }
        public IEnumerable<Filmler> rastgele_film { get; set; }

    }
}