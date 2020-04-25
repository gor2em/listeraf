using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using testRAF.Models;
using testRAF.Models.Entity;
using testRAF.ViewModels;
using PagedList;

namespace testRAF.Controllers
{

    public class UserController : Controller
    {

        // GET: User
        listerafDBEntities4 lr = new listerafDBEntities4();
        myViewModel MVW = new myViewModel();

        public ActionResult Profil(int id)
        {
            var model = lr.Users.Find(id);
            var ud = lr.uDetays.Where(x => x.UserId == id).ToList();
            var detaylar = lr.uDetays.Find(id);
            ViewBag.id = id;

            if (model.UserId == id)
            {
                var x = detaylar.RutbeId;
                StringBuilder sb = new StringBuilder();
                if (model.RolId == 2)
                {
                    x = detaylar.RutbeId = 7; lr.SaveChanges();
                    sb.Append("<p><b class='r-admin'>" + detaylar.Rutbeler.RutbeAd + "</b></p>");
                }
                else
                {
                    if (detaylar.Puan >= 0 && detaylar.Puan <= 99 || detaylar.Puan < 0)
                    {
                        x = detaylar.RutbeId = 1; lr.SaveChanges();
                        sb.Append("<p><b>" + detaylar.Rutbeler.RutbeAd + "</b></p>");
                    }
                    else if (detaylar.Puan >= 100 && detaylar.Puan <= 149)
                    {
                        x = detaylar.RutbeId = 2; lr.SaveChanges();
                        sb.Append("<p><b class='r-acemi'>" + detaylar.Rutbeler.RutbeAd + "</b></p>");
                    }
                    else if (detaylar.Puan >= 150 && detaylar.Puan <= 199)
                    {
                        x = detaylar.RutbeId = 3; lr.SaveChanges();
                        sb.Append("<p><b class='r-yarisci'>" + detaylar.Rutbeler.RutbeAd + "</b></p>");
                    }
                    else if (detaylar.Puan >= 200 && detaylar.Puan <= 249)
                    {
                        x = detaylar.RutbeId = 4; lr.SaveChanges();
                        sb.Append("<p><b class='r-yildiz'>" + detaylar.Rutbeler.RutbeAd + "</b></p>");
                    }
                    else if (detaylar.Puan >= 250 && detaylar.Puan <= 299)
                    {
                        x = detaylar.RutbeId = 5; lr.SaveChanges();
                        sb.Append("<p> <b class='r-asil'>" + detaylar.Rutbeler.RutbeAd + "</b></p>");
                    }
                    else
                    {
                        x = detaylar.RutbeId = 6; lr.SaveChanges();
                        sb.Append("<p><b class='r-efsane'>" + detaylar.Rutbeler.RutbeAd + "</b></p>");
                    }

                }
                ViewBag.Rutbe = sb.ToString();
            }
            MVW.udetay = ud;
            MVW.UD = detaylar;
            MVW.user = model;
            return View(MVW);
        }
        [HttpPost]
        public ActionResult Profil(HttpPostedFileBase fileUpload, int id)
        {
            var x = lr.Users.Find(id);
            var y = lr.uDetays.Find(id);
            int resimId = -1;
            if (fileUpload != null)
            {
                Image img = Image.FromStream(fileUpload.InputStream);
                int width = Convert.ToInt32(ConfigurationManager.AppSettings["pWidth"].ToString());
                int height = Convert.ToInt32(ConfigurationManager.AppSettings["pHeight"].ToString());

                string name = "/src/pResim/" + Guid.NewGuid() +
                Path.GetExtension(fileUpload.FileName);//resmin kendi uzantısını al \jpg png yazmak yerine

                Bitmap bm = new Bitmap(img, width, height);
                bm.Save(Server.MapPath(name));

                resimler rsm = new resimler();
                rsm.OrtaYol = name;
                rsm.UserId = x.UserId;
                lr.resimlers.Add(rsm);
                lr.SaveChanges();
                if (rsm.ResimId != null)
                    resimId = rsm.ResimId;
            }
            if (resimId != -1)
            {
                x.ResimId = resimId;
                y.ResimId = resimId;
            }
            //lr.Users.Add(x);
            var uResim = x.ResimId;
            var udResim = y.ResimId;

            lr.SaveChanges();
            return RedirectToAction("Profil");
        }
        public JsonResult Izlediklerim(int id,Filmler fl)
        {
            //var veriler = lr.izlenenlers.Where(x => x.UserId == id).ToList().ToPagedList(page??1,36);
            var veriler = lr.izlenenlers.Where(x => x.UserId == id).ToList();
            return Json(
                new
                {
                    Result = from obj in veriler
                             select new
                             {
                                 obj.id,
                                 obj.title,
                                 obj.poster_path,
                                 obj.imdb_id
                             }
                }, JsonRequestBehavior.AllowGet
                );
        }
        public JsonResult Begendiklerim(int id)
        {

            var veriler = lr.begenilenlers.Where(x => x.UserId == id).ToList();
            return Json(
                new
                {
                    Result = from obj in veriler
                             select new
                             {
                                 obj.id,
                                 obj.title,
                                 obj.poster_path,
                                 obj.imdb_id
                             }
                }, JsonRequestBehavior.AllowGet
                );
        }
        public JsonResult DahaSonraIzle(int id)
        {

            var veriler = lr.dahasonraizles.Where(x => x.UserId == id).ToList();
            return Json(
                new
                {
                    Result = from obj in veriler
                             select new
                             {
                                 obj.id,
                                 obj.title,
                                 obj.poster_path,
                                 obj.imdb_id
                             }
                }, JsonRequestBehavior.AllowGet
                );
        }
        public JsonResult Yorumlar(int id)
        {
            var veriler = lr.Yorumlars.Where(x => x.UserId == id && x.durum == true).ToList();
            return Json(
                new
                {
                    Result = from obj in veriler
                             select new
                             {
                                 obj.YorumIcerik,
                                 obj.imdb_id,
                                 obj.User.UserName,
                                 obj.YorumTarih,
                                 obj.id,
                                 obj.title
                             }
                }, JsonRequestBehavior.AllowGet
                );
        }

        [HttpPost]
        public string IzSil(string imdb_id, uDetay ud)
        {
            var izBul = lr.uDetays.Find(ud.UserId);
            izlenenler iz = lr.izlenenlers.FirstOrDefault(x => x.imdb_id == imdb_id && x.UserId == ud.UserId);
            lr.izlenenlers.Remove(iz);
            var topIzlenen = izBul.topIzlenen--;

            Filmler f = lr.Filmlers.FirstOrDefault(z=>z.imdb_id==imdb_id);
            var a = f.izlenme--;
            //var y = lr.Filmlers.Find(imdb_id);
            //var topFilmIzlenen = y.izlenme--;

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
        public string BeSil(string imdb_id, uDetay ud)
        {
            var izBul = lr.uDetays.Find(ud.UserId);
            begenilenler be = lr.begenilenlers.FirstOrDefault(x => x.imdb_id == imdb_id && x.UserId == ud.UserId);
            lr.begenilenlers.Remove(be);
            var topIzlenen = izBul.topBegenilen--;

            Filmler f = lr.Filmlers.FirstOrDefault(z => z.imdb_id == imdb_id);
            var a = f.begenme--;

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
        public string DHSSil(string imdb_id, uDetay ud)
        {
            var izBul = lr.uDetays.Find(ud.UserId);
            dahasonraizle dhs = lr.dahasonraizles.FirstOrDefault(x => x.imdb_id == imdb_id && x.UserId == ud.UserId);
            lr.dahasonraizles.Remove(dhs);
            var topIzlenen = izBul.topDahaSonraIzle--;

            Filmler f = lr.Filmlers.FirstOrDefault(z => z.imdb_id == imdb_id);
            var a = f.dahasonraizle--;

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