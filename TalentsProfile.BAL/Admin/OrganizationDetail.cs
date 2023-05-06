using System;

namespace TalentsProfile.BAL.Admin
{
    /// <summary>
    /// Defines organization basic information to manage an organization data.
    /// </summary>
    public class OrganizationDetail
    {
        /// <summary>
        /// Organization id to update a record. 
        /// </summary>
        public Int32? Id { get; set; }

        /// <summary>
        /// Name of organization
        /// </summary>
        public string OrganizationName { get; set; }

        /// <summary>
        /// Address of organization
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// City of organization
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Country id of organization
        /// </summary>
        public int Country { get; set; }

        /// <summary>
        /// Category of organization, e.g. CO/CONST etc
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Website of organization
        /// </summary>
        public string Website { get; set; }

        /// <summary>
        /// Logo Uri of organization
        /// </summary>
        public string CompLogo { get; set; }

        /// <summary>
        /// Summary profile of organization
        /// </summary>
        public string CompProfile { get; set; }

        /// <summary>
        /// Functional area of organization
        /// </summary>
        public string DomainId { get; set; }

        /// <summary>
        /// Sector of organization, e.g. government/private/public etc.
        /// </summary>
        public string Sector { get; set; }
    }
}
