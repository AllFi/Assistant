using Syn.Bot.Oscova;
using Syn.Bot.Oscova.Attributes;
using System.Collections.Generic;
using WeatherPlugin.WeatherProviders;

namespace WeatherPlugin
{
    public class WeatherBotPlugin : OscovaPlugin
    {
        public WeatherBotPlugin( OscovaBot bot ) : base( bot )
        {
            bot.Dialogs.Add( new WeatherDialog() );
        }
    }

    public class WeatherDialog : Dialog
    {
        private List<WeatherProvider> _weatherProviders = new List<WeatherProvider>();
        private WeatherProvider _mainWeatherProvider = null;

        public WeatherDialog()
        {
            _weatherProviders.Add( new OpenWeatherMap() );
            _weatherProviders.Add( new GismeteoWeather() );
            _mainWeatherProvider = _weatherProviders[0];
        }

        [Expression( "погода за окном" )]
        public void TurnOn( Context context, Result result )
        {
            result.SendResponse( _mainWeatherProvider.GetCurrentWeather().ToHumanString() );
        }
    }
}
