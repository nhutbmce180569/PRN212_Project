using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FinalProject.Models
{
    class Validator
    {
        public Validator()
        {
            
        }
        public bool IsValidPhone(string? input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return false;
            }
            return !string.IsNullOrEmpty(input) && (input.Length == 10 || input.Length == 11) && input.All(char.IsDigit);
        }

        public bool IsValidOrganiztionName(string? input, int lenght)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return false;
            }
            return !string.IsNullOrEmpty(input) && input.Length <= lenght;
        }

        public bool IsValidPersonName(string? input, int lenght)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return false;
            }
            return !string.IsNullOrEmpty(input) && input.Length <= lenght && input.All(char.IsDigit);
        }

        public bool IsValidTaxId(string? input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return false;
            }
            return !string.IsNullOrEmpty(input) && input.Length == 10 && input.All(char.IsDigit);
        }

        public bool IsValidEmail(string? email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return false;
            }

            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            return Regex.IsMatch(email, pattern);
        }

        public bool IsValidNumberAmount(long price, long min)
        {
            if (price < min)
            {
                return false;
            }

            return true;
        }

    }
}
