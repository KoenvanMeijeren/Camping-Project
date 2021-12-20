using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    public class Client
    {
        public string UID { get; set; }
        public Type ClientType { get; private set; }

        public Client(bool issuperuser)
        {
            if (issuperuser)
            {
                this.ClientType = Type.CampingOwner;
            }

            this.ClientType = Type.CampingCustomer;
            
        }

    }

    public enum Type
    { 
        CampingOwner,
        CampingCustomer
    }

}
