using Syn.Bot.Oscova;
using Syn.Bot.Oscova.Attributes;

namespace TestPluginNamespace
{
    public class TestBotPlugin : OscovaPlugin
    {
        public TestBotPlugin( OscovaBot bot ) : base( bot )
        {
            bot.Dialogs.Add( new TestDialog() );
        }
    }

    public class TestDialog : Dialog
    {
        [Expression( "hello" )]
        public void Hello( Context context, Result result )
        {
            result.SendResponse( "привет человек" );
        }

        [Expression( "good bye" )]
        public void Bye( Context context, Result result )
        {
            result.SendResponse( "пока человек" );
        }

        [Expression( "wake up" )]
        public void WakeUp( Context context, Result result )
        {
            result.SendResponse( "уже на ногах" );
        }

        [Fallback]
        public void GlobalFallback( Context context, Result result )
        {

        }
    }
}
