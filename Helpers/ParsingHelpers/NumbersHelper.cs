using System.Collections.Generic;
using System.Linq;

namespace ParsingHelpers
{
    public static class NumbersHelper
    {
        private static Dictionary<string, long> _singles = new Dictionary<string, long>
        {
            ["ноль"] = 0,
            ["один"] = 1,
            ["одна"] = 1,
            ["два"] = 2,
            ["две"] = 2,
            ["три"] = 3,
            ["четыре"] = 4,
            ["пять"] = 5,
            ["шесть"] = 6,
            ["семь"] = 7,
            ["восемь"] = 8,
            ["девять"] = 9
        };

        private static Dictionary<string, long> _teens = new Dictionary<string, long>
        {
            ["десять"] = 10,
            ["одиннадцать"] = 11,
            ["двенадцать"] = 12,
            ["тринадцать"] = 13,
            ["четырнадцать"] = 14,
            ["пятнадцать"] = 15,
            ["шестнадцать"] = 16,
            ["семнадцать"] = 17,
            ["восемнадцать"] = 18,
            ["девятнадцать"] = 19,
        };

        private static Dictionary<string, long> _tens = new Dictionary<string, long>
        {
            ["двадцать"] = 20,
            ["тридцать"] = 30,
            ["сорок"] = 40,
            ["пятьдесят"] = 50,
            ["шестьдесят"] = 60,
            ["семьдесят"] = 70,
            ["восемьдесят"] = 80,
            ["девяносто"] = 90
        };

        private static Dictionary<string, long> _hundreds = new Dictionary<string, long>
        {
            ["сто"] = 100,
            ["двести"] = 200,
            ["триста"] = 300,
            ["четыреста"] = 400,
            ["пятьсот"] = 500,
            ["шестьсот"] = 600,
            ["семьсот"] = 700,
            ["восемьсот"] = 800,
            ["девятьсот"] = 900,
        };

        private static Dictionary<string, long> _powers = new Dictionary<string, long>
        {
            ["тысяч"] = 1000,
            ["тысяча"] = 1000,
            ["тысячи"] = 1000,
            ["миллион"] = 1000000,
            ["миллиона"] = 1000000,
            ["миллионов"] = 1000000,
            ["миллиард"] = 1000000000,
            ["миллиарда"] = 1000000000,
            ["миллиардов"] = 1000000000,
            ["триллион"] = 1000000000000,
            ["триллиона"] = 1000000000000,
            ["триллионов"] = 1000000000000,
            ["квадриллион"] = 1000000000000000,
            ["квадриллиона"] = 1000000000000000,
            ["квадриллионов"] = 1000000000000000,
        };

        public static bool IsNumber( this string russianWords )
        {
            return ParseNumber( russianWords ) > 0;
        }

        public static long ParseNumber( this string russianWords )
        {
            if ( string.IsNullOrEmpty( russianWords ) )
                return 0;

            var words = russianWords.Trim().Split( ' ' );
            if ( words.Count() == 0 )
                return 0;

            long number = 0;
            long acc = 0;

            foreach ( var word in words )
            {
                if ( _powers.ContainsKey( word ) )
                {
                    acc = acc > 0 ? acc : 1;
                    number += acc * _powers[word];
                    acc = 0;
                }

                if ( _hundreds.ContainsKey( word ) )
                    acc += _hundreds[word];

                if ( _tens.ContainsKey( word ) )
                    acc += _tens[word];

                if ( _teens.ContainsKey( word ) )
                    acc += _teens[word];

                if ( _singles.ContainsKey( word ) )
                    acc += _singles[word];
            }

            number += acc;
            return number;
        }
    }
}
