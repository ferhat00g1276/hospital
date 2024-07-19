using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Exceptions
{
    public class InvalidEmailException:Exception
    {
        public string Message { get; set; }
        public InvalidEmailException(string message)
        {
            Message = message;
        }
    }
}
