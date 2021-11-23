using System;
using System.Data.SqlClient;

namespace System
{
    class DatabaseConnector
    {
        private static SqlConnection connection;
        public static void Connect()
        {
            try 
            { 
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.DataSource = "127.0.0.1";
                builder.UserID = "SA";            
                builder.Password = "Klimaatverandering1";     
                builder.InitialCatalog = "TestDB";

                connection = new SqlConnection(builder.ConnectionString);
                connection.Open();
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
            try
            {
                connection.Close();
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}