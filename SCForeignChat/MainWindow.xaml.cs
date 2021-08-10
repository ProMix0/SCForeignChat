using SCForeignChat.Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using LibreTranslate.Net;
using System.Text.RegularExpressions;
using SCForeignChat.Classes.Model;

namespace SCForeignChat
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public TranslateInfo TranslateInfo { get; }

        public ChannelsManager Manager { get; } = new();

        private static readonly string[] urls = new string[] { "https://libretranslate.de/",
            "https://translate.mentality.rip/","https://translate.astian.org/","https://translate.argosopentech.com/"};

        private readonly StreamReader reader;

        public MainWindow()
        {
            Random random = new();

            TranslateInfo = new RandomTranslateInfo(urls, LanguageCode.Russian, LanguageCode.English);

            InitializeComponent();

            FileSystemWatcher watcher = new(
                Directory.EnumerateDirectories(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\My Games\StarConflict\logs")
                .OrderByDescending(folder => Directory.GetCreationTime(folder)).First());
            reader = new(File.Open(watcher.Path + @"\chat.log", FileMode.Open, FileAccess.Read));
            reader.ReadToEnd();
            watcher.Changed += LogAppend;
            watcher.EnableRaisingEvents = true;

            //Test();
            //Test2();
        }

        private void Test2()
        {
            LogAppend(null, new(WatcherChangeTypes.Changed, "", "chat.log"));
        }

        private void LogAppend(object sender, FileSystemEventArgs e)
        {
            if (e.Name != "chat.log") return;

            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();

                Manager.AddMessage(line);
            }
        }

        private async Task TestAsync()
        {
            Channel channel1 = new("Test1");
            channel1.Messages.Add(new("00:00", "Me", "I'm so beautiful"));
            channel1.Messages.Add(new("00:01", "You", "Yes, you are so beautiful"));

            Channel channel2 = new("Test2");
            channel2.Messages.Add(new("00:00", "Me", "I'm so ugly"));
            channel2.Messages.Add(new("00:01", "You", "Yes, you are so ugly"));
            channel2.Messages.Add(new("00:02", "Me", "You damn right"));

            /*Channels.Add(channel1);
            Channels.Add(channel2);*/

            foreach (Message message in channel1.Messages.Union(channel2.Messages))
            {
                await message.Translate(TranslateInfo);
            }
        }

        private class RandomTranslateInfo : TranslateInfo
        {
            public override LibreTranslate.Net.LibreTranslate Translater
            {
                get
                {
                    return translaters[random.Next(translaters.Length)];
                }
            }

            private readonly LibreTranslate.Net.LibreTranslate[] translaters;
            private readonly Random random = new();

            public RandomTranslateInfo(string[] translaters, LanguageCode from, LanguageCode to)
            : base(null, from, to)
            {
                this.translaters = translaters.Select(url => new LibreTranslate.Net.LibreTranslate(url)).ToArray();
            }
        }

        private bool translated = true;
        private DateTime lastChange = DateTime.Now;
        private Task translationTask;

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            translated = false;
            lastChange = DateTime.Now;

            if (translationTask == null || !translationTask.Status.HasFlag(TaskStatus.Running))
                translationTask = TranslateTask();
        }

        private async Task TranslateTask()
        {
            await Task.Delay(300);
            if (lastChange.AddSeconds(0.5) > DateTime.Now)
                translateField.Text = await TranslateInfo.Translater.TranslateAsync(new()
                {
                    Source = TranslateInfo.To,
                    Target = TranslateInfo.From,
                    Text = translateText.Text
                });
            Clipboard.SetText(translateField.Text);
        }
    }
}
