using HomeWork_10.Core.Base.Person;
using HomeWork_10.Core.Interfaces.Iemployee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace HomeWork_10.Core.Classes.Employee
{
    public class Employee : Person, Iemployee
    {
        public Employee() { }

        public Employee(int id, string name, string surname, int salary) : base(id, name, surname)
        {
            this.salary = salary;
        }

        public int salary { get; set; }
    }
}
