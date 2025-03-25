using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

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
        public bool IsValidFullname(string name)
        {
            if (name.Length >= 255)
            {
                return false;
            }
            return true;
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

        public bool IsValidPassword(string? password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return false;
            string pattern = @"^(?=.*[A-Z])(?=.*[^a-zA-Z0-9]).{8,}$";
            return Regex.IsMatch(password, pattern);
        }

        public bool IsEmailExistsCustomer(string email)
        {
            using (var context = new FstoreContext())
            {
                return context.Customers.Any(e => e.Email.ToLower() == email.ToLower());
            }
        }
        public bool IsValidBirthday(DateTime day)
        {
            if (day == null)
            {
                return false;
            }
            return day <= DateTime.Today;
        }
        public bool IsValidPrice(long? price)
        {
            if (price <= 0)
            {
                return false;
            }

            return true;
        }
    }
}
