using System;
using System.Web;

namespace Arcmatics.Authentication
{
    /// <summary>
    /// User of Linkedin account type.
    /// </summary>
    public class LinkedInUser : Membership
    {
        public short UserType { get { return base._userTypeValue; } }
        public string UserTypeName { get { return base._userTypeName; } }
        public string Id { get; set; }
        public string EmailAddress { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Headline { get; set; }
        public string PictureUrl { get; set; }
        public string PublicProfileUrl { get; set; }
        public dynamic Location { get; set; }

        /// <summary>
        /// City name
        /// </summary>
        public string Name { get; set; }
        public string Summary { get; set; }
        public string Specialties { get; set; }
        public string Industry { get; set; }
        public dynamic Positions { get; set; }
        public dynamic ApiStandardProfileRequest { get; set; }


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
