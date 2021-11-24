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
    /// <summary>
    /// Provides a class for executing SQL statements.
    /// </summary>
    public class Query
    {

        private readonly SqlCommand _sqlCommand;

        private bool _success;

        /// <summary>
        /// Inserts string query in SqlCommand.
        /// </summary>
        public Query(string query)
        {
            this._sqlCommand = new SqlCommand(query);
        }

        /// <summary>
        /// Adds parameters to the SqlCommand.
        /// </summary>
        public void AddParameter(string parameter, object value)
        {
            this._sqlCommand.Parameters.AddWithValue(parameter, value);
        }
        
        /// <summary>
        /// Selects rows from SqlCommand. 
        /// </summary>
        /// <returns>List with chosen rows</returns>
        public IEnumerable<Dictionary<string, string>> Select()
        {
            if (!this._sqlCommand.CommandText.Contains("SELECT"))
            {
                this._success = false;
                SystemError.Handle(new ArgumentException("SQL query must contain a SELECT query in order to execute this method."));
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

            this._success = true;

            return list;
        }

        /// <summary>
        /// Select first row from SqlCommand.
        /// </summary>
        /// <returns>Dictionary with first row from the SELECT statement</returns>
        public Dictionary<string, string> SelectFirst()
        {
            if (!this._sqlCommand.CommandText.Contains("SELECT"))
            {
                this._success = false;
                SystemError.Handle(new ArgumentException("SQL query must contain a SELECT query in order to execute this method."));
            }
            
            DatabaseConnector.Open();
            this._sqlCommand.Connection = DatabaseConnector.GetConnection();
            
            using SqlDataReader reader = this._sqlCommand.ExecuteReader();
            reader.Read();
            Dictionary<string, string> dictionary = this.DataRecordToDictionary(reader);
            
            DatabaseConnector.Close();

            this._success = true;
            
            return dictionary;
        }

        /// <summary>
        /// Executes the SqlCommand.
        /// </summary>
        public void Execute()
        {
            DatabaseConnector.Open();

            this._sqlCommand.Connection = DatabaseConnector.GetConnection();
            var updatedRows = this._sqlCommand.ExecuteNonQuery();
            
            DatabaseConnector.Close();

            this._success = updatedRows > 0;
        }

        /// <summary>
        /// Checks succes.
        /// </summary>
        /// <returns>Succes status</returns>
        public bool SuccessFullyExecuted()
        {
            return this._success;
        }
        
        /// <summary>
        /// Inserts DataRecord items in a Dictionary.
        /// </summary>
        /// <returns>Filled Dictionary</returns>
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
