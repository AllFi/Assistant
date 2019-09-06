using System;
using System.Collections.Generic;

namespace ParsingHelpers
{
    public static class TimeSpansHelper
    {
        public enum UnitOfTime
        {
            Unknown,
            Second,
            Minute,
            Hour,
            Day,
            Week
        }

        private static Dictionary<string, UnitOfTime> _unitsOfTime = new Dictionary<string, UnitOfTime>()
        {
            ["second"] = UnitOfTime.Second,
            ["minute"] = UnitOfTime.Minute,
            ["hour"] = UnitOfTime.Hour,
            ["day"] = UnitOfTime.Day,
            ["week"] = UnitOfTime.Week,
        };

        private static TimeSpan AddTo( TimeSpan timeSpan, string numberWords, UnitOfTime unit )
        {
            var number = NumbersHelper.ParseNumber( numberWords );
            switch ( unit )
            {
                case UnitOfTime.Second: return timeSpan.Add( TimeSpan.FromSeconds( number ) );
                case UnitOfTime.Minute: return timeSpan.Add( TimeSpan.FromMinutes( number ) );
                case UnitOfTime.Hour: return timeSpan.Add( TimeSpan.FromHours( number ) );
                case UnitOfTime.Day: return timeSpan.Add( TimeSpan.FromDays( number ) );
                case UnitOfTime.Week: return timeSpan.Add( TimeSpan.FromDays( number * 7 ) );
            }
            return timeSpan;
        }

        private static bool IsTimeWord( string word, out UnitOfTime unit )
        {
            foreach ( var timeUnit in _unitsOfTime.Keys )
            {
                if ( word.StartsWith( timeUnit ) )
                {
                    unit = _unitsOfTime[timeUnit];
                    return true;
                }
            }
            unit = UnitOfTime.Unknown;
            return false;
        }

        public static TimeSpan WordsToTimeSpan( string timeString )
        {
            var timeWords = timeString.Replace( "-", "" ).Split( ' ' );
            var numberBuffer = String.Empty;
            var resultTimeSpan = new TimeSpan();
            foreach ( var word in timeWords )
            {
                if ( NumbersHelper.IsNumber( word ) )
                {
                    numberBuffer = $"{numberBuffer} {word}";
                }
                else if ( IsTimeWord( word, out var unitOfTime ) )
                {
                    resultTimeSpan = AddTo( resultTimeSpan, numberBuffer, unitOfTime );
                    numberBuffer = String.Empty;
                }
            }
            return resultTimeSpan;
        }
    }
}
