using System;
using Translator.Library.HelperMethods;
using System.Linq;
using System.Globalization;
using System.Collections.Generic;

namespace Translator.Library
{
    public class NumericalTranslator
    {
        private double _WholePortion { get; set; }
        private string _DecimalPortion {get; set;}
        private bool _IsNegative {get; set;}
        private bool _IsZero {get; set;}
        public NumericalTranslator(double number_to_parse)
        {
            if (number_to_parse == 0)
            {
                this._IsZero = true;
            } else 
            {
                if (number_to_parse < 0)
                {
                    this._IsNegative = true;
                    number_to_parse = Math.Abs(number_to_parse);
                }

                this._WholePortion = Math.Truncate(number_to_parse);

                // Casted to decimal first as it removes any input given in scientific notation.
                var splitInput = ((decimal)number_to_parse).ToString(CultureInfo.InvariantCulture).Split('.');
                if (splitInput.Count() == 2)
                {
                    this._DecimalPortion = splitInput[1];
                }
            }
        }

        public string GetOutput()
        {
            if (this._IsZero)
            {
                return "Zero";
            }

            var result = "";
            if (this._WholePortion != 0)
            {
                var wholeNumResult = ParseWholeNumber(this._WholePortion);
                if (!string.IsNullOrWhiteSpace(wholeNumResult))
                {
                    wholeNumResult = CleanupOutput(wholeNumResult);
                    result += wholeNumResult;
                }
            }

            if (!string.IsNullOrWhiteSpace(this._DecimalPortion))
            {
                if (double.TryParse(this._DecimalPortion, out double convertedToDouble))
                {
                    var decimalNumResult = ParseWholeNumber(convertedToDouble);
                    if (!string.IsNullOrWhiteSpace(decimalNumResult))
                    {
                        decimalNumResult = CleanupOutput(decimalNumResult);
                        if (convertedToDouble == 1)
                        {
                            decimalNumResult += BaseTenHelperMethods.GetBaseTenName(this._DecimalPortion.Length) + "th";
                        } else
                        {
                            decimalNumResult += BaseTenHelperMethods.GetBaseTenName(this._DecimalPortion.Length) + "ths";
                        }

                        if (string.IsNullOrWhiteSpace(result))
                        {
                            result += decimalNumResult;
                        } else
                        {
                            result += $" and {decimalNumResult}";
                        }
                    } 
                }
            }

            if (this._IsNegative)
            {
                return $"Negative {result}";
            } else
            {
                return result;
            }
        }

        private string CleanupOutput(string output_to_clean)
        {
            output_to_clean = output_to_clean.TrimStart();
            output_to_clean = output_to_clean.TrimEnd();
            if (output_to_clean.StartsWith("and"))
            {
                output_to_clean = output_to_clean.Substring(4);
            }
            return output_to_clean;
        }

        private string ParseWholeNumber(double input, int block_of_thousand = 0)
        {
            if (input < 1000)
            {
                var result = ParseWholeNumberLessThanThousand(input);
                if (!string.IsNullOrWhiteSpace(result))
                {
                    return $"{result}{BaseTenHelperMethods.GetBaseTenName(block_of_thousand)}";
                }
            }

            string chunk = ParseWholeNumberLessThanThousand(input % 1000);
            if (!string.IsNullOrWhiteSpace(chunk))
            {
                chunk = " " + chunk + BaseTenHelperMethods.GetBaseTenName(block_of_thousand);
            }
            return ParseWholeNumber(input / 1000, block_of_thousand += 3) + chunk;
        }

        private static string ParseWholeNumberLessThanThousand(double input)
        {
            switch ((int)input)
            {
                case 0:
                    return "";
                case var val when input < 10:
                    return $" and {BaseTenHelperMethods.GetTensValue(val)}";
                case var val when input < 20:
                    return $" and {BaseTenHelperMethods.GetTeensValue(val)}";
                case var val when input < 100:
                    {
                        var hundreds = BaseTenHelperMethods.GetHundredsValue(val / 10);
                        var tens = BaseTenHelperMethods.GetTensValue(val % 10);

                        if (!string.IsNullOrWhiteSpace(tens))
                        {
                            return $" and {hundreds}-{tens}";
                        }
                        else
                        {
                            return $" and {hundreds}";
                        }
                    }
                case var val when input < 1000:
                    {
                        return $"{BaseTenHelperMethods.GetTensValue(val / 100)} Hundred" + ParseWholeNumberLessThanThousand(val - (100 * (val / 100)));
                    }
                default:
                    throw new ArgumentException("Invalid number given. This is written to accept < 1000");
            }
        }
    }
}
