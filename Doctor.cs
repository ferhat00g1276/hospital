using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital
{
    public class Doctor
    {
        public Doctor(string name, string surname, int experience, Departments dep)
        {
            Name = name;
            Surname = surname;
            Experience = experience;
            Department = dep;
            Id = ++id;
        }
        public Doctor() // deserializasiyada problem yaranmisdi, arasdırdım baxdım ki parametrsiz belə bir constructor lazim imiş 
        {
            BusyHours = new List<Reserve>();
        }
        public int Id { get; set; }
        static int id = 0;
        public string Name { get; set; }
        public string Surname { get; set; }
        public int Experience { get; set; }
        public Departments Department { get; set; }

        public enum Departments
        {
            Pediatrichs = 0,
            Traumatology = 1,
            Stomatology = 2
        }

        public List<Reserve> BusyHours { get; set; }

        public void DisplayInfo()
        {
            Console.WriteLine($"Name: {Name} {Surname}");
            Console.WriteLine($"Experience: {Experience} years");
            Console.WriteLine($"Department: {Department}");
            Console.WriteLine($"ID: {Id}");
        }
    }
}
