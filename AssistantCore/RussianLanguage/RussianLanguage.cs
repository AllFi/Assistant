using System.Globalization;
using Syn.Bot.Oscova.Languages.English;

namespace AssistantCore.Russian
{
    public class RussianLanguage : EnglishLanguage
    {
        public RussianLanguage( string wordNetDirectory = "" )
        {
            Culture = CultureInfo.GetCultureInfo( "ru-RU" );
            var english = new EnglishLanguage();
            Filters = english.Filters;
            Tokenizer = english.Tokenizer;
            Detokenizer = english.Detokenizer;
            Normalizer = english.Normalizer;
            Stemmer = new RussianStemmer();
            StopWords = english.StopWords;
            Punctuations = english.Punctuations;
            WordNet.LoadFromDirectory( wordNetDirectory );
        }
    }
}
