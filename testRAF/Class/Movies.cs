using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace testRAF.Class
{
    public class Movies
    {

    }
    public class testMovies
    {
        public string poster_path { get; set; }
        public string name { get; set; }
        public int id { get; set; }
        public string imdb_id { get; set; }
        public string original_title { get; set; }
        public double vote_average { get; set; }
    }
    public class testResult
    {
        public string poster_path { get; set; }
        public int id { get; set; }
        public string imdb_id { get; set; }
        public string name { get; set; }
        public string original_title { get; set; }
        public List<testMovies> test_movies { get; set; }
        public double vote_average { get; set; }

        public string title { get; set; }
    }
    public class testResponseMovies
    {
        public int page { get; set; }
        public List<testResult> results { get; set; }
        public int total_results { get; set; }
        public int total_pages { get; set; }
    }
    public class ResponseMovies
    {
        public bool adult { get; set; }
        public List<string> also_known_as { get; set; }
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
        public string original_title { get; set; }


        public string title { get; set; }
        public string vote_average { get; set; }
        public string overview { get; set; }
        
        public string release_date { get; set; }
        public string poster_path { get; set; }
        public List<object> genres  { get; set; }
    }
}