using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using testRAF.Models;
using testRAF.Models.Entity;

namespace testRAF.Controllers
{
    public class testController : Controller
    {
        listerafDBEntities4 lr = new listerafDBEntities4();
        // GET: test
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult FilmGetir(string q)
        {
            var film = (from p in lr.Filmlers where p.title.Contains(q) select p.title).Distinct().Take(10);

            string content = string.Join<string>("\n", film);
            return Content(content);
        }
    }
}