using Assystant.SystemHelpers;
using NAudio.Lame;
using NAudio.Wave;
using System;
using System.IO;
using System.Speech.Recognition;
using System.Threading;

namespace VoiceUI.Helpers
{
    public class NAudioRecorder
    {
        private DateTime _lastSpeechTime { get; set; }
        private bool _commandInProgress = false;

        public void HandleAudioStateChanging( object sender, AudioStateChangedEventArgs e )
        {
            if ( e.AudioState == AudioState.Speech )
                _lastSpeechTime = DateTime.Now;
        }

        public byte[] Record()
        {
            if ( _commandInProgress )
                return new byte[0];

            MakeSignal();
            StartListening( out bool wasMuted );
            using ( var waveIn = new WaveInEvent() )
            {
                waveIn.DeviceNumber = 0;
                waveIn.WaveFormat = new WaveFormat( 8000, 1 );
                waveIn.BufferMilliseconds = 250;

                using ( var ms = new MemoryStream() )
                {
                    using ( var writer = new WaveFileWriter( ms, waveIn.WaveFormat ) )
                    {
                        waveIn.DataAvailable += new EventHandler<WaveInEventArgs>( ( sender, e ) => writer.Write( e.Buffer, 0, e.Buffer.Length ) );
                        waveIn.StartRecording();

                        Thread.Sleep( 1000 );
                        _lastSpeechTime = DateTime.Now;
                        while ( DateTime.Now - _lastSpeechTime < TimeSpan.FromSeconds( 1 ) )
                        {
                            Thread.Sleep( 1000 );
                        }
                        waveIn.StopRecording();
                    }

                    FinishListening( wasMuted );
                    return ConvertWavToMp3( ms.ToArray() );
                }
            }
        }

        private void StartListening( out bool wasMuted )
        {
            _commandInProgress = true;
            wasMuted = VolumeHelper.IsMuted();
            if ( !wasMuted ) VolumeHelper.Mute();
        }

        private void FinishListening( bool wasMuted )
        {
            if ( !wasMuted ) VolumeHelper.UnMute();
            _commandInProgress = false;
        }

        private static void MakeSignal()
        {
            using ( var ms = File.OpenRead( "Resources/signal.mp3" ) )
            using ( var rdr = new Mp3FileReader( ms ) )
            using ( var wavStream = WaveFormatConversionStream.CreatePcmStream( rdr ) )
            using ( var baStream = new BlockAlignReductionStream( wavStream ) )
            using ( var waveOut = new WaveOut( WaveCallbackInfo.FunctionCallback() ) )
            {
                waveOut.Init( baStream );
                waveOut.Play();
                while ( waveOut.PlaybackState == PlaybackState.Playing )
                {
                    Thread.Sleep( 50 );
                }
            }
        }

        private static byte[] ConvertWavToMp3( byte[] wavFile )
        {
            using ( var retMs = new MemoryStream() )
            using ( var ms = new MemoryStream( wavFile ) )
            using ( var rdr = new WaveFileReader( ms ) )
            using ( var wtr = new LameMP3FileWriter( retMs, rdr.WaveFormat, quality: LAMEPreset.STANDARD ) )
            {
                rdr.CopyTo( wtr );
                return retMs.ToArray();
            }
        }
    }
}
