using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HomeWork_10.Utils.Validators.EmailValidator
{
    internal static class EmailValidator
    {

        public static bool CheckEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            int at_counter = email.Count(x => x == '@');
            int dot_counter = email.Count(x => x == '.');

            var Regx = new Regex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"); //using regular expression for more realism ;)

            if (!Regx.IsMatch(email))
                return false; 


            return true;
        }
    }
}
