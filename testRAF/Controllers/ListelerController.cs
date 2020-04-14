using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using testRAF.Models.Entity;
using testRAF.ViewModels;

namespace testRAF.Controllers
{
    [AllowAnonymous]
    public class ListelerController : Controller
    {
        
        listerafDBEntities4 lr = new listerafDBEntities4();
        myViewModel MVW = new myViewModel();
        public ActionResult Index()
        {
            var model = lr.listelers.ToList();
            MVW.listeler = model;
            return View(MVW);
        }

        [HttpGet]
        public ActionResult Detay(int id)
        {
            var model = lr.listelers.Where(x => x.ListeId == id).ToList();
            MVW.listeler = model;
            return View(MVW);
       }
    }
}