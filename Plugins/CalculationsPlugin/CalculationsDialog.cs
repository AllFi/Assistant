using ParsingHelpers;
using Syn.Bot.Oscova;
using Syn.Bot.Oscova.Attributes;

namespace CalculationsPlugin
{
    public class CalculationsBotPlugin : OscovaPlugin
    {
        public CalculationsBotPlugin( OscovaBot bot ) : base( bot )
        {
            bot.Dialogs.Add( new CalculationsDialog() );
        }
    }

    public class CalculationsDialog : Dialog
    {
        [Expression( "@sys.text {плюс} @sys.text" )]
        [Expression( "@sys.text {умножить на} @sys.text" )]
        [Expression( "@sys.text {делить на} @sys.text" )]
        [Expression( "@sys.text {минус} @sys.text" )]
        [Entity( "operation" )]
        public void TurnOn( Context context, Result result )
        {
            var numbers = result.Entities.AllOfType( Sys.Text );
            if ( numbers.Count < 2 )
                result.SendResponse( "Повторите пожалуйста!" );

            var firstNum = NumbersHelper.ParseNumber( numbers[0].Value );
            var secondNum = NumbersHelper.ParseNumber( numbers[1].Value );

            var operation = result.Entities.OfType( "operation" ).Value;

            switch ( operation )
            {
                case "плюс":
                    result.SendResponse( ( firstNum + secondNum ).ToString() );
                    return;
                case "умножить на":
                    result.SendResponse( ( firstNum * secondNum ).ToString() );
                    return;
                case "делить на":
                    result.SendResponse( ( ( double )firstNum / secondNum ).ToString() );
                    return;
                case "минус":
                    result.SendResponse( ( firstNum - secondNum ).ToString() );
                    return;
            }
        }
    }
}
