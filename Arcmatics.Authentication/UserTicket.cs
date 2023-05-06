using System;
using System.Runtime.Serialization;

namespace Arcmatics.Authentication
{
    /// <summary>
    /// Define a user ticket that should be issued after successfull authentication.
    /// </summary>
    [Serializable]
    public class UserTicket : ISerializable, IDisposable
    {
        private string _email = string.Empty;
        private string _tocken = string.Empty;
        private string _name = string.Empty;
        private string _uType = string.Empty;
        //private string _id = string.Empty;

        /// <summary>
        /// Registered email id.
        /// </summary>
        public String Email { get { return _email; } }

        /// <summary>
        /// Current request tocken id.
        /// </summary>
        public String Tocken
        {
            get { return _tocken; }
            set { _tocken = value; }
        }

        /// <summary>
        /// Name of registered user.
        /// </summary>
        public String Name { get { return _name; } }

        /// <summary>
        /// User account type in curent system.
        /// </summary>
        public String UType { get { return _uType; } }

        /// <summary>
        /// User identity number.
        /// </summary>
        //public String Id { get { return _id; } }


        /// <summary>
        /// Serialize object data.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Email", Email);
            info.AddValue("Tocken", Tocken);
            info.AddValue("Name", Name);
            info.AddValue("UType", UType);
            //info.AddValue("Id", Id);
        }


        /// <summary>
        /// Deserialize object data.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public UserTicket(SerializationInfo info, StreamingContext context)
        {
            _email = (String)info.GetValue("Email", typeof(string));
            _tocken = (String)info.GetValue("Tocken", typeof(string));
            _name = (String)info.GetValue("Name", typeof(string));
            _uType = (String)info.GetValue("UType", typeof(string));
            //_id = (String)info.GetValue("Id", typeof(string));
        }


        /// <summary>
        /// Initialize the object.
        /// </summary>
        public UserTicket(string email, string tocken, string name, string uType)
        {
            _email = email;
            _tocken = tocken;
            _name = name;
            _uType = UType;
            //_id = id;
        }


        public void Dispose()
        {
        }
    }


    /// <summary>
    /// Returns current logged-in user's tocken.
    /// </summary>
    public class GetUSId
    {
        public string GetUserTocken()
        {
            try
            {
                Cryptography crypt = new Cryptography();
                string[] tocken = (crypt.DecryptString(System.Web.HttpContext.Current.Session["talentsprofile_user"].ToString())).Split('-');
                return tocken[1];
            }
            catch
            {
                throw new ArgumentException("Invalid user");
            }
        }
    }
}
