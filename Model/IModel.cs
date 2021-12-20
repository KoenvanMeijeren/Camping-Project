using System.Collections.Generic;

namespace Visualization
{
    /// <summary>
    /// Provides an interface for models, representing a database table. One model class represents one record from
    /// the database table, multiple models represents multiple records from the database.
    /// </summary>
    public interface IModel
    {
        /// <summary>
        /// The ID of one record.
        /// </summary>
        public int Id { get; }
        
        /// <summary>
        /// Inserts one model into the database.
        /// </summary>
        /// <returns>Whether the query was successful or not.</returns>
        public bool Insert();
        
        /// <summary>
        /// Updates one model from the database.
        /// </summary>
        /// <param name="dictionary">The values to be updated.</param>
        /// <returns>Whether the query was successful or not.</returns>
        public bool Update(Dictionary<string, string> dictionary);
        
        /// <summary>
        /// Deletes one model from the database.
        /// </summary>
        /// <returns>Whether the query was successful or not.</returns>
        public bool Delete();
    }
}