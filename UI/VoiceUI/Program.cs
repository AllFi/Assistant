using AssistantCore;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.Threading.Tasks;
using System.Web;

namespace VoiceUI
{
    class Program
    {
        private static Assistant _assistant = null;

        static void Main( string[] args )
        {
            _assistant = new Assistant( $"{Directory.GetCurrentDirectory()}/Plugins", TextToSpeech, $"{Directory.GetCurrentDirectory()}/RussianLanguage/WordNet" );
            _assistant.Start();
            InitHttpClient();

            using ( SpeechRecognitionEngine recognizer = new SpeechRecognitionEngine( new System.Globalization.CultureInfo( "en-US" ) ) )
            {
                recognizer.LoadGrammar( new DictationGrammar() );
                recognizer.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>( recognizer_SpeechRecognized );
                recognizer.SetInputToDefaultAudioDevice();
                recognizer.InitialSilenceTimeout = TimeSpan.FromMilliseconds( 100 );
                recognizer.RecognizeAsync( RecognizeMode.Multiple );
                while ( true )
                {
                    Console.ReadLine();
                }
            }
        }

        private static async void recognizer_SpeechRecognized( object sender, SpeechRecognizedEventArgs e )
        {
            using ( var ms = new MemoryStream() )
            {
                e.Result.Audio.WriteToWaveStream( ms );
                var request = await SpeechToText( ms );
                Console.WriteLine( request );
                _assistant.HandleRequest( request );
            }
        }
        private static readonly HttpClient http = new HttpClient();

        private static void InitHttpClient()
        {
            http.DefaultRequestHeaders.Add( "Authorization", $"Bearer {"CV5BYP4W3CSLZ5IQCEXEX6BBNR5TKJVA"}" );
            http.DefaultRequestHeaders.Add( "Accept-Language", "ru-RU" );
            http.Timeout = TimeSpan.FromSeconds( 5 );
        }

        private static async Task<string> SpeechToText( MemoryStream ms )
        {
            try
            {
                Stopwatch sw = Stopwatch.StartNew();
                var context = HttpUtility.UrlEncode( $"{{\"locale\": \"ru_RU\"}}" );
                var uri = $"https://api.wit.ai/speech?v=20170307&context={context}";
                var data = new ByteArrayContent( ms.ToArray() );
                data.Headers.Add( "Content-Type", "audio/wav" );
                var response = await http.PostAsync( uri, data );
                var sResponse = await response.Content.ReadAsStringAsync();
                var respObj = new { _text = "" };
                respObj = JsonConvert.DeserializeAnonymousType( sResponse, respObj );
                Console.WriteLine( sw.ElapsedMilliseconds );
                return respObj._text;
            }
            catch
            {
                return String.Empty;
            }
        }

        private static void TextToSpeech( AssistantResponse response )
        {
            SpeechSynthesizer synth = new SpeechSynthesizer();
            synth.SetOutputToDefaultAudioDevice();
            synth.SpeakAsync( response.Text );
        }
    }
}
