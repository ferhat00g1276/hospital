using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital
{
    public class HospitalClass
    {
        public HospitalClass(string name, List<Doctor> allDoctors)
        {
            Name = name;
            AllDoctors = allDoctors;
            InitializeDepartments();
        }

        public string Name { get; set; }
        public List<Doctor> AllDoctors { get; set; }
        public List<string> AllDepartments { get; set; }

        private void InitializeDepartments()
        {
            AllDepartments = AllDoctors
                .Select(d => d.Department.ToString())
                .Distinct()
                .ToList();
        }

        public List<Doctor> GetDoctorsByDepartment(Doctor.Departments department)
        {
            return AllDoctors.Where(d => d.Department == department).ToList();
        }
    }
}
