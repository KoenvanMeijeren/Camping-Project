using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
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
        /// Constructs the query.
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
        /// Selects rows from database table. 
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

            try
            {
                DatabaseConnector.Open();
                this._sqlCommand.Connection = DatabaseConnector.GetConnection();

                using SqlDataReader reader = this._sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(this.DataRecordToDictionary(reader));
                }

                DatabaseConnector.Close();

                this._success = true;
            }
            catch (Exception exception)
            {
                this._success = false;
                
                SystemError.Handle(exception);
            }
            

            return list;
        }

        /// <summary>
        /// Select first row from database table.
        /// </summary>
        /// <returns>Dictionary with first row from the SELECT statement</returns>
        public Dictionary<string, string> SelectFirst()
        {
            if (!this._sqlCommand.CommandText.Contains("SELECT"))
            {
                this._success = false;
                SystemError.Handle(new ArgumentException("SQL query must contain a SELECT query in order to execute this method."));
            }

            Dictionary<string, string> dictionary = new Dictionary<string, string>();

            try
            {
                DatabaseConnector.Open();
                this._sqlCommand.Connection = DatabaseConnector.GetConnection();
            
                using SqlDataReader reader = this._sqlCommand.ExecuteReader();
                if (reader == null)
                {
                    return null;
                }
            
                bool hasData = reader.Read();
                if (!hasData)
                {
                    return null;
                }
            
                dictionary = this.DataRecordToDictionary(reader);
            
                DatabaseConnector.Close();

                this._success = true;
            }
            catch (Exception exception)
            {
                this._success = false;
                
                SystemError.Handle(exception);
            }
            
            return dictionary;
        }

        /// <summary>
        /// Executes the SqlCommand and saves the success status.
        /// </summary>
        public void Execute()
        {
            try
            {
                DatabaseConnector.Open();

                this._sqlCommand.Connection = DatabaseConnector.GetConnection();
                var updatedRows = this._sqlCommand.ExecuteNonQuery();

                DatabaseConnector.Close();

                this._success = updatedRows > 0;
            }
            catch (Exception exception)
            {
                this._success = false;
                
                SystemError.Handle(exception);
            }
        }

        /// <summary>
        /// Determines if the query has been successfully executed.
        /// </summary>
        /// <returns>Whether the query was successful or not.</returns>
        public bool IsSuccessFullyExecuted()
        {
            return this._success;
        }
        
        /// <summary>
        /// Inserts DataRecord items in a Dictionary. Renders all kind of data types to one single data type, we do this
        /// because this makes it easy for us to use the most common data type, string, and we only have to cast the
        /// less common data types, such as int or bool. If we do not do this, then we have to save it as an object type
        /// and we have to cast every single the corresponding data type every time we use it.
        /// </summary>
        /// <returns>Filled Dictionary</returns>
        private Dictionary<string, string> DataRecordToDictionary(IDataRecord dataRecord)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();

            for (int delta = 0; delta < dataRecord.FieldCount; delta++)
            {
                string dataType = dataRecord.GetDataTypeName(delta);
                string column = dataRecord.GetName(delta);
                if (dictionary.ContainsKey(column))
                {
                    continue;
                }
                
                string value = dataType switch
                {
                    "text" => dataRecord.GetString(delta),
                    "string" => dataRecord.GetString(delta),
                    "varchar" => dataRecord.GetString(delta),
                    "nvarchar" => dataRecord.GetString(delta),
                    "int" => dataRecord.GetInt32(delta).ToString(),
                    "tinyint" => dataRecord.GetByte(delta).ToString(),
                    "float" => dataRecord.GetDouble(delta).ToString(CultureInfo.InvariantCulture),
                    "double" => dataRecord.GetDouble(delta).ToString(CultureInfo.InvariantCulture),
                    "decimal" => dataRecord.GetDecimal(delta).ToString(CultureInfo.InvariantCulture),
                    "bool" => dataRecord.GetBoolean(delta).ToString(),
                    "date" => dataRecord.GetDateTime(delta).ToString(CultureInfo.InvariantCulture),
                    "datetime" => dataRecord.GetDateTime(delta).ToString(CultureInfo.InvariantCulture),
                    _ => throw new InvalidEnumArgumentException(
                        $"The data type: {dataType} is not supported yet to read data from database records.")
                };
                    
                dictionary.Add(column, value);
            }

            return dictionary;
        }
    }
}
