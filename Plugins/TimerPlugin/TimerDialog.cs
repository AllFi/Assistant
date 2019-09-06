using ParsingHelpers;
using Syn.Bot.Oscova;
using Syn.Bot.Oscova.Attributes;
using System;
using System.Collections.Generic;
using System.Threading;

namespace TimerPlugin
{
    public class TimerBotPlugin : OscovaPlugin
    {
        public TimerBotPlugin( OscovaBot bot ) : base( bot )
        {
            bot.Dialogs.Add( new TimerDialog() );
        }
    }

    public class TimerDialog : Dialog
    {
        private List<Timer> _timers = new List<Timer>();

        [Expression( "set the timer for @sys.text" )]
        public void SetTimerForSeconds( Context context, Result result )
        {
            var timeWords = result.Entities.OfType( Sys.Text ).Value;
            var timeSpan = TimeSpansHelper.WordsToTimeSpan( timeWords );
            SetTimer( timeSpan );
            result.SendResponse( $"таймер поставлен" );
        }

        private void SetTimer( TimeSpan timeSpan )
        {
            var timer = new Timer( Signal, null, (int)timeSpan.TotalMilliseconds, Timeout.Infinite );
            _timers.Add( timer );
        }

        private void Signal( object state )
        {
            Console.WriteLine( "Сигнал" );
        }
    }
}
