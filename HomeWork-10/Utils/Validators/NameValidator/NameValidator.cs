using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace HomeWork_8.Utils.Validators.DataValidators.UserNameValidator
{
    internal static class NameValidator
    {
        public static bool CheckName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return false;

            else if (name.Length < 3)
                return false;

            else return true;
        }
    }
}