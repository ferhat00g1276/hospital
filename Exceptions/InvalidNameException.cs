using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Exceptions
{
    public class InvalidNameException:Exception
    {
        public string Message { get; set; }
        public InvalidNameException(string message)
        {
            Message = message;
        }
    }
}
