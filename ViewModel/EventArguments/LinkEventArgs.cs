using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.EventArguments
{
    public class LinkEventArgs : EventArgs
    {
        public readonly string Href;

        public LinkEventArgs(string href)
        {
            this.Href = href;
        }
    }
}
