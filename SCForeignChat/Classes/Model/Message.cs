using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using LibreTranslate.Net;
using SCForeignChat.Classes.Model;

namespace SCForeignChat.Classes
{
    public class Message:INotifyPropertyChanged
    {
        public virtual string OutText
        {
            get
            {
                return $"{Time} | [{Author}] {TranslatedText}";
            }
        }

        public string OriginalText { get; }
        public string TranslatedText 
        {
            get
            {
                return translatedText;
            }
            private set
            {
                translatedText = value;
                OnPropertyChanged("OutText");
            } 
        }
        private string translatedText;

        public string Author { get; }
        public string Time { get; }

        public Message(string time, string author, string message)
        {
            Time = time;
            Author = author;
            OriginalText = message;
        }

        public async Task Translate(TranslateInfo translateInfo)
        {
            var translate =await translateInfo.Translater.TranslateAsync(new()
            {
                Source = translateInfo.From,
                Target = translateInfo.To,
                Text = OriginalText
            });
            TranslatedText = translate;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
