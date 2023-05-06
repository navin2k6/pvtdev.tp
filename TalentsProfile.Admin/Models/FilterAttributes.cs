using System.Web;
using System.Web.Mvc;
using Arcmatics.Authentication;

namespace Talentsprofile.Admin.Models
{
    public class CheckUserSessionAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try
            {
                SystemUser sysUser = new SystemUser();
                sysUser.ValidateUser(HttpContext.Current.Request.Cookies["talentsprofile_user"]);
            }
            catch
            {
                HttpContext.Current.Response.Cookies.Clear();
                filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary { { "controller","login"},
                                                                                                               {"action","index" } });

            }
        }
    }

    public class AuthenticateExceptionAttribute : FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {

        }
    }
}