using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EverCoach.Api.Application.Commands
{
    public class CreateCoachCommand
    {
        public string Name { get; private set; }
        public string Email { get; private set; }
        public int Age { get; private set; }
        public string PhoneNum { get; private set; }
        //public DateTime Dob { get; private set; }
    }
}
