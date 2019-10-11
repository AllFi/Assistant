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
        private static SpeechSynthesizer _synth = null;
        private static VoiceAssistantConfig _config = null;

        static void Main( string[] args )
        {
            for ( int i = 0; i < 3; i++ )
            {
                try
                {
                    StartVoiceAssistant();
                }
                catch ( Exception ex )
                {
                    // TODO: добавь логирование
                }
            }
        }

        private static void StartVoiceAssistant()
        {
            _config = VoiceAssistantConfig.Default();
            _speechRecognizerClient = new WitAiSpeechRecognitionClient( _config.WitAiToken, _config.WitAiTimeoutSeconds );
            _recorder = new NAudioRecorder( _config.SignalPath );

            using ( _synth = new SpeechSynthesizer() )
            {
                _synth.SetOutputToDefaultAudioDevice();
                using ( _recognizer = CreateSimbaCommandRecognizer() )
                {
                    _assistant = new Assistant( _config.PluginsPath, TextToSpeech, _config.WordNetPath );
                    _assistant.Start();
                    while ( true )
                    {
                        Console.ReadLine();
                    }
                }
            }
        }

        private static SpeechRecognitionEngine CreateSimbaCommandRecognizer()
        {
            var recognizer = new SpeechRecognitionEngine( CultureInfo.GetCultureInfo( "en-US" ) );
            var builder = new GrammarBuilder( _config.AssistantName ) { Culture = CultureInfo.GetCultureInfo( "en-US" ) };
            var symbaGrammar = new Grammar( builder ) { Name = _config.AssistantName };
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
            if ( e.Result.Grammar.Name != _config.AssistantName || e.Result.Confidence < _config.MinConfidence )
                return;

            var speech = await Task.Run( () => _recorder.Record() );
            var request = await _speechRecognizerClient.SpeechToText( speech );
            if ( !String.IsNullOrEmpty( request ) )
                _assistant.HandleRequest( request );
        }

        private static void TextToSpeech( AssistantResponse response )
        {
            _assistant.MuteAllPlugins();
            _synth.Speak( response.Text );
            _assistant.UnmuteAllPlugins();
        }
    }
}
