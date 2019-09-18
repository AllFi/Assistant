using System;
using System.Device.Location;
using System.Threading;

namespace WeatherPlugin.WeatherProviders
{
    public abstract class WeatherProvider
    {
        public abstract WeatherResponse GetCurrentWeather();

        private static GeoCoordinateWatcher _watcher;
        private GeoCoordinateWatcher Watcher
        {
            get
            {
                if ( _watcher != null )
                    return _watcher;
                _watcher = new GeoCoordinateWatcher();
                _watcher.TryStart( false, TimeSpan.FromMilliseconds( 10000 ) );
                while ( _watcher.Status != GeoPositionStatus.Ready )
                    Thread.Sleep( 50 );
                return _watcher;
            }
        }

        protected void GetCurrentGeoCoordinates( out double latitude, out double longitude )
        {
            var coord = Watcher.Position.Location;
            latitude = coord.Latitude;
            longitude = coord.Longitude;
        }
    }
}
