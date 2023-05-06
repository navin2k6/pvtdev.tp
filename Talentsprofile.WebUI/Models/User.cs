using System;
using System.IO;
using System.Web;
using TalentsProfile.DTO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web.Mvc;

namespace TalentsProfile
{
    public static class HttpUser
    {
        /// <summary>
        /// Read and convert binary cookie to UserTicket object.
        /// </summary>
        /// <returns></returns>
        public static UserTicket ReadUsercookie()
        {
            HttpCookie ck = HttpContext.Current.Request.Cookies["talentsprofileUser"];
            if (ck != null && !string.IsNullOrEmpty(ck.Value))
            {
                byte[] arrTicket = Convert.FromBase64String(ck.Value);
                BinaryFormatter formater = new BinaryFormatter();
                MemoryStream stream = new MemoryStream(arrTicket);
                UserTicket ticket = (UserTicket)formater.Deserialize(stream);
                return ticket;
            }
            else
                return null;
        }
    }


    /// <summary>
    /// Check for user's logged-in session.
    /// </summary>
    public class UserSessionAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //base.OnActionExecuting(filterContext);
            HttpCookie ck = HttpContext.Current.Request.Cookies["talentsprofileUser"];
            if (ck == null || string.IsNullOrEmpty(ck.Value))
                filterContext.Result = new RedirectResult("../../login");
        }
    }
}