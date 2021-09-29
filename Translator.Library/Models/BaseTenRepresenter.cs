using Translator.Library.HelperMethods;
using Translator.Library;

namespace Translator.Library.Models
{
    public class BaseTenRepresenter
    {
        public int Hundred { get; private set; }
        public int Remainder { get; private set; }
        public string BaseTenName { get; private set; }

        public BaseTenRepresenter(double number, int base_ten_col)
        {
            if (number < 0 || number == 0)
            {
                return;
            }
            if (number >= 100)
            {
                this.Hundred = (int)number / 100;
            } else
            {
                this.Hundred = 0;
            }
            this.Remainder = (int)number % 100;

            this.BaseTenName =
                HelperMethods.BaseTenHelperMethods.GetBaseTenName(base_ten_col);
        }

        public string GetOutput()
        {
            var output = "";
            if (this.Hundred > 0)
            {
                output = $"{BaseTenHelperMethods.GetTensValue(this.Hundred)} Hundred";
            }
            var remainder = this.GetRemainder();
            if (!string.IsNullOrWhiteSpace(remainder))
            {
                output = output.AppendWithSeperator(" and ", remainder);
                // if (!string.IsNullOrWhiteSpace(output))
                // {
                //     output += $"{output} and {remainder}";
                // } else
                // {
                //     output += remainder;
                // }
            }
            
            if (!string.IsNullOrWhiteSpace(output) && !string.IsNullOrWhiteSpace(this.BaseTenName))
            {
                output = output.AppendWithSeperator(" ", this.BaseTenName);
                // output += $" {this.BaseTenName}";
            }

            return output;
        }

        private string GetRemainder()
        {
            if (this.Remainder == 0)
            {
                return "";
            }

            var remainderString = "";
            if (this.Remainder < 10)
            {
                remainderString = BaseTenHelperMethods.GetTensValue(this.Remainder);
            } else if (this.Remainder < 20)
            {
                remainderString = BaseTenHelperMethods.GetTeensValue(this.Remainder);
            } else 
            {
                var hundreds = BaseTenHelperMethods.GetHundredsValue(this.Remainder / 10);
                var tens = BaseTenHelperMethods.GetTensValue(this.Remainder % 10);

                if (!string.IsNullOrWhiteSpace(tens))
                {
                    remainderString = $"{hundreds}-{tens}";
                } else
                {
                    remainderString = hundreds;
                }
            }

            return remainderString;
        }
    }
}