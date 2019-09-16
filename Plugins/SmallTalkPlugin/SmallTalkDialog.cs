using OpenQA.Selenium.Chrome;
using Syn.Bot.Oscova;
using Syn.Bot.Oscova.Attributes;
using System.Threading;

namespace SmallTalkPlugin
{
    public class SmallTalkBotPlugin : OscovaPlugin
    {
        public SmallTalkBotPlugin( OscovaBot bot ) : base( bot )
        {
            bot.Dialogs.Add( new SmallTalkDialog() );
        }
    }

    public class SmallTalkDialog : Dialog
    {
        private ChromeDriver _chromeDriver { get; set; }

        public SmallTalkDialog()
        {
            InitChromeDriverIfNeccessary();
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
                _chromeDriver.Url = @"http://p-bot.ru/";
                _chromeDriver.FindElementByXPath( @"//*[@id=""btnSay""]" ).Click();
            }
        }

        [Fallback]
        public void SmallTalk( Context context, Result result )
        {
            var request = result.Request.Text;
            _chromeDriver.FindElementByXPath( @"//*[@id=""user_request""]" ).SendKeys( request );
            _chromeDriver.FindElementByXPath( @"//*[@id=""btnSay""]" ).Click();
            
            var output = _chromeDriver.FindElementByXPath( @"//*[@id=""answer_0""]" ).Text.Replace( "ρBot: ", "" );
            while ( output == "думаю..." )
            {
                Thread.Sleep( 100 );
                output = _chromeDriver.FindElementByXPath( @"//*[@id=""answer_0""]" ).Text.Replace( "ρBot: ", "" );
            }

            var lastLength = 0;
            do
            {
                lastLength = output.Length;
                Thread.Sleep( 100 );
                output = _chromeDriver.FindElementByXPath( @"//*[@id=""answer_0""]" ).Text.Replace( "ρBot: ", "" );
            }
            while ( output.Length != lastLength );

            result.SendResponse( output );
        }
    }
}
