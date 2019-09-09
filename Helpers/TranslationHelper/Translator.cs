using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace TranslationHelper.Translator
{ 
    public static class Translator
    {
        private static HttpClient http = new HttpClient();

        static Translator()
        {
            http.DefaultRequestHeaders.Add( "Authorization", $"Bearer {"CV5BYP4W3CSLZ5IQCEXEX6BBNR5TKJVA"}" );
            http.DefaultRequestHeaders.Add( "Accept-Language", "ru-RU" );
            http.Timeout = TimeSpan.FromSeconds( 10 );
        }

        public static string RussianToEnglish( string russianText )
        {
            if ( String.IsNullOrEmpty( russianText ) )
                return String.Empty;
            try
            {
                var request = $"https://translate.yandex.net/api/v1.5/tr.json/translate?key=trnsl.1.1.20190902T155713Z.8df117bbef2b110a.64054e0443f6c07c89a917fdf36a608dc15b1dd2&text={russianText}&lang=en&format=plain";
                var response = http.GetStringAsync( request ).Result;
                var respObj = new { text = new List<string>() };
                var englishText = JsonConvert.DeserializeAnonymousType( response, respObj ).text[0];
                Console.WriteLine( englishText );
                return englishText;
            }
            catch ( Exception ex )
            {
                Console.WriteLine( ex.Message );
                return String.Empty;
            }
        }

        public static string EnglishToRussian( string englishText )
        {
            if ( String.IsNullOrEmpty( englishText ) )
                return String.Empty;

            englishText = englishText.Replace( ".", "" ).Replace( ",", "" ).Replace( "\"", "" );
            try
            {
                var request = $"https://translate.yandex.net/api/v1.5/tr.json/translate?key=trnsl.1.1.20190902T155713Z.8df117bbef2b110a.64054e0443f6c07c89a917fdf36a608dc15b1dd2&text={englishText}&lang=ru&format=plain";
                var response = http.GetStringAsync( request ).Result;
                var respObj = new { text = new List<string>() };
                var russianText = JsonConvert.DeserializeAnonymousType( response, respObj ).text[0];
                Console.WriteLine( russianText );
                return russianText;
            }
            catch ( Exception ex )
            {
                Console.WriteLine( ex.Message );
                return String.Empty;
            }
        }
    }
}
