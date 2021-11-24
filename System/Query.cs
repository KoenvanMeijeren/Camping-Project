using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Server;

namespace SystemCore
{
    public static class Query
    {
        public static IEnumerable<Dictionary<string, string>> Select(SqlCommand sqlCommand)
        {
            List<Dictionary<string, string>> list = new List<Dictionary<string, string>>();
            
            DatabaseConnector.Open();
            sqlCommand.Connection = DatabaseConnector.GetConnection();
            
            using SqlDataReader reader = sqlCommand.ExecuteReader();
            while (reader.Read())
            {
                list.Add(Query.DataRecordToDictionary(reader));
            }

            DatabaseConnector.Close();
            
            return list;
        }
        
        public static void Update() { }

        public static void Delete() { }

        private static Dictionary<string, string> DataRecordToDictionary(IDataRecord dataRecord)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();

            for (int delta = 0; delta < dataRecord.FieldCount; delta++)
            {
                string dataType = dataRecord.GetDataTypeName(delta);
                string column = dataRecord.GetName(delta);

                string value = dataType switch
                {
                    "string" => dataRecord.GetString(delta),
                    "nvarchar" => dataRecord.GetString(delta),
                    "int" => dataRecord.GetInt32(delta).ToString(),
                    "bool" => dataRecord.GetBoolean(delta).ToString(),
                    _ => throw new InvalidEnumArgumentException(
                        $"The data type: {dataType} is not supported yet to read data from database records.")
                };
                    
                dictionary.Add(column, value);
            }

            return dictionary;
        }
    }
}
