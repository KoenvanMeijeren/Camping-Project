using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.EventArguments
{
    public class LinkEventArgs : EventArgs
    {
        public string href;

        public LinkEventArgs(string href)
        {
            this.href = href;
        }
    }
}
