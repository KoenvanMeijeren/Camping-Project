using System.Collections.Generic;

namespace Model
{
    public interface IModel
    {
        public int Id { get; }
        
        public bool Insert();
        public bool Update(Dictionary<string, string> dictionary);
        
        public bool Delete();
    }
}