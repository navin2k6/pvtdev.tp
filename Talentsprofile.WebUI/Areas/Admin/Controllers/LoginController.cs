using System.Web;
using System.IO;
using System.Web.Mvc;
using TalentsProfile.DTO;
using Arcmatics.Authentication;

namespace Talentsprofile.WebUI.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Index(Login user)
        {
            if (!ModelState.IsValid)
                return View(user);


            Membership mem = new SystemUser();
            Arcmatics.Authentication.UserTicket userTkt = mem.AuthenticateUser(user.Email, user.Password);
            HttpCookie ck = new HttpCookie("talentsprofile_user",
                                           string.Format("{0},{1},{2},{3}", userTkt.Name, userTkt.Email, userTkt.UType, userTkt.Tocken));
            HttpContext.Response.Cookies.Add(ck);
            HttpContext.Session["talentsprofile_user"] = userTkt.Tocken;
            return new RedirectResult("/adm/dashboard");
        }
    }
}