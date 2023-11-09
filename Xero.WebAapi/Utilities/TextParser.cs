using Microsoft.CodeAnalysis.Operations;
using System.Text.RegularExpressions;
using Xero.WebAapi.DataModels;
using Xero.WebAapi.Models;

namespace Xero.WebAapi.Utilities
{
    public class TextParser
    {
        public static bool TryGetInvoiceDate(string input, out DateTime invoicedate)
        {
            invoicedate = default;
            bool match = false;
            try
            {
                input = input.Trim();
                input = input.Remove(0, 12).Trim();

                string pattern = @"^[a-zA-Z]+ [0-9]{2}, [0-9]{4}";
                match = Regex.IsMatch(input, pattern);
                if (match)
                    invoicedate = DateTime.Parse(input);

            }
            catch {
                return false;
            }
            return match;
        }

        public static string GetCompanyName(string input)
        {
            return input.Trim();            
        }

        public static Currency TryGetCurrency(string input)
        {
            var result = Currency.NONE;
            switch(input)
            {
                case "USD": 
                    result= Currency.USD;
                    break;
                case "CAD":
                    result = Currency.CAD;
                    break;
                case "GBP":
                    result = Currency.GBP;
                    break;
            }

            return result;
        }

        public static bool TryGetRequiredData(string input, out string outputType, out decimal output)
        {

            input = input.Trim().Replace(" ", "");

            output = 0;
            outputType = "";

            bool match = false;
            try
            {
                
                if (input.Contains("TotalDue"))
                {                   
                    var res = input.Replace("TotalDue", "");
                    Decimal.TryParse(res.Remove(0, 1), out output);
                    outputType = "TotalDue";
                    match = true;
                }
                else if(input.Contains("Total"))
                {
                    var res = input.Replace("Total", "");
                    Decimal.TryParse(res.Remove(0, 1), out output);
                    outputType = "Total";
                    match = true;
                }
                else if (input.Contains("Tax0%"))
                {                   
                    var res = input.Replace("Tax0%", "");
                    Decimal.TryParse(res.Remove(0, 1), out output);
                    outputType = "Tax";
                    match = true;
                }
                else if (input.Contains("GST13%"))
                {                    
                    var res = input.Replace("GST13%", "");
                    Decimal.TryParse(res.Remove(0, 1), out output);
                    outputType = "Tax";
                    match = true;
                }
            }
            catch (Exception ex)
            {
               
            }


            return match;
        }
    }
}
