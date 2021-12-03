using System;
using System.Data;
using System.Data.SqlClient;

namespace SystemCore
{
    /// <summary>
    /// Provides a class for creating a connection with the database.
    /// </summary>
    public static class DatabaseConnector
    {
        private static SqlConnection _connection;

        /// <summary>
        /// Initializes the database connection.
        /// </summary>
        private static void Initialize()
        {
            try 
            { 
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder
                {
                    DataSource = ConfigReader.GetSetting("DBSource"),
                    UserID = ConfigReader.GetSetting("DBUser"),
                    Password = ConfigReader.GetSetting("DBPassword"),
                    InitialCatalog = ConfigReader.GetSetting("DBName")
                };

                DatabaseConnector._connection = new SqlConnection(builder.ConnectionString);
            }
            catch (SqlException e)
            {
                SystemError.Handle(e);
            }
        }
        
        /// <summary>
        /// Opens the databases connection.
        /// </summary>
        public static void Open()
        {
            if (DatabaseConnector._connection?.State == ConnectionState.Open)
            {
                return;
            }
            
            if(DatabaseConnector._connection == null)
            {
                DatabaseConnector.Initialize();
            }

            try
            {
                DatabaseConnector._connection.Open();
            }
            catch (Exception exception)
            {
                SystemError.Handle(exception);
            }
        }

        /// <summary>
        /// Gets the connection with the database.
        /// </summary>
        /// <returns>Database connection</returns>
        public static SqlConnection GetConnection()
        {
            return DatabaseConnector._connection;
        }

        /// <summary>
        /// Closes the database connection.
        /// </summary>
        public static void Close()
        {
            if(DatabaseConnector._connection?.State == ConnectionState.Open)
            {
                DatabaseConnector._connection.Close();
            }
        }
    }
}