using AssistantCore;
using System;
using System.IO;

namespace ConsoleUI
{
    class Program
    {
        static void Main( string[] args )
        {
            Assistant assistant = new Assistant( $"{Directory.GetCurrentDirectory()}/Plugins", Receive, $"{Directory.GetCurrentDirectory()}/WordNet" );
            assistant.Start();

            while ( true )
            {
                Console.Write( "Вы: " );
                var request = Console.ReadLine().Replace( "Вы: ", "" );
                assistant.HandleRequest( request );
            }
        }

        public static void Receive( AssistantResponse response )
        {
            Console.WriteLine( $"Бот: {response.Text}" );
        }
    }
}
