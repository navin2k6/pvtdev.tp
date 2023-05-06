using System;
using System.Collections.Generic;


namespace TalentsProfile.BAL.Admin
{
    public interface IOrganization
    {
        /// <summary>
        /// Basic information of arganozation to save.
        /// </summary>
        /// <param name="email">User account email id, pass empty.</param>
        /// /// <param name="tocken">User tocken id, pass empty.</param>
        /// <param name="name">Name of organization</param>
        /// <param name="address">Address of registrying organization</param>
        /// <param name="city">City of registrying organization</param>
        /// <param name="country">Country of registrying organization</param>
        /// <param name="uri">Web Uri of organization</param>
        /// <param name="logoUri">Logo Uri of organization</param>
        /// <param name="functionalArea">Functional/service area of organozation</param>
        /// <param name="category">Category of organization - direct company or hiring agency etc.</param>
        /// <param name="sector">Sector of organization - public/private/government etc.</param>
        /// <param name="profile">Short profile of organization</param>
        /// <returns>Success status</returns>
        bool AddOrganization(string email, string tocken, string name, string address, string city, int country, string uri, string logoUri,
                             int functionalArea, string category, string sector, string profile);


        /// <summary>
        /// Returns list of organizations, if parameter is null.
        /// Returns single entity w.r.t value, if parameter passed.
        /// </summary>
        /// <param name="orgId">Organization identity</param>
        /// <returns></returns>
        List<OrganizationDetail> GetOrganization(Int32? orgId);


        /// <summary>
        /// Returns list of data records, displayed under highlight section
        /// </summary>
        /// <returns>List of key-value pair. Key denotes particular and value denotes corresponding value.</returns>
        List<KeyValuePair<string, Int32>> GetHighlights();
    }
}
