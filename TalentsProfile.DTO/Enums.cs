using System;

namespace TalentsProfile.DTO
{
    /// <summary>
    /// Define the types of probable users.
    /// </summary>
    public enum UserType
    {
        User = 0, // Anonymous user.
        JobSeeker = 1,
        Employer = 2,
    }


    /// <summary>
    /// Define the types of probable users.
    /// </summary>
    public enum JobStatus
    {
        Draft = 1,
        Running,
        Paused,
        Deleted,
        Expired,
    }


    /// <summary>
    /// Type of action to perform.
    /// </summary>
    public enum UserAction
    {
        Add = 1,
        Update,
        Get,
        Delete,
        Edit,
    }

    /// <summary>
    /// Type of list to be bind.
    /// </summary>
    public enum ListType
    {
        COUNTRY = 1,
        MARITAL_STATUS,
        VISA_STATUS,
        PASSPORT_STATUS,
    }

    public enum NoticePeriod
    {
        Immediate = 0,
        Upto_15_Days,
        In_Notice_Period,
        Upto_30_Days,
        Upto_45_Days,
        Upto_60_Days,
        Upto_75_Days,
        Upto_90_Days,
        Above_90_Days,
    }

    public enum ResourceType
    {
        Resume = 1,
        ProfilePic,
    }

    public enum SocialAccountType
    {
        Default = 0,
        Google,
        LinkedIn,
    }
}
