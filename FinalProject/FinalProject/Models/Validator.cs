using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Models
{
    class Validator
    {
        public Validator()
        {
            
        }

        public bool IsValidPhone(string input)
        {
            return !string.IsNullOrEmpty(input) && input.Length == 10 && input.All(char.IsDigit);
        }
    }
}
