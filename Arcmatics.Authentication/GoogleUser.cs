using System;
using System.Web;

namespace Arcmatics.Authentication
{
    /// <summary>
    /// Defines login mechanism for Google based account holder.
    /// </summary>
    public class GoogleUser : Membership
    {
        public short UserType { get { return base._userTypeValue; } }
        public string UserTypeName { get { return base._userTypeName; } }
        public string Id { get; set; }
        public string Email { get; set; }
        public bool Verified_Email { get; set; }
        public string Name { get; set; }
        public string Given_Name { get; set; }
        public string Family_Name { get; set; }
        public string Link { get; set; }
        public string Picture { get; set; }
        public string Gender { get; set; }
        public string Locale { get; set; }

        protected override void CheckUser() { }
        protected override void SaveUser() { }
        protected override UserTicket AuthorizeUser(string userId, string password, string tocken)
        {
            return null;
        }

        public override void ValidateUser(HttpCookie cookie)
        {
            throw new Exception("Not implemented");
        }
    }
}
