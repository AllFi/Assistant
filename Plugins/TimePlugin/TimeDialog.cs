using Syn.Bot.Oscova;
using Syn.Bot.Oscova.Attributes;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace TimePlugin
{
    public class TimeBotPlugin : OscovaPlugin
    {
        public TimeBotPlugin( OscovaBot bot ) : base( bot )
        {
            bot.Dialogs.Add( new TimeDialog() );
        }
    }

    public class TimeDialog : Dialog
    {
        private readonly Dictionary<DayOfWeek, string> russianWeekdays = new Dictionary<DayOfWeek, string>
        {
            [DayOfWeek.Friday] = "пятница",
            [DayOfWeek.Monday] = "понедельник",
            [DayOfWeek.Saturday] = "суббота",
            [DayOfWeek.Sunday] = "воскресенье",
            [DayOfWeek.Thursday] = "четверг",
            [DayOfWeek.Tuesday] = "вторник",
            [DayOfWeek.Wednesday] = "среда"
        };

        [Expression( "сколько времени" )]
        [Expression( "время" )]
        [Expression( "который сейчас час" )]
        public void CurrentTime( Context context, Result result )
        {
            result.SendResponse( $"сейчас {DateTime.Now.ToString("t")}" );
        }

        [Expression( "какое сегодня число" )]
        public void CurrentDay( Context context, Result result )
        {
            var now = DateTime.Now;
            result.SendResponse( $"сегодня {now.Date.ToString( "m", CultureInfo.GetCultureInfo( "ru-RU" ) )}" );
        }

        [Expression( "какой сегодня день недели" )]
        public void CurrentWeekDay( Context context, Result result )
        {
            var now = DateTime.Now;
            result.SendResponse( $"сегодня {russianWeekdays[now.DayOfWeek]}" );
        }
    }
}
