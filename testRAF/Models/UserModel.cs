using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace testRAF.Models
{
    public class UserModel
    {
        public int UserId { get; set; }


        [MinLength(3,ErrorMessage ="Kullanıcı adınız en az 3 karakterden oluşmalıdır!")]
        [MaxLength(20,ErrorMessage ="Kullanıcı adınız en fazla 20 karakter içerebilir!")]
        [Required(AllowEmptyStrings =false, ErrorMessage = "Kullanıcı adı alanı boş bırakılamaz*")]
        [Display(Name ="Kullanıcı Adı")]
        public string UserName { get; set; }

        [RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", ErrorMessage = "Email adresi bulunamadı.")]
        [DataType(DataType.EmailAddress,ErrorMessage ="Geçerli bir mail giriniz!")]
        [Display(Name = "Email")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Email alanı boş bırakılamaz*")]
        public string Email { get; set; }

        [Display(Name = "Şifre")]
        
        [Required(AllowEmptyStrings = false, ErrorMessage = "Şifre alanı boş bırakılamaz*")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Şifre Tekrar")]
        [DataType(DataType.Password)]
        //[MinLength(7, ErrorMessage = "Şifreniz en az 7 karakterden oluşmalıdır*")]
        [Compare("Password",ErrorMessage ="Şifreler eşleşmiyor!")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Şifre tekrar alanı boş bırakılamaz*")]
        public string ConfirmPassword { get; set; }

        public DateTime CreatedOn { get; set; }
        public string Rol { get; set; }

    }
}