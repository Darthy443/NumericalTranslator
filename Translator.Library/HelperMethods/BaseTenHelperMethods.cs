using System;

namespace Translator.Library.HelperMethods
{
    public static class BaseTenHelperMethods
    {
        public static string GetBaseTenName(int col_value) => col_value switch
        {
            1 => " Ten",
            2 => " Hundred",
            3 => " Thousand",
            4 => " Ten-Thousand",
            5 => " Hundred-Thousand",
            6 => " Million",
            7 => " Ten-Million",
            8 => " Hundred-Million",
            9 => " Billion",
            10 => " Ten-Billion",
            11 => " Hundred-Billion",
            12 => " Trillion",
            13 => " Ten-Trillion",
            14 => " Hundred-Trillion",
            15 => " Quadrillion",
            0 => "",
            _ => throw new ArgumentException("Value outside of expected range.")
        };

        
        public static string GetHundredsValue(int value) => value switch {
            1 => "Ten",
            2 => "Twenty",
            3 => "Thirty",
            4 => "Fourty",
            5 => "Fifty",
            6 => "Sixty",
            7 => "Seventy",
            8 => "Eighty",
            9 => "Ninety",
            0 => "",
            _ => ""
        };

        public static string GetTeensValue(int teen_value) => teen_value switch
        {
            10 => "Ten",
            11 => "Eleven",
            12 => "Twelve",
            13 => "Thirteen",
            14 => "Fourteen",
            15 => "Fifteen",
            16 => "Sixteen",
            17 => "Seventeen",
            18 => "Eighteen",
            19 => "Nineteen",
            _ => throw new ArgumentException("Unexpected value. This should be given a number from 10 -> 19")
        };

        public static string GetTensValue(int number) => number switch
        {
            1 => "One",
            2 => "Two",
            3 => "Three",
            4 => "Four",
            5 => "Five",
            6 => "Six",
            7 => "Seven",
            8 => "Eight",
            9 => "Nine",
            0 => "",
            _ => throw new ArgumentException("Value not in range. Expects a single digit number")
        };
    }
}