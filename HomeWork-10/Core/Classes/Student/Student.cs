using HomeWork_10.Core.Base.Person;
using HomeWork_10.Core.Interfaces.Istudent;

namespace HomeWork_10.Core.Classes.Student
{
    public class Student : Person, Istudent
    {
        public Student(int id, string? name, string? surName, string? email) : base(id, name, surName)
        {
            this.email = email;
        }
        public Student() { }

        public string? email { get; set; }
   
    }
}
