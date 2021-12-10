using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using SystemCore;

namespace Model
{
    /// <summary>
    /// Provides a base model for interacting with database tables. Provides all behavior for interacting with database
    /// tables, such as selecting, inserting, updating and deleting. Further more it provides behavior for rendering a
    /// database record to a model and vice versa.
    /// </summary>
    /// <typeparam name="T">A model class, representing a record within a database table.</typeparam>
    public abstract class ModelBase<T> : IModel
    {
        public int Id { get; protected init; }
        
        protected List<T> Collection = new List<T>();

        protected readonly string Table, PrimaryKey;
        
        /// <summary>
        /// Constructs a new model base object.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="primaryKey">The primary key of the table.</param>
        protected ModelBase(string table, string primaryKey)
        {
            this.Table = table;
            this.PrimaryKey = primaryKey;
        }

        /// <summary>
        /// Selects all records from table.
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<T> Select()
        {
            Query query = new Query(this.BaseSelectQuery());

            this.Collection = new List<T>();
            var items = query.Select();
            foreach (Dictionary<string, string> dictionary in items)
            {
                this.Collection.Add(this.ToModel(dictionary));
            }

            return this.Collection;
        }
        
        /// <summary>
        /// Selects one record by id from table.
        /// </summary>
        /// <param name="id">Numeric value used as identifier of row.</param>
        /// <returns>The selected model.</returns>
        public virtual T SelectById(int id)
        {
            Query query = new Query(this.SelectQuery());
            query.AddParameter(this.PrimaryKey, id.ToString());

            return this.ToModel(query.SelectFirst());
        }

        /// <summary>
        /// Selects the last record from the database table, this is done by ordering by the primary key column.
        /// </summary>
        /// <returns>The last record of the database table.</returns>
        public virtual T SelectLast()
        {
            Query query = new Query(this.SelectLastQuery());

            return this.ToModel(query.SelectFirst());
        }

        /// <summary>
        /// Inserts one record into the database table.
        /// </summary>
        /// <returns>Whether the query was successful or not.</returns>
        public virtual bool Insert()
        {
            Dictionary<string, string> dictionary = this.ToDictionary();
            StringBuilder values = new StringBuilder();
            StringBuilder columns = new StringBuilder();
            
            string lastKey = dictionary.Last().Key;
            foreach (var key in dictionary.Select(keyValuePair => keyValuePair.Key))
            {
                columns.Append(key.Equals(lastKey) ? $"{key}" : $"{key}, ");
                values.Append(key.Equals(lastKey) ? $"@{key}" : $"@{key}, ");
            }
            
            Query query = new Query($"INSERT INTO {this.Table} ({columns}) VALUES ({values})");
            foreach (KeyValuePair<string,string> keyValuePair in dictionary)
            {
                query.AddParameter(keyValuePair.Key, keyValuePair.Value);
            }
            
            query.Execute();

            return query.IsSuccessFullyExecuted();
        }

        /// <summary>
        /// Updates one record from the database table.
        /// </summary>
        /// <returns>Whether the query was successful or not.</returns>
        public virtual bool Update(Dictionary<string, string> dictionary)
        {
            StringBuilder values = new StringBuilder();
            
            string lastKey = dictionary.Last().Key;
            foreach (var key in dictionary.Select(keyValuePair => keyValuePair.Key))
            {
                values.Append(key.Equals(lastKey) ? $"{key} = @{key}" : $"{key} = @{key}, ");
            }
            
            Query query = new Query($"UPDATE {this.Table} SET {values} WHERE {this.PrimaryKey} = @{this.PrimaryKey}");
            query.AddParameter(this.PrimaryKey, this.Id);
            foreach (KeyValuePair<string,string> keyValuePair in dictionary)
            {
                query.AddParameter(keyValuePair.Key, keyValuePair.Value);
            }
            
            query.Execute();

            return query.IsSuccessFullyExecuted();
        }


        /// <summary>
        /// Deletes one record from the database table.
        /// </summary>
        /// <returns>Whether the query was successful or not.</returns>
        public virtual bool Delete()
        {
            Query query = new Query($"DELETE FROM {this.Table} WHERE {this.PrimaryKey} = @{this.PrimaryKey}");
            query.AddParameter(this.PrimaryKey, this.Id.ToString());
            
            query.Execute();

            return query.IsSuccessFullyExecuted();
        }
        
        /// <summary>
        /// Renders a dictionary to model, used for strongly typing the data from the database record.
        /// </summary>
        /// <param name="dictionary">One row of a table stored in a dictionary.</param>
        /// <returns>The model created from the dictionary.</returns>
        protected abstract T ToModel(Dictionary<string, string> dictionary);

        /// <summary>
        /// Renders data from database row to a dictionary or a model to a dictionary for inserting or updating the data
        /// in the database table.
        /// </summary>
        /// <returns>The dictionary, keyed by column and the corresponding value.</returns>
        protected abstract Dictionary<string, string> ToDictionary();

        /// <summary>
        /// Provides a base query used for all other queries which interacts with the database.
        /// </summary>
        /// <returns>The base query for selecting data. (E.g. SELECT * FROM TABLE)</returns>
        protected virtual string BaseSelectQuery()
        {
            return $"SELECT * FROM {this.Table} BT";
        }

        /// <summary>
        /// Provides a query for selecting one record from table by ID.
        /// </summary>
        /// <returns>The query for getting one record from table by ID.</returns>
        protected virtual string SelectQuery()
        {
            return $"{this.BaseSelectQuery()} WHERE {this.PrimaryKey} = @{this.PrimaryKey}";
        }
        
        /// <summary>
        /// Provides a query for selecting the last record from the database table. This is commonly used for getting
        /// the previous inserted record.
        /// </summary>
        /// <returns>The query for getting the last record from table.</returns>
        protected virtual string SelectLastQuery()
        {
            return $"{this.BaseSelectQuery()} ORDER BY {this.PrimaryKey} DESC";
        }

    }
}