using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MusicPlugin.Utils
{
    public static class Volume
    {
        [DllImport( "user32.dll" )]
        static extern void keybd_event( byte bVk, byte bScan, uint dwFlags, int dwExtraInfo );

        public static void Mute()
        {
            keybd_event( ( byte )Keys.VolumeMute, 0, 0, 0 );
        }

        public static void Down( int volumeChange = 6 )
        {
            for ( int i = 0; i < volumeChange / 2; i++ )
                keybd_event( ( byte )Keys.VolumeDown, 0, 0, 0 );
        }

        public static void Up( int volumeChange = 6 )
        {
            for ( int i = 0; i < volumeChange / 2; i++ )
                keybd_event( ( byte )Keys.VolumeUp, 0, 0, 0 );
        }
    }
}
