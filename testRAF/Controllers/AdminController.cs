using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using testRAF.App_Classes;
using testRAF.Models.Entity;
using testRAF.ViewModels;
using PagedList;
namespace testRAF.Controllers
{
    [Authorize(Roles = "A")]
    public class AdminController : Controller
    {
        // GET: Admin

        listerafDBEntities4 lr = new listerafDBEntities4();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Uyeler(int page=1)
        {
            var model = lr.Users.ToList().ToPagedList(page,10);
            return View(model);
        }

        public ActionResult uRol(int id)
        {
            var rolGetir = lr.Users.Find(id);//sorgudan geleni değeri taşıdık dropdwona göndereceğiz.
            List<SelectListItem> roller = (from i in lr.Rollers.ToList()
                                           select new SelectListItem
                                           {
                                               Text = i.RolAd,
                                               Value = i.RolId.ToString()
                                           }).ToList();

            ViewBag.rolDeger = roller;
            return View(rolGetir);
        }


        public ActionResult rolGuncelle(User r)
        {
            var kullanici = lr.Users.Find(r.UserId);
            kullanici.UserName = r.UserName;
            //urun.Email = r.Email;


            var rolSecilen = lr.Rollers.Where(m => m.RolId == r.Roller.RolId).FirstOrDefault();
            kullanici.RolId = rolSecilen.RolId;

            lr.SaveChanges();
            return RedirectToAction("Uyeler");
        }


        public ActionResult uyeSil(int id)
        {
            //User u = lr.Users.FirstOrDefault(x=>x.UserId==id);
            //lr.Users.Remove(u);
            //try
            //{
            //    lr.SaveChanges();
            //    return "true";
            //}
            //catch (Exception)
            //{
            //    return "false";
            //}

            var urun = lr.Users.Find(id);
            lr.Users.Remove(urun);
            lr.SaveChanges();
            return RedirectToAction("Uyeler");

        }

        public ActionResult Listeler()
        {
            return View();
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Listeler(listeler l, HttpPostedFileBase fileUpload)
        {
            if (fileUpload != null)
            {
                Image img = Image.FromStream(fileUpload.InputStream);
                int orta_w = Convert.ToInt32(ConfigurationManager.AppSettings["mw"].ToString());
                int orta_h = Convert.ToInt32(ConfigurationManager.AppSettings["mh"].ToString());

                //int buyuk_w = Convert.ToInt32(ConfigurationManager.AppSettings["hw"].ToString());
                //int buyuk_h = Convert.ToInt32(ConfigurationManager.AppSettings["hh"].ToString());

                string ortayol = "/src/img/listeler/ortayol/" + Guid.NewGuid() +
                Path.GetExtension(fileUpload.FileName);

                //string buyukyol = "/src/img/listeler/buyukyol/" + Guid.NewGuid() +
                //Path.GetExtension(fileUpload.FileName);


                Bitmap o_bm = new Bitmap(img, orta_w, orta_h);
                o_bm.Save(Server.MapPath(ortayol));
                //Bitmap b_bm = new Bitmap(img, buyuk_w, buyuk_h);
                //b_bm.Save(Server.MapPath(buyukyol));


                listeResim lresim = new listeResim();
                lresim.ListeOrtaYol = ortayol;
                //lresim.ListeBuyukYol = buyukyol;
                lr.listeResims.Add(lresim);
                lr.SaveChanges();

                l.LResimId = lresim.LResimId;
                l.ListeTarih = DateTime.Now;
                l.begenme = 0;
                l.goruntulenme = 0;
                int userId = lr.Users.FirstOrDefault(x => x.UserName == User.Identity.Name).UserId;//giriş  yapmış kullanıcı alındı
                l.UserId = userId;
                lr.listelers.Add(l);
                lr.SaveChanges();

            }
            return RedirectToAction("Index");
        }


        public ActionResult Yorumlar(int? page)
        {
            var model = lr.Yorumlars.OrderByDescending(x => x.YorumId).ToList().ToPagedList(page ?? 1, 10);
            return View(model);
        }

        [HttpPost]
        public string yorumSil(int id)
        {
            Yorumlar y = lr.Yorumlars.FirstOrDefault(x => x.YorumId == id);
            lr.Yorumlars.Remove(y);

            var yorumBul = lr.uDetays.Find(y.UserId);
            var topYorum = yorumBul.topYorum--;
            var puan = yorumBul.Puan -= 5;
            try
            {
                lr.SaveChanges();
                return "true";
            }
            catch (Exception)
            {
                return "false";
            }
        }
        [HttpPost]
        public string yorumGuncelle(int id)
        {
            FilmlerController FC = new FilmlerController();
            var onaylanacakYorumId = lr.Yorumlars.Find(id);
            onaylanacakYorumId.durum = true;

            var x = lr.uDetays.Find(onaylanacakYorumId.UserId);
            var onayPuan = x.Puan += FC.Puan(1, 15);

            try
            {
                lr.SaveChanges();
                return "true";
            }
            catch (Exception)
            {
                return "false";
            }

        }



    }
}