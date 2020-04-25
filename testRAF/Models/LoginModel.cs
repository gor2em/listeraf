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
        public string UserName { get; set; }

        [RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", ErrorMessage = "Email adresi bulunamadı.")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Geçerli bir mail giriniz!")]
        [Display(Name = "Email")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Email alanı boş bırakılamaz*")]
        public string Email { get; set; }


        [Display(Name = "Şifre")]
        [Required(ErrorMessage ="Şifrenizi giriniz*")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}