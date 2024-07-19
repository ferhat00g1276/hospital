using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital
{
    public class User
    {
        public User(string name, string surname, string email)
        {
            Name = name;
            Surname = surname;
            Email = email;
        }
        public User()
        {

        }

        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public List<Reserve> Reserves { get; set; }
    }
}
