using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using testRAF.Class;
using testRAF.Models.Entity;

namespace testRAF.Controllers
{
    public class DizilerController : Controller
    {
        // GET: Diziler
        
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
            string apiKey = "a64ffa7c8877a661d43e9a9c0896f2c8";
            HttpWebRequest apiRequest = WebRequest.Create("https://api.themoviedb.org/3/tv/top_rated?api_key=" + apiKey + "&language=tr&query=" + movieName + "&page=" + pageNo + "&include_adult=false") as HttpWebRequest;

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
    }
}