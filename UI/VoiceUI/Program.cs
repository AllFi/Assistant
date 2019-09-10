using AssistantCore;
using Assystant.SystemHelpers;
using NAudio.Wave;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace VoiceUI
{
    class Program
    {
        private static Assistant _assistant = null;
        private static SpeechRecognitionEngine _recognizer = null;

        private static DateTime _lastSpeechTime { get; set; }
        private static bool _commandInProgress = false;
        private static readonly HttpClient http = new HttpClient();

        private static void InitHttpClient()
        {
            http.DefaultRequestHeaders.Add( "Authorization", $"Bearer {"CV5BYP4W3CSLZ5IQCEXEX6BBNR5TKJVA"}" );
            http.DefaultRequestHeaders.Add( "Accept-Language", "ru-RU" );
            http.Timeout = TimeSpan.FromSeconds( 20 );
        }

        static void Main( string[] args )
        {
            _assistant = new Assistant( $"{Directory.GetCurrentDirectory()}/Plugins", TextToSpeech, $"{Directory.GetCurrentDirectory()}/WordNet" );
            _assistant.Start();
            InitHttpClient();

            using ( _recognizer = new SpeechRecognitionEngine( CultureInfo.GetCultureInfo( "en-US" ) ) )
            {
                var builder = new GrammarBuilder( "simba" ) { Culture = CultureInfo.GetCultureInfo( "en-US" ) };
                Grammar symbaGrammar = new Grammar( builder );
                symbaGrammar.Name = "Simba";
                _recognizer.UnloadAllGrammars();
                _recognizer.LoadGrammar( symbaGrammar );
                _recognizer.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>( HandleSpeech );
                _recognizer.AudioStateChanged += new EventHandler<AudioStateChangedEventArgs>( HandleAudioStateChanging );
                _recognizer.SetInputToDefaultAudioDevice();
                _recognizer.InitialSilenceTimeout = TimeSpan.FromMilliseconds( 100 );
                _recognizer.RecognizeAsync( RecognizeMode.Multiple );
                while ( true )
                {
                    Console.ReadLine();
                }
            }
        }

        private static void HandleAudioStateChanging( object sender, AudioStateChangedEventArgs e )
        {
            if ( e.AudioState == AudioState.Speech )
                _lastSpeechTime = DateTime.Now;
        }

        private static async void HandleSpeech( object sender, SpeechRecognizedEventArgs e )
        {
            if ( e.Result.Grammar.Name != "Simba" || _commandInProgress || e.Result.Confidence < 0.93 )
                return;

            StartListening( out bool wasMuted );
            var speech = await Task.Run( () => Record() );
            FinishListening( wasMuted );
            var request = await SpeechToText( speech );
            if ( !String.IsNullOrEmpty( request ) )
            {
                Console.WriteLine( request );
                _assistant.HandleRequest( request );
            }
        }

        private static void StartListening( out bool wasMuted )
        {
            _commandInProgress = true;
            MakeSignal();
            wasMuted = VolumeHelper.IsMuted();
            if ( !wasMuted ) VolumeHelper.Mute();
        }

        private static void FinishListening( bool wasMuted )
        {
            if ( !wasMuted ) VolumeHelper.Mute();
            _commandInProgress = false;
        }

        private static void MakeSignal()
        {
            using ( var ms = File.OpenRead( "Resources/signal.mp3" ) )
            using ( var rdr = new Mp3FileReader( ms ) )
            using ( var wavStream = WaveFormatConversionStream.CreatePcmStream( rdr ) )
            using ( var baStream = new BlockAlignReductionStream( wavStream ) )
            using ( var waveOut = new WaveOut( WaveCallbackInfo.FunctionCallback() ) )
            {
                waveOut.Init( baStream );
                waveOut.Play();
                while ( waveOut.PlaybackState == PlaybackState.Playing )
                {
                    Thread.Sleep( 50 );
                }
            }
        }

        private static byte[] Record()
        {
            using ( var waveIn = new WaveInEvent() )
            {
                waveIn.DeviceNumber = 0;
                waveIn.WaveFormat = new WaveFormat( 8000, 1 );
                waveIn.BufferMilliseconds = 250;
                
                using ( var ms = new MemoryStream() )
                {
                    using ( var writer = new WaveFileWriter( ms, waveIn.WaveFormat ) )
                    {
                        waveIn.DataAvailable += new EventHandler<WaveInEventArgs>( ( sender, e ) => writer.Write( e.Buffer, 0, e.Buffer.Length ) );
                        waveIn.StartRecording();

                        Thread.Sleep( 1000 );
                        _lastSpeechTime = DateTime.Now;
                        while ( DateTime.Now - _lastSpeechTime < TimeSpan.FromSeconds( 1 ) )
                        {
                            Thread.Sleep( 1000 );
                        }
                        waveIn.StopRecording();
                        return ms.ToArray();
                    }
                }
            }
        }

        private static async Task<string> SpeechToText( byte[] speech )
        {
            try
            {
                Stopwatch sw = Stopwatch.StartNew();
                var context = HttpUtility.UrlEncode( $"{{\"locale\": \"ru_RU\"}}" );
                var uri = $"https://api.wit.ai/speech?v=20170307&context={context}";
                var data = new ByteArrayContent( speech );
                data.Headers.Add( "Content-Type", "audio/raw;encoding=signed-integer;bits=16;rate=8000;endian=little" );
                var response = await http.PostAsync( uri, data );
                var sResponse = await response.Content.ReadAsStringAsync();
                var respObj = new { _text = "" };
                respObj = JsonConvert.DeserializeAnonymousType( sResponse, respObj );
                Console.WriteLine( sw.ElapsedMilliseconds );
                return respObj._text;
            }
            catch ( Exception ex )
            {
                Console.WriteLine( ex.Message );
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
