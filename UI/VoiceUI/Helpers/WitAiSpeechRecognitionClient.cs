using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace VoiceUI.Helpers
{
    public class WitAiSpeechRecognitionClient
    {
        private HttpClient _httpClient;

        public WitAiSpeechRecognitionClient()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add( "Authorization", $"Bearer {"CV5BYP4W3CSLZ5IQCEXEX6BBNR5TKJVA"}" );
            _httpClient.DefaultRequestHeaders.Add( "Accept-Language", "ru-RU" );
            _httpClient.Timeout = TimeSpan.FromSeconds( 10 );
        }

        public async Task<string> SpeechToText( byte[] speech )
        {
            try
            {
                var sw = Stopwatch.StartNew();
                var context = HttpUtility.UrlEncode( $"{{\"locale\": \"ru_RU\"}}" );
                var uri = $"https://api.wit.ai/speech?v=20170307&context={context}";
                var data = new ByteArrayContent( speech );
                data.Headers.Add( "Content-Type", "audio/mpeg3" );

                var response = await _httpClient.PostAsync( uri, data );
                var sResponse = await response.Content.ReadAsStringAsync();
                var respObj = new { _text = "" };
                respObj = JsonConvert.DeserializeAnonymousType( sResponse, respObj );
                Console.WriteLine( $"Elapsed: {sw.ElapsedMilliseconds}, Bytes: {speech.Length}, Recognized: {respObj._text}" );
                return respObj._text;
            }
            catch ( Exception ex )
            {
                Console.WriteLine( ex.Message );
                return String.Empty;
            }
        }
    }
}