using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EverCoach.Api.Application.Commands
{
    public class CreateCoachCommand
    {
        public string Name { get;  set; }
        public string Email { get;  set; }
        public int Age { get;  set; }
        public string PhoneNum { get;  set; }
        //public DateTime Dob { get; private set; }
    }
}
