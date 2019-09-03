using MusicPlugin.MusicProviders;
using MusicPlugin.Utils;
using Syn.Bot.Oscova;
using Syn.Bot.Oscova.Attributes;
using System.Collections.Generic;

namespace MusicPlugin
{
    public class MusicBotPlugin : OscovaPlugin
    {
        public MusicBotPlugin( OscovaBot bot ) : base( bot )
        {
            bot.Dialogs.Add( new MusicDialog() );
        }
    }

    public class MusicDialog : Dialog
    {
        private List<MusicProvider> _musicProviders = new List<MusicProvider>();
        private MusicProvider _mainMusicProvider = null;

        public MusicDialog()
        {
            _musicProviders.Add( new YandexMusic() );
            _mainMusicProvider = _musicProviders[0];
        }

        [Expression( "turn on @sys.text" )]
        public void TurnOn( Context context, Result result )
        {
            _mainMusicProvider.TurnOn( result.Entities.OfType( Sys.Text ).Value );
            result.SendResponse( "как вам угодно" );
        }

        [Expression( "turn the music off" )]
        public void TurnOff( Context context, Result result )
        {
            _mainMusicProvider.TurnOff();
            result.SendResponse( "как вам угодно" );
        }

        [Expression( "next" )]
        public void Next( Context context, Result result )
        {
            _mainMusicProvider.Next();
            result.SendResponse( "как вам угодно" );
        }

        [Expression( "previous" )]
        public void Previous( Context context, Result result )
        {
            _mainMusicProvider.Previous();
            result.SendResponse( "как вам угодно" );
        }

        [Expression( "pause" )]
        public void Pause( Context context, Result result )
        {
            _mainMusicProvider.Pause();
            result.SendResponse( "как вам угодно" );
        }

        [Expression( "play" )]
        public void Play( Context context, Result result )
        {
            _mainMusicProvider.Play();
            result.SendResponse( "как вам угодно" );
        }

        [Expression( "volume up" )]
        public void VolumeUp( Context context, Result result )
        {
            Volume.Up();
            result.SendResponse( "как вам угодно" );
        }

        [Expression( "volume down" )]
        public void VolumeDown( Context context, Result result )
        {
            Volume.Down();
            result.SendResponse( "как вам угодно" );
        }

        [Expression( "mute" )]
        public void VolumeMute( Context context, Result result )
        {
            Volume.Mute();
            result.SendResponse( "как вам угодно" );
        }
    }
}
