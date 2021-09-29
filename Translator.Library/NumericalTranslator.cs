using System;
using Translator.Library.HelperMethods;
using System.Linq;
using System.Globalization;
using System.Collections.Generic;
using Translator.Library.Models;

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
                result = OutputWholeComponent();
            }

            var fractionalComponent = OutputFractionalComponent();
            if (!string.IsNullOrWhiteSpace(fractionalComponent))
            {
                result = result.AppendWithSeperator(" and ", fractionalComponent);
            }

            if (this._IsNegative)
            {
                return $"Negative {result}";
            } else
            {
                return result;
            }
        }

        private string OutputWholeComponent()
        {
            return ParseWholeNumber(this._WholePortion);
        }

        private string ParseWholeNumber(double number_to_parse)
        {
            var numberInThousands = TransformNumberIntoBaseTenThousands(number_to_parse, new List<BaseTenRepresenter>());
            if (numberInThousands != null)
            {
                if (numberInThousands.Count() == 1)
                {
                    return numberInThousands[0].GetOutput();
                } else 
                {
                    var output = "";
                    for(int i = numberInThousands.Count() - 1; i > -1; i--)
                    {
                        var number = numberInThousands[i].GetOutput();
                        if (string.IsNullOrWhiteSpace(number))
                            continue;

                        if (i == 0)
                        {
                            if (numberInThousands[i].Hundred == 0)
                            {
                                output =  output.AppendWithSeperator(" and ", number);
                            } else
                            {
                                output =  output.AppendWithSeperator(" ", number);
                            }
                        } else
                        {
                            output =  output.AppendWithSeperator(" ", number);
                        }
                    }
                    return output;
                }
            }
            return "";
        }

        private string OutputFractionalComponent()
        {
            var result = "";
            if (!string.IsNullOrWhiteSpace(this._DecimalPortion))
            {
                if (double.TryParse(this._DecimalPortion, out double convertedToDouble))
                {
                    result = ParseWholeNumber(convertedToDouble);
                    if (!string.IsNullOrWhiteSpace(result))
                    {
                        if (convertedToDouble == 1)
                        {
                            result += " " + BaseTenHelperMethods.GetBaseTenName(this._DecimalPortion.Length) + "th";
                        } else
                        {
                            result += " " + BaseTenHelperMethods.GetBaseTenName(this._DecimalPortion.Length) + "ths";
                        }
                    } 
                }
            }

            return result;
        }

        private List<BaseTenRepresenter> TransformNumberIntoBaseTenThousands(double input, List<BaseTenRepresenter> output, int block_of_thousand = 0)
        {
            if (input == 0)
            {
                return output;
            }
 
            if (input < 1000)
            {
                output.Add(new BaseTenRepresenter(input, block_of_thousand));
                return output;
            }
            
            if ((input % 1000) > 0)
                output.Add(new BaseTenRepresenter(input % 1000, block_of_thousand));
 
            return TransformNumberIntoBaseTenThousands(input / 1000, output, block_of_thousand += 3);
        }
    }
}
