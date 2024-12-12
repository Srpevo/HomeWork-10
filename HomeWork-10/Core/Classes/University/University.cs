
namespace HomeWork_10.Core.Classes.University
{
    internal class University
    {

        public University(int id, string name, string description)
        {
            Id = id;
            this.Name = name;
            this.Description = description;
        }

        public int Id {  get; set; } 
        public string? Name { get; set; }
        public string? Description { get; set; }
    }
}
