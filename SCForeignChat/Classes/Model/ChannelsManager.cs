using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SCForeignChat.Classes.Model
{
    public class ChannelsManager
    {

        

        public ObservableCollection<Channel> Channels { get; } = new();

        private readonly GroupMaster master;

        public ChannelsManager()
        {
            master = new(Channels);
        }

        public void AddMessage(string line,TranslateInfo translateInfo=null)
        {
            Message message=null;
            string channelName=null;

            if (Regexs.JoinChannel.IsMatch(line))
            {
                Match match = Regexs.JoinChannel.Match(line);
                channelName = match.Groups[1].Value;
                message = new ServiceMessage($"Join channel");
            }

            if (Regexs.LeaveChannel.IsMatch(line))
            {
                Match match = Regexs.LeaveChannel.Match(line);
                channelName = match.Groups[1].Value;
                message = new ServiceMessage($"Leave channel");
            }

            if (Regexs.Message.IsMatch(line))
            {
                Match match = Regexs.Message.Match(line);
                string time = match.Groups[1].Value;
                channelName = match.Groups[2].Value;
                string author = match.Groups[3].Value;
                string messageText = match.Groups[4].Value;
                message = new Message(time, author, messageText);
            }

            if (message == null || channelName == null) return;

            master.Route(channelName, message);
            if (translateInfo != null) message.Translate(translateInfo);
        }
    }

    internal static class Regexs
    {
        public static readonly Regex JoinChannel = new(@"Join channel <#(\w+)>");
        public static readonly Regex LeaveChannel = new(@"Leave channel <#(\w+)>");
        public static readonly Regex Message = new(@"([0-9]{2}:[0-9]{2}:[0-9]{2}\.[0-9]{3}).*<\s*#{0,1}(.+)\s*>\[\s*(\w+)\] (.*)");
    }
}
