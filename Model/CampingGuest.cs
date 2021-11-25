using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class CampingGuest
    {
        public int Id { get; private set; }
        
        public string Name { get; private set; }
        
        public DateTime Birthdate { get; private set; }

        public CampingGuest(int id, string name, DateTime birthdate)
        {
            this.Id = id;
            this.Name = name;
            this.Birthdate = birthdate;
        }
        
    }
}
