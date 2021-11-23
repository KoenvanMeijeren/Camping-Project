using System;
using System.Data.SqlClient;

namespace System
{
    class DatabaseConnector
    {
        public static void Connect()
        {
            try 
            { 
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.DataSource = "127.0.0.1";
                builder.UserID = "SA";            
                builder.Password = "Klimaatverandering1";     
                builder.InitialCatalog = "TestDB";

                using SqlConnection connection = new SqlConnection(builder.ConnectionString);
                Console.WriteLine("\nQuery data example:");
                Console.WriteLine("=========================================\n");

                String sql = "SELECT name, quantity FROM Inventory";

                using SqlCommand command = new SqlCommand(sql, connection);
                connection.Open();
                
                using SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Console.WriteLine("{0} {1}", reader.GetString(0), reader.GetInt32(1));
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }
            Console.ReadLine();
        }
    }
}