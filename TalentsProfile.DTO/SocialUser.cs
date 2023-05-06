using System;
//using TalentsProfile.DTO;

namespace TalentsProfile.DTO
{
    public abstract class SocialUser
    {
        // Social user should be always jobseeker.
        protected readonly short _userTypeValue;
        protected readonly string _userTypeName;
        public abstract SocialAccountType UserSocialAccountType { get; }

        protected SocialUser()
        {
            _userTypeValue = (short)(UserType.JobSeeker);
            _userTypeName = UserType.JobSeeker.ToString();
        }
    }

    /// <summary>
    /// User of Google account type.
    /// </summary>
    public class GoogleUser : SocialUser
    {
        public short UserType { get { return base._userTypeValue; } }
        public string UserTypeName { get { return base._userTypeName; } }
        public override SocialAccountType UserSocialAccountType { get { return SocialAccountType.Google; } }
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
    }

    /// <summary>
    /// User of Linkedin account type.
    /// </summary>
    public class LinkedInUser : SocialUser
    {
        public short UserType { get { return base._userTypeValue; } }
        public string UserTypeName { get { return base._userTypeName; } }
        public override SocialAccountType UserSocialAccountType { get { return SocialAccountType.LinkedIn; } }
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
    }


    /// <summary>
    /// Default user or Talentsprofile registered users.
    /// </summary>
    public class Talentsprofile : SocialUser
    {
        public override SocialAccountType UserSocialAccountType
        {
            get { return SocialAccountType.Default; }
        }
    }
}
