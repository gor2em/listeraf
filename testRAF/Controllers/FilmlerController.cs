using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using testRAF.Models;
using testRAF.Class;
using testRAF.Models.Entity;
using testRAF.ViewModels;
using System.Web.Script.Serialization;
using Newtonsoft.Json.Linq;

namespace testRAF.Controllers
{

    public class FilmlerController : Controller
    {

        // GET: Filmler
        listerafDBEntities4 lr = new listerafDBEntities4();

        public ActionResult Index(string mN, int? page)
        {
            if (page != null)
                CallAPI(mN, Convert.ToInt32(page));
            else
                CallAPI(mN, 0);


            Models.TheMovieDb theMovieDb = new Models.TheMovieDb();
            theMovieDb.movieName = mN;
            return View(theMovieDb);
        }

        [HttpPost]
        public ActionResult Index(Models.TheMovieDb theMovieDb, string movieName)
        {
            if (ModelState.IsValid)
            {
                CallAPI(movieName, 0);
            }
            return View(theMovieDb);
        }
        public void CallAPI(string movieName, int page)
        {
            int pageNo = Convert.ToInt32(page) == 0 ? 1 : Convert.ToInt32(page);

            /*Calling API https://developers.themoviedb.org/3/movie/top_rated */
            string apiKey = "a64ffa7c8877a661d43e9a9c0896f2c8";
            HttpWebRequest apiRequest = WebRequest.Create("https://api.themoviedb.org/3/movie/top_rated?api_key=" + apiKey + "&language=tr&region=tr&query=" + movieName + "&page=" + pageNo + "&include_adult=false") as HttpWebRequest;

            string apiResponse = "";
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3
                    | SecurityProtocolType.Tls
                    | SecurityProtocolType.Tls11
                    | SecurityProtocolType.Tls12;
            using (HttpWebResponse response = apiRequest.GetResponse() as HttpWebResponse)
            {
                StreamReader reader = new StreamReader(response.GetResponseStream());
                apiResponse = reader.ReadToEnd();
            }
            /*End*/


            testResponseMovies rootObject = JsonConvert.DeserializeObject<testResponseMovies>(apiResponse);

            StringBuilder sb = new StringBuilder();
            sb.Append("<div class=\"container\">");
            foreach (testResult result in rootObject.results)
            {
                string image = result.poster_path == null ? Url.Content("~/Content/Image/no-image.png") : "https://image.tmdb.org/t/p/w500/" + result.poster_path;
                string link = Url.Action("Detay", "Filmler", new { id = result.id });
                sb.Append("<div class=\"col-lg-2 imgnp\" resourceId=\"" + result.id + "\">" + "<a href=\"" + link + "\"><img src=\"" + image + "\" class=\"img-responsive imgH\" />" + "<p>" + result.title + "</a></p></div>");
            }
            ViewBag.Result = sb.ToString();

            int pageSize = 20;
            PagingInfo pagingInfo = new PagingInfo();
            pagingInfo.CurrentPage = pageNo;
            pagingInfo.TotalItems = rootObject.total_results;
            pagingInfo.ItemsPerPage = pageSize;
            ViewBag.Paging = pagingInfo;
        }
        
        [HttpGet]
        public ActionResult Detay(int id, Yorumlar y, Filmler fl, filmResim fresim, string imdb_id)
        {


            /*Calling API https://developers.themoviedb.org/3/people */
            string apiKey = "a64ffa7c8877a661d43e9a9c0896f2c8";
            HttpWebRequest apiRequest = WebRequest.Create("https://api.themoviedb.org/3/movie/" + id + "?api_key=" + apiKey + "&language=tr") as HttpWebRequest;

            string apiResponse = "";
            using (HttpWebResponse response = apiRequest.GetResponse() as HttpWebResponse)
            {
                StreamReader reader = new StreamReader(response.GetResponseStream());
                apiResponse = reader.ReadToEnd();
            }
            /*End*/


            ResponseMovies rootObject = JsonConvert.DeserializeObject<ResponseMovies>(apiResponse);
            TheMovieDb theMovieDb = new TheMovieDb();
            genres g = new genres();

            testViewModel finalModel = new testViewModel();

            theMovieDb.id = rootObject.id;
            theMovieDb.imdb_id = rootObject.imdb_id;
            theMovieDb.title = rootObject.title;
            theMovieDb.vote_average = rootObject.vote_average;
            theMovieDb.overview = rootObject.overview;
            theMovieDb.release_date = rootObject.release_date;
            theMovieDb.original_title = rootObject.original_title;
            //theMovieDb.g_name = string.Join(",", rootObject.genres);
            object[] genres = new object[] { rootObject.genres };
   
            //var jObject = JObject.Parse(genres);
            //jObject["name"] = JsonConvert.SerializeObject(jObject["name"].ToObject<string[]>());

            //var result = JsonConvert.DeserializeObject<Dictionary<string, string>>(jObject.ToString());
            //object[] genres = new object[] { rootObject.genres };

            //for (int i = 0; i < genres.Length; i++)
            //    g.name = genres[i].ToString();


            //g.name = string.Join(",", genres);


            theMovieDb.poster_path = rootObject.poster_path == null ? Url.Content("~/Content/Image/no-image.png") : "https://image.tmdb.org/t/p/w500/" + rootObject.poster_path;

            var model = lr.Yorumlars.Where(x => x.imdb_id == theMovieDb.imdb_id && x.durum == true).ToList();

            var kontrolFilmId = lr.Filmlers.Any(f => f.id == theMovieDb.id);
            var flist = lr.Filmlers;
            if (kontrolFilmId)
            {
                flist.ToList();
            }
            else
            {
                fl.title = theMovieDb.title;
                fl.id = theMovieDb.id;
                fl.imdb_id = theMovieDb.imdb_id;
                fresim.poster_path = "https://image.tmdb.org/t/p/w500/" + rootObject.poster_path;
                fl.izlenme = 0;
                fl.begenme = 0;
                fl.dahasonraizle = 0;
                fl.yorumlar = 0;
                fl.FResimId = fresim.FResimId;

                lr.Filmlers.Add(fl);
                lr.filmResims.Add(fresim);
                lr.SaveChanges();
            }
            //int userId = lr.Users.FirstOrDefault(x => x.UserName == User.Identity.Name).UserId;
            //i.UserId = userId;

            //finalModel.iz = i;
            //finalModel.iz.id = id;
            finalModel.filmler_list = flist;
            finalModel.Yorumlar = model;
            finalModel.Filmler = theMovieDb;

            return View("Detay", finalModel);
        }




        //puan
        public int Puan(int min, int max)
        {
            Random r = new Random();
            int sayi = r.Next(min, max);
            return sayi;

        }

        [HttpPost]
        public string Izlediklerim(izlenenler i, uDetay ud, Filmler fl,string imdb_id)
        {
            var kontrolId = lr.izlenenlers.Any(x => x.imdb_id == i.imdb_id && x.UserId == i.UserId);
            if (kontrolId)
            {
                var izBul = lr.uDetays.Find(ud.UserId);
                izlenenler iz = lr.izlenenlers.FirstOrDefault(x => x.imdb_id == imdb_id && x.UserId == ud.UserId);
                lr.izlenenlers.Remove(iz);
                var topIzlenen = izBul.topIzlenen--;

                Filmler f = lr.Filmlers.FirstOrDefault(z => z.imdb_id == imdb_id);
                var a = f.izlenme--;
                lr.SaveChanges();
                return "true";
            }
            else
            {
                lr.izlenenlers.Add(i);
                var x = lr.uDetays.Find(ud.UserId);
                var topIzlenen = x.topIzlenen++;

                var y = lr.Filmlers.Find(fl.TID);
                var topFilmIzlenen = y.izlenme++;

                var puan = x.Puan += Puan(1, 8);


                //Random r = new Random();
                //int puan = r.Next(1, 10);
                //var puan_al = x.Puan+=puan;

                lr.SaveChanges();
                return "false";
            }
        }

        [HttpPost]
        public string Begendiklerim(begenilenler b, uDetay ud, Filmler fl,string imdb_id)
        {
            var kontrolId = lr.begenilenlers.Any(x => x.imdb_id == b.imdb_id && x.UserId == b.UserId);
            if (kontrolId)
            {
                var izBul = lr.uDetays.Find(ud.UserId);
                begenilenler be = lr.begenilenlers.FirstOrDefault(x => x.imdb_id == imdb_id && x.UserId == ud.UserId);
                lr.begenilenlers.Remove(be);
                var topIzlenen = izBul.topBegenilen--;

                Filmler f = lr.Filmlers.FirstOrDefault(z => z.imdb_id == imdb_id);
                var a = f.begenme--;
                lr.SaveChanges();
                return "true";
            }
            else
            {
                lr.begenilenlers.Add(b);
                var x = lr.uDetays.Find(ud.UserId);
                var topBegenilen = x.topBegenilen++;

                var y = lr.Filmlers.Find(fl.TID);
                var topFilmBegenilen = y.begenme++;

                var puan = x.Puan += Puan(1, 8);
                lr.SaveChanges();
                return "false";
            }
        }

        [HttpPost]
        public string DahaSonraIzle(dahasonraizle dhs, uDetay ud, Filmler fl,string imdb_id)
        {
            var kontrolId = lr.dahasonraizles.Any(x => x.imdb_id == dhs.imdb_id && x.UserId == dhs.UserId);
            if (kontrolId)
            {
                var izBul = lr.uDetays.Find(ud.UserId);
                dahasonraizle d_h_s = lr.dahasonraizles.FirstOrDefault(x => x.imdb_id == imdb_id && x.UserId == ud.UserId);
                lr.dahasonraizles.Remove(d_h_s);
                var topIzlenen = izBul.topDahaSonraIzle--;

                Filmler f = lr.Filmlers.FirstOrDefault(z => z.imdb_id == imdb_id);
                var a = f.dahasonraizle--;
                lr.SaveChanges();
                return "true";
            }
            else
            {
                lr.dahasonraizles.Add(dhs);
                var x = lr.uDetays.Find(ud.UserId);
                var topDahaSonraIzle = x.topDahaSonraIzle++;

                var y = lr.Filmlers.Find(fl.TID);
                var topFilmDHS = y.dahasonraizle++;

                var puan = x.Puan += Puan(1, 8);
                lr.SaveChanges();
                return "false";
            }
        }

        [HttpPost]
        public string YorumEkle(Yorumlar y, uDetay ud, Filmler fl)
        {

            if (y.YorumIcerik != null)
            {
                y.YorumTarih = DateTime.Now;
                y.durum = false;
                lr.Yorumlars.Add(y);
                var x = lr.uDetays.Find(ud.UserId);
                var topYorum = x.topYorum++;

                var fy = lr.Filmlers.Find(fl.TID);
                var topFilmYorumlar = fy.yorumlar++;
                lr.SaveChanges();
                return "true";
            }
            else
            {
                return "false";
            }


        }

    }
}