using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Collections.Generic;

namespace Talentsprofile.Admin.Controllers
{
    public class CorporateController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
    }
}