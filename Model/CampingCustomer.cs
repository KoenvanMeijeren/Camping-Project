using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class CampingCustomer
    {
        public int Id { get; private set; }
        
        public Address Address { get; private set; }
        
        public DateTime Birthdate { get; private set; }
        
        public string Email { get; private set; }
        
        public string Phonenumber  { get; private set; }

        public CampingCustomer(int id, Address address, DateTime birthdate, string email, string phonenumber)
        {
            this.Id = id;
            this.Address = address;
            this.Birthdate = birthdate;
            this.Email = email;
            this.Phonenumber = phonenumber;
        }
        
    }
}
