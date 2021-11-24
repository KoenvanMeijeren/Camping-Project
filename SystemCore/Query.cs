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
    public class Query
    {

        private readonly SqlCommand _sqlCommand;

        public Query(string query)
        {
            this._sqlCommand = new SqlCommand(query);
        }

        public void AddParameter(string parameter, object value)
        {
            this._sqlCommand.Parameters.AddWithValue(parameter, value);
        }
        
        public IEnumerable<Dictionary<string, string>> Select()
        {
            if (!this._sqlCommand.CommandText.Contains("SELECT"))
            {
                throw new ArgumentException("SQL query must contain a SELECT query in order to execute this method.");
            }
            
            List<Dictionary<string, string>> list = new List<Dictionary<string, string>>();
            
            DatabaseConnector.Open();
            this._sqlCommand.Connection = DatabaseConnector.GetConnection();
            
            using SqlDataReader reader = this._sqlCommand.ExecuteReader();
            while (reader.Read())
            {
                list.Add(this.DataRecordToDictionary(reader));
            }

            DatabaseConnector.Close();
            
            return list;
        }


        public Dictionary<string, string> SelectFirst()
        {
            if (!this._sqlCommand.CommandText.Contains("SELECT"))
            {
                throw new ArgumentException("SQL query must contain a SELECT query in order to execute this method.");
            }
            
            DatabaseConnector.Open();
            this._sqlCommand.Connection = DatabaseConnector.GetConnection();
            
            using SqlDataReader reader = this._sqlCommand.ExecuteReader();
            reader.Read();
            Dictionary<string, string> dictionary = this.DataRecordToDictionary(reader);
            
            DatabaseConnector.Close();

            return dictionary;
        }

        public void Update() {}

        public void Delete() { }


        private Dictionary<string, string> DataRecordToDictionary(IDataRecord dataRecord)
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
