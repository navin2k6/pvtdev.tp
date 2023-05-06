using System;
using System.Web;
using System.Data;
using System.Reflection;
using Arcmatics.TalentsProfile.DAL;


namespace Arcmatics.Authentication
{
    /// <summary>
    /// Default user or system registered users.
    /// </summary>
    public class SystemUser : Membership
    {
        protected override void CheckUser() { }
        protected override void SaveUser() { }
        protected override UserTicket AuthorizeUser(string userId, string password, string tocken)
        {
            DataProvider objProvider = new DataProvider();
            DataTable user = objProvider.GetDataTable(AseCommandGenerator.GenerateCommand((MethodInfo)MethodBase.GetCurrentMethod(),
                                    new object[] { userId, password, base.Tocken }, ""), string.Empty);

            UserTicket ticket = null;
            if (user != null && user.Rows.Count > 0)
            {
                ticket = new UserTicket(user.Rows[0]["Email"].ToString(), user.Rows[0]["Tocken"].ToString(),
                                 user.Rows[0]["Name"].ToString(), user.Rows[0]["UType"].ToString());
                //user.Rows[0]["Id"].ToString());

                base._userTypeName = user.Rows[0]["UType"].ToString();
            }
            else
                throw new ArgumentException("NOT_EXISTS");

            Authentication.Cryptography crypt = new Cryptography();
            ticket.Tocken = crypt.EncryptString(string.Format("{0}-{1}", user.Rows[0]["Id"].ToString(), user.Rows[0]["Tocken"].ToString()));

            return ticket;
        }

        /// <summary>
        /// Validate user ticket and return values.
        /// </summary>
        /// <param name="cookie"></param>
        /// <returns></returns>
        public override void ValidateUser(HttpCookie cookie)
        {
            try
            {
                if (cookie == null || string.IsNullOrEmpty(cookie.Value))
                {
                    throw new System.Exception("Invalid_User");
                }
                else
                {
                    string[] user = cookie.Value.Split(',');
                    //Cryptography crypt = new Cryptography();
                    //string[] tocken = (crypt.DecryptString(user[3])).Split('-');
                    //if (HttpContext.Current.Session.SessionID.Equals(tocken[1]))

                    if(HttpContext.Current.Session["talentsprofile_user"].Equals(user[3]))
                    {
                        // TODO: User is valid
                    }
                    else
                    {
                        throw new System.Exception("Invalid_User");
                    }
                }
            }
            catch
            {
                throw new System.InvalidOperationException("Invalid_User");
            }
        }
    }
}
