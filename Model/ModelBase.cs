using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SystemCore;

namespace Model
{
    public abstract class ModelBase<T> : IModel
    {
        public int Id { get; protected init; }
        
        protected List<T> Collection = new List<T>();

        protected abstract string Table();
        
        protected abstract string PrimaryKey();
        
        /// <summary>
        /// Selects all records from table.
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<T> Select()
        {
            Query query = new Query(this.BaseQuery());

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
            query.AddParameter(this.PrimaryKey(), id.ToString());

            return this.ToModel(query.SelectFirst());
        }
        
        public virtual T SelectLast()
        {
            Query query = new Query(this.SelectLastQuery());

            return this.ToModel(query.SelectFirst());
        }

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
            
            Query query = new Query($"INSERT INTO {this.Table()} ({columns}) VALUES ({values})");
            foreach (KeyValuePair<string,string> keyValuePair in dictionary)
            {
                query.AddParameter(keyValuePair.Key, keyValuePair.Value);
            }
            
            query.Execute();

            return query.IsSuccessFullyExecuted();
        }

        public virtual bool Update(Dictionary<string, string> dictionary)
        {
            StringBuilder values = new StringBuilder();
            
            string lastKey = dictionary.Last().Key;
            foreach (var key in dictionary.Select(keyValuePair => keyValuePair.Key))
            {
                values.Append(key.Equals(lastKey) ? $"{key} = @{key}" : $"{key} = @{key}, ");
            }
            
            Query query = new Query($"UPDATE {this.Table()} SET {values} WHERE {this.PrimaryKey()} = @{this.PrimaryKey()}");
            query.AddParameter(this.PrimaryKey(), this.Id);
            foreach (KeyValuePair<string,string> keyValuePair in dictionary)
            {
                query.AddParameter(keyValuePair.Key, keyValuePair.Value);
            }
            
            query.Execute();

            return query.IsSuccessFullyExecuted();
        }
        
        public virtual bool Delete()
        {
            Query query = new Query($"DELETE FROM {this.Table()} WHERE {this.PrimaryKey()} = @{this.PrimaryKey()}");
            query.AddParameter(this.PrimaryKey(), this.Id.ToString());
            
            query.Execute();

            return query.IsSuccessFullyExecuted();
        }
        
        protected abstract T ToModel(Dictionary<string, string> dictionary);

        protected abstract Dictionary<string, string> ToDictionary();

        protected virtual string BaseQuery()
        {
            return $"SELECT * FROM {this.Table()} BT";
        }

        protected virtual string SelectQuery()
        {
            return $"{this.BaseQuery()} WHERE {this.PrimaryKey()} = @{this.PrimaryKey()}";
        }
        
        protected virtual string SelectLastQuery()
        {
            return $"{this.BaseQuery()} ORDER BY {this.PrimaryKey()} ASC";
        }

    }
}