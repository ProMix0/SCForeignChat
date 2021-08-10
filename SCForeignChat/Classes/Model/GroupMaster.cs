using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SCForeignChat.Classes.Model
{
    public class GroupMaster
    {
        private readonly Dictionary<string, List<Regex>> groups = new()
        {
            { "Battle", new() { new(@"battle.*") } },
            { "Trading", new() { new(@"trading") } },
            { "Corporation", new() { new(@"clan.*") } },
            { "General", new() { new(@"general.*") } },
            { "Private", new() { new(@"PRIVATE.*") } }
        };

        private readonly ObservableCollection<Channel> channels;

        public GroupMaster(ObservableCollection<Channel> channels)
        {
            this.channels = channels;
        }

        public void Route(string channel, Message message)
        {
            foreach(var group in groups)
            {
                foreach(var regex in group.Value)
                {
                    if (regex.IsMatch(channel))
                    {
                        if (!channels.Any(c => c.ChannelName == group.Key))
                        {
                            channels.Add(new(group.Key));
                        }
                        channels.First(c => c.ChannelName == group.Key).Add(message);
                        return;
                    }
                }
            }
        }
    }
}
