using NAudio.Wave;
using ParsingHelpers;
using Syn.Bot.Oscova;
using Syn.Bot.Oscova.Attributes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        [Expression( "поставь таймер на @sys.text" )]
        public void SetTimerForSeconds( Context context, Result result )
        {
            var timeWords = result.Entities.OfType( Sys.Text ).Value;
            var timeSpan = TimeSpansHelper.WordsToTimeSpan( timeWords );
            SetTimer( timeSpan );
            result.SendResponse( $"таймер на {timeWords} поставлен" );
        }

        [Expression( "удали последний таймер" )]
        public void RemoveLastTimer( Context context, Result result )
        {
            _timers.Remove( _timers.Last() );
            result.SendResponse( $"таймер удален" );
        }

        [Expression( "удали все таймеры" )]
        public void RemoveAllTimers( Context context, Result result )
        {
            _timers.Clear();
            result.SendResponse( $"все таймеры удалены" );
        }

        [Expression( "выключи таймер" )]
        public void StopSignal( Context context, Result result )
        {
            _playing = false;
            result.SendResponse( $"как вам угодно" );
        }

        private void SetTimer( TimeSpan timeSpan )
        {
            var timer = new Timer( SignalStart, null, (int)timeSpan.TotalMilliseconds, Timeout.Infinite );
            _timers.Add( timer );
        }

        private bool _playing = false;

        private void SignalStart( object state )
        {
            using ( var ms = File.OpenRead( "Plugins/Resources/timer_signal.mp3" ) )
            using ( var rdr = new Mp3FileReader( ms ) )
            using ( var wavStream = WaveFormatConversionStream.CreatePcmStream( rdr ) )
            using ( var baStream = new BlockAlignReductionStream( wavStream ) )
            using ( var waveOut = new WaveOut( WaveCallbackInfo.FunctionCallback() ) )
            {
                _playing = true;
                while ( _playing )
                {
                    baStream.Position = 0;
                    waveOut.Init( baStream );
                    waveOut.Play();
                    while ( waveOut.PlaybackState == PlaybackState.Playing )
                    {
                        Thread.Sleep( 50 );
                    }
                }
            }
        }
    }
}
