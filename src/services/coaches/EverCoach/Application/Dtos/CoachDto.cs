using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EverCoach.Dtos
{
    public class CoachDto
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [Required]
        public string Name { get; set; }
        public Boolean Sex { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [Range(1, 150)]
        public int Age { get; set; }
        [Required]
        public string Address { get; set; }
        [Phone]
        public string PhoneNum { get; set; }
        [DataType(DataType.Date)]
        public DateTime Dob { get; set; }
    }
 
}
