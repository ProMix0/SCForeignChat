using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCForeignChat.Classes.Model
{
    public class ServiceMessage : Message
    {

        public override string OutText { get { return OriginalText; } }


        public ServiceMessage(string message)
            :base("","",message)
        {}
    }
}
