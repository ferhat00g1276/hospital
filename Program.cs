using Hospital;
using Hospital.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

class Program
{
    private static bool IsAllLetters(string input)
    {
        foreach (char c in input)
        {
            if (!char.IsLetter(c))
            {
                return false;
            }
        }
        return true;
    }

    public static bool LogIn(out User user)
    {
        user = new User();
        try
        {
            Console.WriteLine("Enter your name");
            user.Name = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(user.Name))
            {
                throw new InvalidNameException("Name cannot be empty .");
            }
            if (!IsAllLetters(user.Name))
            {
                throw new InvalidNameException("Name can only contain letters.");
            }

            Console.WriteLine("Enter your surname");
            user.Surname = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(user.Surname))
            {
                throw new InvalidSurnameException("Surname cannot be empty .");
            }
            if (!IsAllLetters(user.Surname))
            {
                throw new InvalidSurnameException("Surname can only contain letters.");
            }

            Console.WriteLine("Enter your email");
            user.Email = Console.ReadLine();
            if (!user.Email.EndsWith("@gmail.com") && !user.Email.EndsWith("@mail.ru"))
            {
                throw new InvalidEmailException("Email has to end with @gmail.com or @mail.ru");
            }
            if (string.IsNullOrWhiteSpace(user.Email))
            {
                throw new InvalidEmailException("Email cannot be empty .");
            }

            return true;
        }
        catch (InvalidNameException ex)
        {
            Console.WriteLine("Invalid name: " + ex.Message);
            return false;
        }
        catch (InvalidSurnameException ex)
        {
            Console.WriteLine("Invalid surname: " + ex.Message);
            return false;
        }
        catch (InvalidEmailException ex)
        {
            Console.WriteLine("Invalid email: " + ex.Message);
            return false;
        }
    }

    public static void DepartmentsMenu(HospitalClass hospital)
    {
        Console.WriteLine("Choose a department:");
        for (int i = 0; i < hospital.AllDepartments.Count(); i++)
        {
            Console.WriteLine($"{i + 1}. {hospital.AllDepartments[i]}");
        }
    }

    public static List<Doctor> DisplayDoctorsInDepartment(HospitalClass hospital, int departmentIndex)
    {
        List<Doctor> doctorsInDepartment = new List<Doctor>();

        if (departmentIndex < 1 || departmentIndex > hospital.AllDepartments.Count)
        {
            Console.WriteLine("Invalid department selection.");
            return doctorsInDepartment;
        }

        string selectedDepartment = hospital.AllDepartments[departmentIndex - 1];
        Console.WriteLine($"Doctors in {selectedDepartment} department:");

        foreach (var doctor in hospital.AllDoctors)
        {
            if (doctor.Department.ToString() == selectedDepartment)
            {
                Console.WriteLine($"{doctor.Id}. {doctor.Name} {doctor.Surname}");
                doctorsInDepartment.Add(doctor);
            }
        }

        return doctorsInDepartment;
    }

    public static void SelectDoctor(List<Doctor> doctors, User user)
    {
        Console.WriteLine("Enter the ID of the doctor to view details, or 0 to go back:");
        int doctorId;
        if (int.TryParse(Console.ReadLine(), out doctorId))
        {
            if (doctorId == 0)
            {
                return; // geri menyuya qayidilmasi
            }

            Doctor selectedDoctor = doctors.Find(d => d.Id == doctorId);
            if (selectedDoctor != null)
            {
                selectedDoctor.DisplayInfo();
                ScheduleAppointment(selectedDoctor, user);
            }
            else
            {
                Console.WriteLine("Doctor not found. Please enter a valid ID.");
            }
        }
        else
        {
            Console.WriteLine("Invalid input. Please enter a valid number.");
        }
    }

    public static void ScheduleAppointment(Doctor doctor, User user)
    {
        try
        {
            Console.WriteLine("Enter the month for the appointment (1-12):");
            int month = int.Parse(Console.ReadLine());

            // ayin duzgunluyunun yoxlanilmasi
            if (month < 1 || month > 12)
            {
                throw new Exception("Invalid month. Must be between 1 and 12.");
            }

            Console.WriteLine("Enter the day for the appointment (1-31):");
            int day = int.Parse(Console.ReadLine());

            // gunun aya gore uygunlugunun yoxlanilmasi
            if (!IsValidDay(month, day))
            {
                throw new Exception("Invalid day for the given month.");
            }

            Console.WriteLine("Enter the time for the appointment (morning, afternoon, evening):");
            string time = Console.ReadLine().ToLower();

            // vaxtin duzgunluyunun yoxlanilmasi
            if (!IsValidTime(time))
            {
                throw new Exception("Invalid time. Choose 'morning', 'afternoon', or 'evening'.");
            }

            Reserve newReserve = new Reserve(doctor, user, month, day, time);

            if (IsReservationAvailable(newReserve))
            {
                SaveReservation(newReserve);
                Console.WriteLine("Appointment scheduled successfully.");
            }
            else
            {
                Console.WriteLine("The selected date and time are not available. Please choose a different date or time.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error scheduling appointment: " + ex.Message);
        }
    }
    private static bool IsValidDay(int month, int day)
    {
        // her aya gore gun sayinin verilmesi
        int[] daysInMonth = { 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };

        // gunun duzgunluyunun yoxlanilmasi
        if (day < 1 || day > daysInMonth[month - 1])
        {
            return false;
        }

        return true;
    }
    public static bool IsValidTime(string time)
    {
        return time == "morning" || time == "afternoon" || time == "evening";
    }

    public static bool IsReservationAvailable(Reserve newReserve)
    {
        List<Reserve> allReserves = LoadReservations();
        foreach (var reserve in allReserves)
        {
            if (reserve.doc.Id == newReserve.doc.Id &&
                reserve.month == newReserve.month &&
                reserve.day == newReserve.day &&
                reserve.time == newReserve.time)
            {
                return false;
            }
        }
        return true;
    }

    public static void SaveReservation(Reserve reserve)
    {
        List<Reserve> allReserves = LoadReservations();
        allReserves.Add(reserve);

        string jsonFileName = "reservations.json";
        string json = JsonSerializer.Serialize(allReserves, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(jsonFileName, json);
    }

    public static List<Reserve> LoadReservations()
    {
        string jsonFileName = "reservations.json";
        if (File.Exists(jsonFileName))
        {
            string json = File.ReadAllText(jsonFileName);
            return JsonSerializer.Deserialize<List<Reserve>>(json);
        }
        return new List<Reserve>();
    }

    static void Main(string[] args)
    {
        Doctor doc1 = new Doctor("Sehriyar", "Atababayev", 4, Doctor.Departments.Traumatology);
        Doctor doc5 = new Doctor("Marie", "Curie", 4, Doctor.Departments.Traumatology);
        Doctor doc2 = new Doctor("Perviz", "Quluzade", 4, Doctor.Departments.Stomatology);
        Doctor doc6 = new Doctor("Angelina", "Jolie", 4, Doctor.Departments.Stomatology);
        Doctor doc3 = new Doctor("Orxan", "Mustafazade", 4, Doctor.Departments.Pediatrichs);
        Doctor doc4 = new Doctor("Fidan", "Eliyeva", 4, Doctor.Departments.Pediatrichs);

        List<Doctor> docs = new List<Doctor>
        {
            doc1, doc2, doc3, doc4, doc5, doc6
        };
        HospitalClass hospital = new HospitalClass("BonaDea", docs);
        bool run = true;
        bool loginAttempt = false;
        User currentUser = null;

        while (run)
        {
            while (!loginAttempt)
            {
                if (LogIn(out currentUser))
                {
                    loginAttempt = true;
                    Console.WriteLine("Logged in successfully!");
                }
                else
                {
                    Console.WriteLine("Log in unsuccessful! Please try again.");
                }
            }

            bool inDepartmentMenu = true;

            while (inDepartmentMenu)
            {
                //Console.Clear();
                DepartmentsMenu(hospital);

                Console.WriteLine("Enter the number of the department to see doctors, or 0 to logout:");
                int departmentChoice;
                if (int.TryParse(Console.ReadLine(), out departmentChoice))
                {
                    if (departmentChoice == 0)
                    {
                        loginAttempt = false;
                        Console.WriteLine("Logging out...");
                        break; // login menyusuna qayidis
                    }

                    List<Doctor> doctorsInDepartment = DisplayDoctorsInDepartment(hospital, departmentChoice);

                    if (doctorsInDepartment.Count > 0)
                    {
                        SelectDoctor(doctorsInDepartment, currentUser);
                    }
                    else
                    {
                        Console.WriteLine("No doctors found in selected department.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a valid number.");
                }
            }

            Console.WriteLine("Do you want to exit the application? (Y/N)");
            string response = Console.ReadLine();
            if (response.ToUpper() == "Y")
            {
                run = false;
                Console.WriteLine("Exiting the application...");
            }
        }
    }
}
