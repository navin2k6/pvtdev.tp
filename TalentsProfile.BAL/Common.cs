/* Author: Navin Pandit
 * Purpose: Talentsprofile.com
*/

using System;
using System.IO;
using System.Linq;
using System.Data;
using System.Text;
using TalentsProfile.DTO;
using System.Reflection;
using Arcmatics.TalentsProfile.DAL;
using System.Collections.Generic;


namespace TalentsProfile.BAL
{
    public static class Common
    {
        /// <summary>
        /// Bind list DDL w.r.t. type passed.
        /// </summary>
        /// <param name="type">ListType</param>
        /// <returns>List of text-vlue pairs</returns>
        public static List<ListLookup> GetListValue(string type)
        {
            DataProvider objProvider = new DataProvider();
            DataTable tblList = objProvider.GetDataTable(AseCommandGenerator.GenerateCommand((MethodInfo)MethodBase.GetCurrentMethod(),
                            new object[] { type }, ""), string.Empty);

            List<ListLookup> list = null;
            if (tblList != null)
                list = new List<ListLookup>(
                    from types in tblList.AsEnumerable()
                    select (new ListLookup()
                    {
                        Id = int.Parse(types["Id"].ToString()),
                        Description = types["Description"].ToString()
                    }));

            list.Insert(0, new ListLookup() { Id = 0, Description = "Select" });
            return list;
        }


        /// <summary>
        /// Writes the error in specified text file. Path must be defined in .config file.
        /// </summary>
        /// <param name="ex"></param>
        public static void LogError(Exception ex)
        {
            string path = System.Web.HttpContext.Current.Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["ErrorLog"].ToString());

            StreamWriter writer = File.AppendText(path);
            writer.WriteLine("Date: " + DateTime.Now.ToString());
            writer.WriteLine("\nError: " + ex.Message);
            writer.WriteLine("\nSource: " + ex.Source);
            writer.WriteLine("\nStack trace: " + ex.StackTrace);
            writer.WriteLine("\n-----------------------------------------------------------------------------------------------------------------\n");
            writer.Close();
            writer.Dispose();
        }
    }
}
