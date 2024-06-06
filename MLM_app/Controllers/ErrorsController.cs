using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MLM_app.Controllers
{
    public class ErrorsController : BaseController
    {
        #region
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        #endregion

        #region 
        [HttpGet]
        public ActionResult NotFound()
        {
            return View();
        }
        #endregion

        #region 
        [HttpGet]
        public ActionResult Forbidden()
        {
            return View();
        }
        #endregion

        #region General
        [HttpGet]
        public ActionResult General()
        {
            return View();
        }
        #endregion

        #region ServerError
        [HttpGet]
        public ActionResult ServerError()
        {
            return View();
        }
        #endregion

    }
}