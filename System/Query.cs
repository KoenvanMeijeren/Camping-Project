using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemCore
{
    public static class Query
    {
        public static List<string> Select(string query)
        {
            List<string> list = new List<string>();
            using (DatabaseConnector.GetConnection())
            {
                using (SqlCommand command = new SqlCommand(query, DatabaseConnector.GetConnection()))
                {
                    DatabaseConnector.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(reader.GetString(1));
                        }
                    }
                }
            }
            return list;
        }

        public static void Update() { }

        public static void Delete() { }

        private static void Execute()
        {
        }
    }
}
