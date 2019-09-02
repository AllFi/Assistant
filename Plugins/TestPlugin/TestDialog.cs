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
        [Expression( "привет сэм" )]
        public void Hello( Context context, Result result )
        {
            result.SendResponse( "привет человек" );
        }

        [Expression( "пока сэм" )]
        public void Bye( Context context, Result result )
        {
            result.SendResponse( "пока человек" );
        }

        [Expression( "проснись сэм" )]
        public void WakeUp( Context context, Result result )
        {
            result.SendResponse( "уже на ногах" );
        }

        [Expression( "заткнись" )]
        public void ShutUp( Context context, Result result )
        {
            result.SendResponse( "сам заткнись" );
        }

        [Expression( "бей бот" )]
        public void Beat( Context context, Result result )
        {
            result.SendResponse( "бью" );
        }
    }
}
