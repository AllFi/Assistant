using AudioSwitcher.AudioApi.CoreAudio;
using System;

namespace Assystant.SystemHelpers
{
    public class VolumeHelper
    {
        private static CoreAudioDevice defaultPlaybackDevice = new CoreAudioController().DefaultPlaybackDevice;

        public static void Mute()
        {
            defaultPlaybackDevice.Mute( true );
        }

        public static void UnMute()
        {
            defaultPlaybackDevice.Mute( false );
        }

        public static bool IsMuted()
        {
            return defaultPlaybackDevice.IsMuted;
        }

        public static void Down( int volumeChange = 5 )
        {
            defaultPlaybackDevice.Volume -= volumeChange;
        }

        public static void Up( int volumeChange = 5 )
        {
            defaultPlaybackDevice.Volume += volumeChange;
        }

        public static void Set( int volumeLevel )
        {
            defaultPlaybackDevice.Volume = volumeLevel;
        }
    }
}
