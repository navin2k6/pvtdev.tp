using System;
using System.Web;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Collections.Generic;
using TalentsProfile.DTO;
using System.IO;


namespace TalentsProfile.BAL
{
    /// <summary>
    /// Class to manage physical files.
    /// </summary>
    public abstract class FileManager
    {
        // Get the path to store resume:
        string FilePath
        {
            get
            {
                if (FileType.Equals(ResourceType.Resume))
                    return System.Web.HttpContext.Current.Server.MapPath("~" + ConfigurationManager.AppSettings["Profiles"].ToString());
                else if (FileType.Equals(ResourceType.ProfilePic))
                    return System.Web.HttpContext.Current.Server.MapPath("~" + ConfigurationManager.AppSettings["ProfilePic"].ToString());
                else
                    return null;
            }
        }

        // Type of reource uploading:
        protected ResourceType FileType
        {
            get;
            set;
        }


        /// <summary>
        /// Save file into physical location.
        /// </summary>
        /// <param name="fileObj"></param>
        /// <param name="fileName"></param>
        protected string SaveResume(HttpPostedFileBase fileObj, string fileName)
        {
            if (fileObj == null)
                throw new ArgumentException("NO_FILE");

            string url = string.Empty;

            if (FilePath != null)
            {
                url = FilePath + fileName;
                fileObj.SaveAs(url);
            }

            return url;
        }
    }
}
