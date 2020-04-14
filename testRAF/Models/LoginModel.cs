using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace testRAF.Models
{
    public class LoginModel
    {

        public int UserId { get; set; }

        [Display(Name ="Kullanıcı Adı")]
        [Required(ErrorMessage ="Kullanıcı adınızı giriniz*")]
        public string UserName { get; set; }

        [Display(Name ="Şifre")]
        [Required(ErrorMessage ="Şifrenizi giriniz*")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}