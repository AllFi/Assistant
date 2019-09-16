using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Threading;

namespace MusicPlugin.MusicProviders
{
    public class YandexMusic : MusicProvider
    {
        public override string Name => "Яндекс Музыка";
        private ChromeDriver _chromeDriver { get; set; }

        public YandexMusic()
        {
            InitChromeDriverIfNeccessary();
        }

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
            var searchInput = _chromeDriver.FindElement( By.XPath( @"//*[@id=""nb-1""]/body/div[1]/div[3]/div/div[1]/div[1]/div/div[1]/input" ) );
            searchInput.Clear();
            searchInput.SendKeys( $"{musicRequest}" );

            var searchButton = _chromeDriver.FindElement( By.XPath( @"//*[@id=""nb-1""]/body/div[1]/div[3]/div/div[1]/div[1]/div/div[1]/button" ) );
            searchButton.Click();

            var firstResultBy = By.XPath( @"//*[@id=""nb-1""]/body/div[1]/div[6]/div[2]/div/div/div[2]/div[2]/div[1]/div/div[1]/div[1]/span[2]/button" );
            var secondResultBy = By.XPath( @"//*[@id=""nb-1""]/body/div[1]/div[6]/div[2]/div/div/div[3]/div[2]/div[1]/div/div[1]/div[1]/span[2]/button" );
            var button = FindElementsWithWaiting( _chromeDriver, 10000, firstResultBy, secondResultBy );
            button.Click();
        }

        public override void TurnOff()
        {
            _chromeDriver.Url = @"https://music.yandex.ru/home";
        }

        public override string WhoIsIt()
        {
            var title = _chromeDriver.FindElement( By.XPath( @"//*[@id=""nb-1""]/body/div[1]/div[10]/div[1]/div[2]/div[6]/div/div/div[1]/div[2]/div/div[1]/a" ) );
            var group = _chromeDriver.FindElement( By.XPath( @"//*[@id=""nb-1""]/body/div[1]/div[10]/div[1]/div[2]/div[6]/div/div/div[1]/div[2]/div/div[2]/span/a" ) );
            return $"Песня {title.Text} группы {group.Text}";
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
                //_chromeDriver.Manage().Window.Position = new System.Drawing.Point( -1000, -1000 );
                _chromeDriver.Url = @"https://music.yandex.ru/home";

                // signing in
                var signInButton = _chromeDriver.FindElement( By.XPath( @"//*[@id=""nb-1""]/body/div[1]/div[3]/div/div[2]/a" ) );
                signInButton.Click();

                _chromeDriver.SwitchTo().Window( _chromeDriver.WindowHandles[1] );

                var loginInput = _chromeDriver.FindElement( By.XPath( @"//*[@id=""passp-field-login""]" ) );
                loginInput.SendKeys( "symba.assistant" );

                var submitLoginButton = _chromeDriver.FindElement( By.XPath( @"//*[@id=""root""]/div/div/div[2]/div/div[2]/div[3]/div[2]/div/div/div[1]/form/div[3]/button[1]" ) );
                submitLoginButton.Click();

                var passwordInput = FindElementsWithWaiting( _chromeDriver, 10000, By.XPath( @"//*[@id=""passp-field-passwd""]" ) );
                passwordInput.SendKeys( "008zxc008zxc" );

                var submitPasswordButton = _chromeDriver.FindElement( By.XPath( @"//*[@id=""root""]/div/div/div[2]/div/div[2]/div[3]/div[2]/div/div/form/div[2]/button[1]" ) );
                submitPasswordButton.Click();

                _chromeDriver.SwitchTo().Window( _chromeDriver.WindowHandles[0] );
            }
        }

        private IWebElement FindElementsWithWaiting( IWebDriver driver, int timeoutMs, params By[] byArray )
        {
            IWebElement element;
            for ( int i = 0; i < timeoutMs / 50; i++ )
            {
                foreach ( var by in byArray )
                {
                    if ( TryGetElement( driver, by, out element ) )
                    {
                        return element;
                    }
                }
                Thread.Sleep( 50 );
            }
            return null;
        }

        private bool TryGetElement( IWebDriver driver, By by, out IWebElement element )
        {
            try
            {
                element = driver.FindElement( by );
                return true;
            }
            catch ( Exception ex )
            {
                element = null;
                return false;
            }
        }
    }
}
