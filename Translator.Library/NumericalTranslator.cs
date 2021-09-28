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

                // Casted to decimal first as it replaces any input given in scientific notation.
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

        private string ParseWholeNumber(double input, int block_of_thousand = 0)
        {
            if (input < 1000)
            {
                var result = ParseWholeNumberLessThanThousand(input, false);
                if (!string.IsNullOrWhiteSpace(result))
                {
                    return $"{result}{BaseTenHelperMethods.GetBaseTenName(block_of_thousand)}";
                }
            }

            string chunk = ParseWholeNumberLessThanThousand(input % 1000, true);
            if (!string.IsNullOrWhiteSpace(chunk))
            {
                chunk = " " + chunk + BaseTenHelperMethods.GetBaseTenName(block_of_thousand);
            }
            return ParseWholeNumber(input / 1000, block_of_thousand += 3) + chunk;
        }

        private static string ParseWholeNumberLessThanThousand(double input, bool requires_appending_to_output)
        {
            string output = "";
            switch ((int)input)
            {
                case 0:
                    return "";
                case var val when input < 10:
                {
                    output = $"{BaseTenHelperMethods.GetTensValue(val)}";
                    break;
                }
                case var val when input < 20:
                {
                    output = $"{BaseTenHelperMethods.GetTeensValue(val)}";
                    break;
                }
                case var val when input < 100:
                {
                    output = BaseTenHelperMethods.GetHundredsValue(val / 10);
                    var tens = BaseTenHelperMethods.GetTensValue(val % 10);

                    if (!string.IsNullOrWhiteSpace(tens))
                    {
                        output += $"-{tens}";
                    }
                    break;
                }
                case var val when input < 1000:
                {
                    return $"{BaseTenHelperMethods.GetTensValue(val / 100)} Hundred " + 
                                ParseWholeNumberLessThanThousand(val - (100 * (val / 100)), true);
                }
                default:
                    throw new ArgumentException("Invalid number given. This is written to accept < 1000");
            }

            if (requires_appending_to_output)
            {
                output = $"and {output}";
            }
            return output;
        }
    }
}
