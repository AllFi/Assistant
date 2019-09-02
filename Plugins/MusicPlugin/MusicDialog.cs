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

        [Expression( "включи @sys.text" )]
        public void TurnOn( Context context, Result result )
        {
            _mainMusicProvider.TurnOn( result.Entities.OfType( Sys.Text ).Value );
            result.SendResponse( "как вам угодно" );
        }

        [Expression( "выключи музыку" )]
        public void TurnOff( Context context, Result result )
        {
            _mainMusicProvider.TurnOff();
            result.SendResponse( "как вам угодно" );
        }

        [Expression( "дальше" )]
        public void Next( Context context, Result result )
        {
            _mainMusicProvider.Next();
            result.SendResponse( "как вам угодно" );
        }

        [Expression( "назад" )]
        public void Previous( Context context, Result result )
        {
            _mainMusicProvider.Previous();
            result.SendResponse( "как вам угодно" );
        }

        [Expression( "пауза" )]
        public void Pause( Context context, Result result )
        {
            _mainMusicProvider.Pause();
            result.SendResponse( "как вам угодно" );
        }

        [Expression( "верни музыку" )]
        public void Play( Context context, Result result )
        {
            _mainMusicProvider.Play();
            result.SendResponse( "как вам угодно" );
        }

        [Expression( "прибавь громкость" )]
        public void VolumeUp( Context context, Result result )
        {
            Volume.Up();
            result.SendResponse( "как вам угодно" );
        }

        [Expression( "убавь громкость" )]
        public void VolumeDown( Context context, Result result )
        {
            Volume.Down();
            result.SendResponse( "как вам угодно" );
        }

        [Expression( "выключи звук" )]
        public void VolumeMute( Context context, Result result )
        {
            Volume.Mute();
            result.SendResponse( "как вам угодно" );
        }

        [Expression( "верни звук" )]
        public void VolumeMuteOff( Context context, Result result )
        {
            Volume.Mute();
            result.SendResponse( "как вам угодно" );
        }
    }
}
