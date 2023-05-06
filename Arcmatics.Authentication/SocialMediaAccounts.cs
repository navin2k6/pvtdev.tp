using System;

namespace Arcmatics.Authentication
{
    /// <summary>
    /// Define types of users, allowed to login using social account.
    /// </summary>
    public enum SocialMediaAccountType
    {
        /// <summary>
        /// For user registered with the current application.
        /// </summary>
        System = 0,

        /// <summary>
        /// For all Google accounts.
        /// </summary>
        Google = 1,

        /// <summary>
        /// For LinkedIn registered users.
        /// </summary>
        LinkedIn = 2,
    }
}
