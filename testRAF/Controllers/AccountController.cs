using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using testRAF.Models.Entity;
using testRAF.Models;
using testRAF.ViewModels;
namespace testRAF.Controllers
{
    //ALLOWANONYMOUS: global asax a authorize özelliğini verdik ama tüm sayfaları engelledi o yüzden  buraya allowoanonymous yazdık

    public class AccountController : Controller
    {
        listerafDBEntities4 objUserDBEntities = new listerafDBEntities4();
        myViewModel MVW = new myViewModel();
        // GET: Account

        [AllowAnonymous]
        public ActionResult Register()
        {
            UserModel objUserModel = new UserModel();
            return View(objUserModel);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Register(UserModel objUserModel)
        {
            if (ModelState.IsValid)
            {
                if (!objUserDBEntities.Users.Any(m => m.UserName == objUserModel.UserName.ToLower() || m.Email == objUserModel.Email.ToLower()))
                {
                    User objUser = new Models.Entity.User();

                    objUser.UserName = objUserModel.UserName.ToLower();
                    objUser.Email = objUserModel.Email.ToLower();
                    objUser.Password = objUserModel.Password;
                    objUser.CreatedOn = DateTime.Now;
                    objUser.RolId = 1;

                    //uDetay
                    uDetay userDetay = new Models.Entity.uDetay();
                    userDetay.UserId = objUserModel.UserId;
                    userDetay.topIzlenen = 0;
                    userDetay.topBegenilen = 0;
                    userDetay.topDahaSonraIzle = 0;
                    userDetay.topYorum = 0;
                    userDetay.Puan = 0;
                    userDetay.RutbeId = 1;
                    
                    objUserDBEntities.uDetays.Add(userDetay);



                    objUserDBEntities.Users.Add(objUser);
                    objUserDBEntities.SaveChanges();
                    objUserModel = new UserModel();

                    //objUserModel.SuccessMessage = "Kayıt işlemi başarılı";
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    ModelState.AddModelError("Error", "Email veya kullanıcı adı alınmış");
                    return View();
                }

            }
            return View();


        }
        [AllowAnonymous]
        public ActionResult Login()
        {
            LoginModel objLoginModel = new LoginModel();
            
            return View(objLoginModel);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(LoginModel objLoginModel)
        {
            if (ModelState.IsValid)
            {
                var logUser = objUserDBEntities.Users.FirstOrDefault(x => x.UserName == objLoginModel.UserName.ToLower() && x.Password == objLoginModel.Password);
                if (logUser != null)
                {
                    FormsAuthentication.SetAuthCookie(logUser.UserName, false);//kullanıcı sisteme kabul edildi -- auth oldu
                    Session["UserName"] = objLoginModel.UserName.ToLower();
                    Session["UserId"] = logUser.UserId;
                    ViewBag.userid = Session["UserId"];


                    if (logUser.RolId == 2)
                       return RedirectToAction("Index", "Admin",MVW);
                    
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("Error", "Geçersiz kullanıcı adı veya şifre!");
                    return View();
                }

            }
            return View();
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }




    }
}