using Syn.Bot.Oscova;
using Syn.Bot.Oscova.Attributes;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SmallTalkPlugin
{
    public class SmallTalkBotPlugin : OscovaPlugin
    {
        public SmallTalkBotPlugin( OscovaBot bot ) : base( bot )
        {
            bot.Dialogs.Add( new SmallTalkDialog() );
        }
    }

    public class SmallTalkDialog : Dialog
    {
        private Dictionary<string, string> _smallTalks = new Dictionary<string, string>();

        public SmallTalkDialog()
        {
            var smallTalks = File.ReadAllLines( "Plugins/Resources/smalltalks.txt" ).Select( smallTalk => smallTalk.Split( '=' ) );
            foreach ( var smallTalk in smallTalks )
            {
                var request = smallTalk[0];
                var response = smallTalk[1];
                _smallTalks.Add( request, response );
            }
        }

        [Fallback]
        public void SmallTalk( Context context, Result result )
        {
            var request = result.Request.Text.Replace( "?", "" ).Replace( "!", "" ).ToLower();
            if ( _smallTalks.ContainsKey( request ) )
            {
                result.SendResponse( _smallTalks[request] );
                return;
            }

            result.SendResponse( "Повторите пожалуйста!" );
        }
    }
}
