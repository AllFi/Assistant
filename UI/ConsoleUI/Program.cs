using AssistantCore;
using AssistantCore.Russian;
using System;
using System.Collections.Generic;
using System.IO;

namespace ConsoleUI
{
    class Program
    {
        static void Main( string[] args )
        {
            Assistant assistant = new Assistant( $"{Directory.GetCurrentDirectory()}/Plugins", Receive, $"{Directory.GetCurrentDirectory()}/RussianLanguage/WordNet" );
            assistant.Start();

            var stemmer = new RussianStemmer();

            while ( true )
            {
                Console.Write( "Вы: " );
                var request = Console.ReadLine().Replace( "Вы: ", "" );
                assistant.HandleRequest( request );
            }
            //Clear();

            //Console.ReadLine();
            //Console.ReadKey();
        }

        public static void Receive( AssistantResponse response )
        {
            Console.WriteLine( $"Бот: {response.Text}" );
        }
    }
}
