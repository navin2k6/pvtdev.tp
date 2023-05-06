using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Collections.Generic;

namespace Talentsprofile.WebUI
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            // Manage MVC Captcha settings
            var captchaManager = (CaptchaMvc.Infrastructure.DefaultCaptchaManager)CaptchaMvc.Infrastructure.CaptchaUtils.CaptchaManager;
            captchaManager.CharactersFactory = () => System.Configuration.ConfigurationManager.AppSettings["CaptchaKey"].ToString();

            captchaManager.PlainCaptchaPairFactory = length =>
            {
                string randomText = CaptchaMvc.Infrastructure.RandomText.Generate(captchaManager.CharactersFactory(), length);
                bool ignoreCase = false;
                return new KeyValuePair<string, CaptchaMvc.Interface.ICaptchaValue>(Guid.NewGuid().ToString("N"),
                    new CaptchaMvc.Models.StringCaptchaValue(randomText, randomText, ignoreCase));
            };
            captchaManager.AddAreaRouteValue = false;
            //
        }


        protected void Application_Error(object sender, EventArgs e)
        {
            //var err = Server.GetLastError();

            //if ((err as HttpException)?.GetHttpCode() == 404)
            //{
            //    Server.ClearError();
            //    Response.StatusCode = 404;
            //}

            //Exception ex = Server.GetLastError();
            //Response.Clear();
            //HttpException httpEx = ex as HttpException;
            //string action = string.Empty;

            //if (httpEx != null)
            //{
            //    switch (httpEx.GetHttpCode())
            //    {
            //        case 500:
            //            action = "";
            //            break;
            //        default:
            //            break;
            //    }
            //}

            //Server.ClearError();
            //Response.Redirect("~/error/InternalSerErr");

            // Check for user trying access area:
            //if (HttpContext.Current.Request.Url.ToString().Contains("/adm/"))
            //    new RedirectResult("/adm/login");
            //else
            //    new RedirectResult("/");
        }
    }
}
