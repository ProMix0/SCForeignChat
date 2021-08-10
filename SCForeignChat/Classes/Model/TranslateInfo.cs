using LibreTranslate.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCForeignChat.Classes.Model
{
    public class TranslateInfo
    {
        public virtual LibreTranslate.Net.LibreTranslate Translater { get; }
        public LanguageCode From { get; }
        public LanguageCode To { get; }

        public TranslateInfo(LibreTranslate.Net.LibreTranslate translater,LanguageCode from,LanguageCode to)
        {
            Translater = translater;
            From = from;
            To = to;
        }
    }
}
