using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using testRAF.Class;
using testRAF.ViewModels;
using testRAF.Models;
using testRAF.Models.Entity;
namespace testRAF.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        myViewModel MVW = new myViewModel();
        listerafDBEntities4 lr = new listerafDBEntities4();
        public ActionResult Index(string mN, int? page)
        {
            if (page != null)
                CallAPI(mN, Convert.ToInt32(page));

            TheMovieDb theMovieDb = new TheMovieDb();
            theMovieDb.movieName = mN;

            MVW.filmler = theMovieDb;
            return View(MVW);
        }

   

        [HttpGet]
        public ActionResult Index(TheMovieDb theMovieDb, string movieName)
        {
            if (ModelState.IsValid)
            {
                CallAPI(movieName, 0);
            }
            //.Where(a => a.durum == true)
            var yorumlar = lr.Yorumlars.OrderByDescending(x => x.YorumId).Where(a => a.durum == true).Take(5).ToList();
            var z = lr.uDetays.OrderByDescending(o => o.Puan).Take(6).ToList();
            var son3list = lr.listelers.OrderByDescending(l=>l.ListeId).Take(4).ToList();
            var enCokIzlenen = lr.Filmlers.OrderByDescending(i=>i.izlenme).Take(6).ToList();
            var rastgeleFilm = lr.Filmlers.OrderBy(u => Guid.NewGuid()).Take(1).ToList();

            MVW.pop_filmler = enCokIzlenen;
            MVW.rastgele_film = rastgeleFilm;
            MVW.listeler = son3list;
            MVW.udetay = z;
            MVW.filmler = theMovieDb;
            MVW.yorumlarim = yorumlar;
            return View(MVW);
        }
        public void CallAPI(string movieName, int page)
        {
            int pageNo = Convert.ToInt32(page) == 0 ? 1 : Convert.ToInt32(page);

            /*Calling API https://developers.themoviedb.org/3/movie/top_rated */
            string apiKey = "a64ffa7c8877a661d43e9a9c0896f2c8";
            HttpWebRequest apiRequest = WebRequest.Create("https://api.themoviedb.org/3/movie/top_rated?api_key=" + apiKey + "&language=en-US&query=" + movieName + "&page=" + pageNo + "&include_adult=false") as HttpWebRequest;

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

            /*http://json2csharp.com*/
            testResponseMovies rootObject = JsonConvert.DeserializeObject<testResponseMovies>(apiResponse);

            StringBuilder sb = new StringBuilder();
            sb.Append("<div class=\"container\">");
            sb.Append("<h2>En yüksek puanlı filmler</h2>");
            foreach (testResult result in rootObject.results)
            {
                string image = result.poster_path == null ? Url.Content("~/Content/Image/no-image.png") : "https://image.tmdb.org/t/p/w500/" + result.poster_path;
                string link = Url.Action("Detay", "Filmler", new { id = result.id });
                sb.Append("<div class=\"col-lg-2 imgnp\" resourceId=\"" + result.id + "\">" + "<a href=\"" + link + "\"><img src=\"" + image + "\" class=\"img-responsive imgH\" />" + "<p>" + result.title + "</a></p></div>");
            }
            ViewBag.Result = sb.ToString();

            int pageSize = 24;
            PagingInfo pagingInfo = new PagingInfo();
            pagingInfo.CurrentPage = pageNo;
            pagingInfo.TotalItems = rootObject.total_results;
            pagingInfo.ItemsPerPage = pageSize;
            ViewBag.Paging = pagingInfo;
        }
    }
}