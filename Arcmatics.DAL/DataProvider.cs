using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;

namespace Arcmatics.TalentsProfile.DAL
{
    public sealed class DataProvider
    {
        /// Protect connection class from out-side world.
        /// 
        /// <summary>
        /// Initialise database connection for public users.
        /// </summary>
        /// <returns>String</returns>
        private SqlConnection GetDbConnection()
        {
            String _connection = string.Empty;
            string path = string.Empty;
            System.Web.UI.Page page = new System.Web.UI.Page();
            path = page.Server.MapPath("~");

            if (File.Exists(path + "/Web.config"))
                _connection = ConfigurationManager.ConnectionStrings["User"].ToString();

            SqlConnection connection = new SqlConnection(_connection);
            return connection;
        }


        /// <summary>
        /// Initialise database connection for registered users.
        /// </summary>
        /// <returns>String</returns>
        private SqlConnection GetDbConnection(string uType)
        {
            String _connection = string.Empty;
            string path = string.Empty;
            System.Web.UI.Page page = new System.Web.UI.Page();
            path = page.Server.MapPath("~");

            if (File.Exists(path + "/Web.config"))
                _connection = ConfigurationManager.ConnectionStrings[uType].ToString();

            SqlConnection connection = new SqlConnection(_connection);
            return connection;
        }

        ///


        /// <summary>
        /// This method returns dataset object filled with tabular data w.r.t. command
        /// </summary>
        /// <param name="command">Sql command having procedures and parameters</param>
        /// <returns></returns>
        public DataSet GetDataSet(SqlCommand command, string uType)
        {
            if (String.IsNullOrEmpty(uType))
                command.Connection = GetDbConnection();
            else
                command.Connection = GetDbConnection(uType);

            DataSet _ds = new DataSet();
            SqlDataAdapter _da = new SqlDataAdapter(command);
            _da.Fill(_ds);
            return _ds;
        }


        /// <summary>
        /// This method returns data table object filled with record set w.r.t. command
        /// </summary>
        /// <param name="command">Sql command having procedures and parameters</param>
        /// <returns></returns>
        public DataTable GetDataTable(SqlCommand command, string uType)
        {
            if (String.IsNullOrEmpty(uType))
                command.Connection = GetDbConnection();
            else
                command.Connection = GetDbConnection(uType);

            DataSet _ds = new DataSet();
            SqlDataAdapter _da = new SqlDataAdapter(command);
            _da.Fill(_ds);

            if (_ds != null && _ds.Tables.Count > 0)
                return _ds.Tables[0];
            else
                return null;
        }


        /// <summary>
        /// This method returns single value after execution of command
        /// </summary>
        /// <param name="command">Sql command having procedures and parameters</param>
        /// <returns></returns>
        public string ExecuteScalar(SqlCommand command, string uType)
        {
            if (String.IsNullOrEmpty(uType))
                command.Connection = GetDbConnection();
            else
                command.Connection = GetDbConnection(uType);

            string value = string.Empty;

            try
            {
                command.Connection.Open();
                value = Convert.ToString(command.ExecuteScalar());
            }
            catch (Exception ex)
            {
                command.Connection.Close();
                command.Connection.Dispose();
                throw new ArgumentException(string.Format("Error: {0} at: {1}", ex.Message, ex.StackTrace));
            }
            finally
            {
                command.Connection.Close();
                command.Connection.Dispose();
            }

            return value;
        }


        /// <summary>
        /// This method returns no value after execution of command
        /// </summary>
        /// <param name="command">Sql command having procedures and parameters</param>
        /// <returns></returns>
        public void ExecuteQuery(SqlCommand command, string uType)
        {
            if (String.IsNullOrEmpty(uType))
                command.Connection = GetDbConnection();
            else
                command.Connection = GetDbConnection(uType);

            try
            {
                command.Connection.Open();
                command.ExecuteNonQuery();
            }
            catch { }
            finally
            {
                command.Connection.Close();
                command.Connection.Dispose();
            }
        }


        /// <summary>
        /// This method returns single value after execution of command.
        /// It needs open connection to managa transaction rollback.
        /// </summary>
        /// <param name="command">Sql command having procedures and parameters</param>
        /// /// <param name="returnValue">Default value false.</param>
        /// <returns></returns>
        public string ExecuteTransactions(SqlCommand command, Boolean returnValue, string uType)
        {
            if (String.IsNullOrEmpty(uType))
                command.Connection = GetDbConnection();
            else
                command.Connection = GetDbConnection(uType);

            if (returnValue)
            {
                string value = string.Empty;
                value = Convert.ToString(command.ExecuteScalar());
                return value;
            }
            else
            {
                command.ExecuteNonQuery();
                return "-1";
            }
        }
    }
}
