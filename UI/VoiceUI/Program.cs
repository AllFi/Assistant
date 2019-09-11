using AssistantCore;
using System;
using System.Globalization;
using System.IO;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.Threading.Tasks;
using VoiceUI.Helpers;

namespace VoiceUI
{
    class Program
    {
        private static Assistant _assistant = null;
        private static SpeechRecognitionEngine _recognizer = null;
        private static WitAiSpeechRecognitionClient _speechRecognizerClient = null;
        private static NAudioRecorder _recorder = null;

        static void Main( string[] args )
        {
            _speechRecognizerClient = new WitAiSpeechRecognitionClient();
            _recorder = new NAudioRecorder();

            using ( _recognizer = CreateSimbaCommandRecognizer() )
            {
                _assistant = new Assistant( $"{Directory.GetCurrentDirectory()}/Plugins", TextToSpeech, $"{Directory.GetCurrentDirectory()}/WordNet" );
                _assistant.Start();
                while ( true )
                {
                    Console.ReadLine();
                }
            }
        }

        private static SpeechRecognitionEngine CreateSimbaCommandRecognizer()
        {
            var recognizer = new SpeechRecognitionEngine( CultureInfo.GetCultureInfo( "en-US" ) );
            var builder = new GrammarBuilder( "simba" ) { Culture = CultureInfo.GetCultureInfo( "en-US" ) };
            var symbaGrammar = new Grammar( builder ) { Name = "Simba" };
            recognizer.UnloadAllGrammars();
            recognizer.LoadGrammar( symbaGrammar );
            recognizer.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>( HandleSpeech );
            recognizer.AudioStateChanged += new EventHandler<AudioStateChangedEventArgs>( _recorder.HandleAudioStateChanging );
            recognizer.SetInputToDefaultAudioDevice();
            recognizer.InitialSilenceTimeout = TimeSpan.FromMilliseconds( 100 );
            recognizer.RecognizeAsync( RecognizeMode.Multiple );
            return recognizer;
        }

        private static async void HandleSpeech( object sender, SpeechRecognizedEventArgs e )
        {
            if ( e.Result.Grammar.Name != "Simba" || e.Result.Confidence < 0.94 )
                return;

            var speech = await Task.Run( () => _recorder.Record() );
            var request = await _speechRecognizerClient.SpeechToText( speech );
            if ( !String.IsNullOrEmpty( request ) )
                _assistant.HandleRequest( request );
        }

        private static void TextToSpeech( AssistantResponse response )
        {
            var synth = new SpeechSynthesizer();
            synth.SetOutputToDefaultAudioDevice();
            synth.Speak( response.Text );
        }
    }
}
