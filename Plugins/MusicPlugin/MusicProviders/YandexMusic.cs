using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Threading;

namespace MusicPlugin.MusicProviders
{
    public class YandexMusic : MusicProvider
    {
        public override string Name => "Яндекс Музыка";
        private ChromeDriver _chromeDriver { get; set; }

        public override void Previous()
        {
            var previousButton = _chromeDriver.FindElement( By.XPath( @"//*[@id=""nb-1""]/body/div[1]/div[10]/div[1]/div[2]/div[1]" ) );
            previousButton.Click();
        }

        public override void Pause()
        {
            var pauseButton = _chromeDriver.FindElement( By.XPath( @"//*[@id=""nb-1""]/body/div[1]/div[10]/div[1]/div[2]/div[3]" ) );
            pauseButton.Click();
        }

        public override void Play()
        {
            var playButton = _chromeDriver.FindElement( By.XPath( @"//*[@id=""nb-1""]/body/div[1]/div[10]/div[1]/div[2]/div[3]" ) );
            playButton.Click();
        }

        public override void Next()
        {
            var nextButton = _chromeDriver.FindElement( By.XPath( @"//*[@id=""nb-1""]/body/div[1]/div[10]/div[1]/div[2]/div[4]" ) );
            nextButton.Click();
        }

        public override void TurnOn( string musicRequest )
        {
            InitChromeDriverIfNeccessary();
            _chromeDriver.Url = @"https://music.yandex.ru/home";

            var searchInput = _chromeDriver.FindElement( By.XPath( @"//*[@id=""nb-1""]/body/div[1]/div[3]/div/div[1]/div[1]/div/div[1]/input" ) );
            searchInput.SendKeys( $"{musicRequest}" );

            var searchButton = _chromeDriver.FindElement( By.XPath( @"//*[@id=""nb-1""]/body/div[1]/div[3]/div/div[1]/div[1]/div/div[1]/button" ) );
            searchButton.Click();

            while ( true )
            {
                string firstXPath = @"//*[@id=""nb-1""]/body/div[1]/div[6]/div[2]/div/div/div[2]/div[2]/div[1]/div/div[1]/div[1]/span[2]/button";
                string secondXPath = @"//*[@id=""nb-1""]/body/div[1]/div[6]/div[2]/div/div/div[3]/div[2]/div[1]/div/div[1]/div[1]/span[2]/button";
                IWebElement button = null;
                if ( TryGetMusicianButton( _chromeDriver, firstXPath, out button ) || TryGetMusicianButton( _chromeDriver, secondXPath, out button ) )
                {
                    button.Click();
                    break;
                }
            }

            ThreadPool.QueueUserWorkItem( ( obj ) =>
            {
                for ( int i = 0; i < 100; i++ )
                {
                    if ( _chromeDriver.WindowHandles.Count > 1 )
                    {
                        _chromeDriver.SwitchTo().Window( _chromeDriver.WindowHandles[1] ).Close();
                        _chromeDriver.SwitchTo().Window( _chromeDriver.WindowHandles[0] );
                        break;
                    }
                }
            } );

            _chromeDriver.Manage().Window.Size = new System.Drawing.Size( 500, 200 );
            //_chromeDriver.Manage().Window.Position = new System.Drawing.Point( -1000, -1000 );
        }

        public override void TurnOff()
        {
            _chromeDriver.Close();
            _chromeDriver = null;
        }

        private void InitChromeDriverIfNeccessary()
        {
            if ( _chromeDriver == null )
            {
                var service = ChromeDriverService.CreateDefaultService();
                service.HideCommandPromptWindow = true;
                ChromeOptions options = new ChromeOptions();
                options.AddExcludedArgument( "enable-automation" );
                options.AddAdditionalCapability( "useAutomationExtension", false );
                options.AddArgument( "--log-level=3" );
                _chromeDriver = new ChromeDriver( service, options );
            }
        }

        private bool TryGetMusicianButton( IWebDriver driver, string xpath, out IWebElement button )
        {
            try
            {
                button = driver.FindElement( By.XPath( xpath ) );
                return true;
            }
            catch
            {
                button = null;
                return false;
            }
        }
    }
}
