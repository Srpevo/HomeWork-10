using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeWork_10.Core.Base.Person
{
    public abstract class Person
    {
        public Person(int id, string? name, string? surName)
        {
            Id = id;
            Name = name;
            SurName = surName;
        }
        public Person() { }

        public int Id { get; set; }
        public string? Name { get; set; }
        public string? SurName { get; set; }
    }
}
