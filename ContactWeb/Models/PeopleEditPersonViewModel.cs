using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ContactWeb.Models
{
    public class PeopleEditPersonViewModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "This person must have a first name!")]
        [MinLength(2, ErrorMessage = "Please give this person more than 1 letter in their name")]
        [MaxLength(30, ErrorMessage = "It's long enough now")]
        public string FirstName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "This person must have a last name!")]
        [MinLength(2, ErrorMessage = "Please give this person more than 1 letter in their name")]
        [MaxLength(30, ErrorMessage = "It's long enough now")]
        public string LastName { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        public int PhoneNumber { get; set; }

        public string Email { get; set; }

        [MaxLength(45, ErrorMessage = "Jeesus, are they living on Mars or something?")]
        public string Adress { get; set; }

        [MaxLength(300, ErrorMessage = "We ran out of server space to store more info!")]
        public string Description { get; set; }

        public int ID { get; set; }

        public string ReturnUrl { get; set; }
    }
}
