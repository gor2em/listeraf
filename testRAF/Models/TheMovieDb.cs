using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using testRAF.Models.Entity;

namespace testRAF.Models
{
    public class TheMovieDb
    {
        public string searchText { get; set; }
        public bool adult { get; set; }
        public string also_known_as { get; set; }
        public string biography { get; set; }
        public string birthday { get; set; }
        public string deathday { get; set; }
        public int gender { get; set; }
        public string homepage { get; set; }
        public int id { get; set; }
        public string imdb_id { get; set; }
        public string name { get; set; }
        public string place_of_birth { get; set; }
        public double popularity { get; set; }
        public string profile_path { get; set; }

        public string movieName { get; set; }
        public string poster_path { get; set; }
        public string original_title { get; set; }


        /*film detay için alanlar*/
        public string title { get; set; }
        [Display(Name = "Puan:")]
        public string vote_average { get; set; }

        [Display(Name = "Konu:")]
        public string overview { get; set; }

        [Display(Name = "Yayın Tarihi:")]
        public string release_date { get; set; }

        public virtual Yorumlar yorum { get; set; }


    }
}