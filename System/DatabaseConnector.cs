using System;
using System.Data;
using System.Data.SqlClient;

namespace SystemCore
{
    public class DatabaseConnector
    {
        private static SqlConnection connection;

        public static void Connect()
        {
            try 
            { 
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.DataSource = ConfigReader.GetSetting("UserID");
                builder.UserID = ConfigReader.GetSetting("Password");            
                builder.Password = ConfigReader.GetSetting("NameDB");     
                builder.InitialCatalog = ConfigReader.GetSetting("ApplicationName");

                connection = new SqlConnection(builder.ConnectionString);
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public static SqlConnection GetConnection()
        {
            return connection;
        }

        public static void Close()
        {
            if(connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
        }

        public static void Open()
        {
            if(connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }
        }
    }
}