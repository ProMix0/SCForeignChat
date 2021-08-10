using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCForeignChat.Classes
{
    public class Channel
    {
        public ObservableCollection<Message> Messages { get; } = new();
        public string ChannelName { get; }

        public Channel(string name)
        {
            ChannelName = name;
        }

        public void Add(Message message)
        {
            Messages.Add(message);
        }
    }
}
