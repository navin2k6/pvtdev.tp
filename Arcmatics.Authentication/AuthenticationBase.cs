using System.Web;

namespace Arcmatics.Authentication
{
    /// <summary>
    /// Define base class as abstract & inherit it to add details.
    /// </summary>
    public abstract class Membership
    {
        protected short _userTypeValue;
        protected string _userTypeName;

        /// <summary>
        /// Assign current session id from Http request.
        /// </summary>
        protected readonly string Tocken = HttpContext.Current.Session.SessionID;

        protected abstract void CheckUser();
        protected abstract void SaveUser();
        protected abstract UserTicket AuthorizeUser(string userId, string password, string tocken);

        /// <summary>
        /// Validate user ticket and return values.
        /// </summary>
        /// <param name="cookie"></param>
        /// <returns></returns>
        public abstract void ValidateUser(HttpCookie cookie);

        public UserTicket AuthenticateUser(string userId, string password)
        {
            //CheckUser();
            //SaveUser();
            return AuthorizeUser(userId, password, this.Tocken);
        }


    }
}
