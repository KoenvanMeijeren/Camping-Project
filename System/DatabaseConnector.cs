using System;
using System.Data;
using System.Data.SqlClient;

namespace SystemCore
{
    public static class DatabaseConnector
    {
        private static SqlConnection _connection;

        private static void Initialize()
        {
            try 
            { 
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.DataSource = "127.0.0.1";
                builder.UserID = "SA";            
                builder.Password = "Klimaatverandering1";     
                builder.InitialCatalog = "TestDB";

                DatabaseConnector._connection = new SqlConnection(builder.ConnectionString);
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public static SqlConnection GetConnection()
        {
            return DatabaseConnector._connection;
        }

        public static void Close()
        {
            if(DatabaseConnector._connection?.State == ConnectionState.Open)
            {
                DatabaseConnector._connection.Close();
            }
        }

        public static void Open()
        {
            if(DatabaseConnector._connection == null || DatabaseConnector._connection?.State == ConnectionState.Closed)
            {
                DatabaseConnector.Initialize();
            }

            DatabaseConnector._connection.Open();
        }
    }
}