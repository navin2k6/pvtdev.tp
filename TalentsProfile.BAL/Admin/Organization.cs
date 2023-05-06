using System;
using System.Web;
using System.Linq;
using System.Data;
using System.Reflection;
using System.Collections.Generic;
using Arcmatics.TalentsProfile.DAL;
using Arcmatics.Authentication;

namespace TalentsProfile.BAL.Admin
{
    public class Organization : IOrganization, IDisposable
    {
        DataProvider objProvider = null;
        string _email = string.Empty;
        readonly string _tocken;

        /// <summary>
        /// Initialize default & common variables and objects.
        /// </summary>
        public Organization(HttpCookie cookie)
        {
            GetUSId usID = new GetUSId();
            _tocken = usID.GetUserTocken();
            string[] user = cookie.Value.Split(',');
            _email = user[1];
            objProvider = new DataProvider();
        }

        public void Dispose()
        {
        }


        public bool AddOrganization(string email, string tocken, string name, string address, string city, int country, string uri, string logoUri,
                                    int functionalArea, string category, string sector, string profile)
        {
            string status = objProvider.ExecuteScalar(AseCommandGenerator.GenerateCommand((MethodInfo)MethodBase.GetCurrentMethod(),
                            new object[] { _email, _tocken, name, address, city, country, uri, logoUri, functionalArea, category, sector, profile }, ""),
                            string.Empty);

            if (status.Equals("OK"))
                return true;
            else
                return false;
        }


        public List<OrganizationDetail> GetOrganization(Int32? orgId)
        {
            DataTable tbl = objProvider.GetDataTable(AseCommandGenerator.GenerateCommand((MethodInfo)MethodBase.GetCurrentMethod(),
                                      new object[] { orgId }, ""),
                                      string.Empty);

            return new List<OrganizationDetail>(from row in tbl.AsEnumerable()
                                                select new OrganizationDetail
                                                {
                                                    Id = Convert.ToInt32(row["OrgId"]),
                                                    OrganizationName = row["OrganizationName"].ToString(),
                                                    City = row["City"].ToString(),
                                                    Category = row["Category"].ToString(),
                                                    Sector = row["Sector"].ToString()
                                                });

        }


        public List<KeyValuePair<string, Int32>> GetHighlights()
        {
            DataTable tbl = objProvider.GetDataTable(AseCommandGenerator.GenerateCommand((MethodInfo)MethodBase.GetCurrentMethod(),
                                      new object[] { }, ""),
                                      string.Empty);

            return new List<KeyValuePair<string, Int32>>(from row in tbl.AsEnumerable()
                                                         select new KeyValuePair<string, Int32>
                                                         (row["Keys"].ToString(),
                                                          Convert.ToInt32(row["Value"])
                                                         ));
        }
    }
}
