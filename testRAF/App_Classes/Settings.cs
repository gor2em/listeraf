using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;

namespace testRAF.App_Classes
{
    public class Settings
    {
        public static Size listeOrtaYol
        {
            get
            {
                Size sonuc = new Size();
                sonuc.Width = Convert.ToInt32(ConfigurationManager.AppSettings["mw"]);
                sonuc.Height = Convert.ToInt32(ConfigurationManager.AppSettings["mh"]);
                return sonuc;
            }
        }
        public static Size listeBuyukYol
        {
            get
            {
                Size sonuc = new Size();
                sonuc.Width = Convert.ToInt32(ConfigurationManager.AppSettings["hw"]);
                sonuc.Height = Convert.ToInt32(ConfigurationManager.AppSettings["hh"]);
                return sonuc;
            }
        }
    }
}