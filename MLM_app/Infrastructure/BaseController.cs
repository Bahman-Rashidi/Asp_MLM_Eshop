using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MLM_app.Infrastructure
{

    //Setting [Display] attribute by global resources!
    /// Note (BUG):
    /// 1. You should change the class (Captions) access modifire to public!
    /// 2. You should change the property (Such as Emai, EmailConfirm,...) access modifier to public!
    public class BaseController : Controller
    {
        public BaseController()
        {

            System.Globalization.CultureInfo oCultureInfo =
                new System.Globalization.CultureInfo("en-US");




            System.Threading.Thread.CurrentThread.CurrentCulture = oCultureInfo;
            System.Threading.Thread.CurrentThread.CurrentUICulture = oCultureInfo;
        }

        //#################################################


    }
}