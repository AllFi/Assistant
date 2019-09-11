using MusicPlugin.MusicProviders;
using Syn.Bot.Oscova;
using Syn.Bot.Oscova.Attributes;
using System.Collections.Generic;
using Assystant.SystemHelpers;
using ParsingHelpers;

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
            result.SendResponse( "приятного прослушивания" );
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

        [Expression( "сделай погромче" )]
        public void VolumeUp( Context context, Result result )
        {
            VolumeHelper.Up();
            result.SendResponse( "как вам угодно" );
        }

        [Expression( "громкость на @sys.text" )]
        public void SetVolume( Context context, Result result )
        {
            var number = result.Entities.OfType( Sys.Text ).Value;
            VolumeHelper.Set( NumbersHelper.ParseNumber( number ) );
            result.SendResponse( $"как вам угодно" );
        }

        [Expression( "сделай потише" )]
        public void VolumeDown( Context context, Result result )
        {
            VolumeHelper.Down();
            result.SendResponse( "как вам угодно" );
        }

        [Expression( "выключи звук" )]
        public void VolumeMuteOn( Context context, Result result )
        {
            VolumeHelper.Mute();
            result.SendResponse( "как вам угодно" );
        }

        [Expression( "верни звук" )]
        public void VolumeMuteOff( Context context, Result result )
        {
            VolumeHelper.UnMute();
            result.SendResponse( "как вам угодно" );
        }
    }
}
