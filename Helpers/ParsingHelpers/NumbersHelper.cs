using System;
using System.Linq;

namespace ParsingHelpers
{
    public static class NumbersHelper
    {
        private static string[] _singles = new string[] { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };
        private static string[] _teens = new string[] { "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
        private static string[] _tens = new string[] { "", "", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninty" };
        private static string[] _powers = new string[] { "", "thousand", "million", "billion", "trillion", "quadrillion", "quintillion" };

        public static int ParseNumber( string words ) => ( int )ParseNumberInternal( words );

        public static bool IsNumber( string word )
        {
            return _singles.Contains( word ) || _teens.Contains( word ) || _tens.Contains( word ) || _powers.Contains( word );
        }

        // https://www.programmingalgorithms.com/algorithm/words-to-numbers
        private static ulong ParseNumberInternal( string words )
        {
            if ( string.IsNullOrEmpty( words ) ) return 0;

            words = words.Trim();
            words += ' ';

            ulong number = 0;

            for ( int i = _powers.Length - 1; i >= 0; i-- )
            {
                if ( !string.IsNullOrEmpty( _powers[i] ) )
                {
                    int index = words.IndexOf( _powers[i] );

                    if ( index >= 0 && words[index + _powers[i].Length] == ' ' )
                    {
                        ulong count = ParseNumberInternal( words.Substring( 0, index ) );
                        number += count * ( ulong )Math.Pow( 1000, i );
                        words = words.Remove( 0, index );
                    }
                }
            }

            {
                int index = words.IndexOf( "hundred" );

                if ( index >= 0 && words[index + "hundred".Length] == ' ' )
                {
                    ulong count = ParseNumberInternal( words.Substring( 0, index ) );
                    number += count * 100;
                    words = words.Remove( 0, index );
                }
            }

            for ( int i = _tens.Length - 1; i >= 0; i-- )
            {
                if ( !string.IsNullOrEmpty( _tens[i] ) )
                {
                    int index = words.IndexOf( _tens[i] );

                    if ( index >= 0 && words[index + _tens[i].Length] == ' ' )
                    {
                        number += ( uint )( i * 10 );
                        words = words.Remove( 0, index );
                    }
                }
            }

            for ( int i = _teens.Length - 1; i >= 0; i-- )
            {
                if ( !string.IsNullOrEmpty( _teens[i] ) )
                {
                    int index = words.IndexOf( _teens[i] );

                    if ( index >= 0 && words[index + _teens[i].Length] == ' ' )
                    {
                        number += ( uint )( i + 10 );
                        words = words.Remove( 0, index );
                    }
                }
            }

            for ( int i = _singles.Length - 1; i >= 0; i-- )
            {
                if ( !string.IsNullOrEmpty( _singles[i] ) )
                {
                    int index = words.IndexOf( _singles[i] + ' ' );

                    if ( index >= 0 && words[index + _singles[i].Length] == ' ' )
                    {
                        number += ( uint )( i );
                        words = words.Remove( 0, index );
                    }
                }
            }

            return number;
        }
    }
}
