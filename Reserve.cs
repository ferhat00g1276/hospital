using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital
{
    public class Reserve
    {
        public Reserve(Doctor doc, User patient, int month, int day, string time)
        {
            this.doc = doc;
            this.patient = patient;
            this.month = month;
            this.day = day;
            this.time = time;
        }

        public Doctor doc { get; set; }
        public User patient { get; set; }
        public int month { get; set; }
        public int day { get; set; }
        public string time { get; set; }
    }
}
