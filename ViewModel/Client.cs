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
        public bool IsSuperUser { get; private set; }

        public Client(bool issuperuser)
        {
            this.IsSuperUser = issuperuser;
        }

    }
}
