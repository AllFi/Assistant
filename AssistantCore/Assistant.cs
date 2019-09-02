using AssistantCore.Russian;
using Syn.Bot.Oscova;
using Syn.Bot.Oscova.Events;
using System;

namespace AssistantCore
{
    public class Assistant
    {
        private string _pluginsDirectory = String.Empty;
        private string _wordNetDirectory = String.Empty;
        private OscovaBot _bot = null;
        private Action<AssistantResponse> _receiver = null;

        public Assistant( string pluginsDirectory, Action<AssistantResponse> receiver, string wordNetDirectory = "" )
        {
            _pluginsDirectory = pluginsDirectory;
            _wordNetDirectory = wordNetDirectory;
            _receiver = receiver;
        }

        public void Start()
        {
            _bot = new OscovaBot();
            _bot.Plugins.LoadFromDirectory( _pluginsDirectory, fileInfo => fileInfo.Name.ToLower().EndsWith( "plugin.dll" ) );
            _bot.Language = new RussianLanguage( _wordNetDirectory );
            _bot.Trainer.StartTraining();
            _bot.MainUser.ResponseReceived += Reply;
            _receiver.Invoke( new AssistantResponse( "Жду ваших указаний!" ) );
        }

        public void HandleRequest( string expression )
        {
            var evaluationResult = _bot.Evaluate( expression );
            Console.WriteLine( evaluationResult.SuggestedIntent.Score );
            evaluationResult.Invoke();
        }

        private void Reply( object sender, ResponseReceivedEventArgs args )
        {
            _receiver.Invoke( new AssistantResponse( args.Response.Text ) );
        }
    }
}
