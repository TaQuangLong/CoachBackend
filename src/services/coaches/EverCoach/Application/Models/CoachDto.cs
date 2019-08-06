using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EverCoach.Models
{
    public class CoachDto
    {
        
        public string Name { get; set; }
        //public Boolean Sex { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }
        public string PhoneNum { get; set; }
    }
}
