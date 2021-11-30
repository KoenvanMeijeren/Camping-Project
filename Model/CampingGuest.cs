using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    // todo: job
    public class CampingGuest : IModel
    {
        public int Id { get; private set; }
        
        public string Name { get; private set; }
        
        public DateTime Birthdate { get; private set; }

        public CampingGuest(string id, string name, DateTime birthdate)
        {
            this.Id = int.Parse(id);
            this.Name = name;
            this.Birthdate = birthdate;
        }
        
    }
}
