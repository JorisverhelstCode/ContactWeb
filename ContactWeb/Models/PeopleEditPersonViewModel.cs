using ContactWeb.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ContactWeb.Models
{
    public class PeopleEditPersonViewModel
    {
        [DisplayName("First name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "This person must have a first name!")]
        [MinLength(2, ErrorMessage = "Please give this person more than 1 letter in their name")]
        [MaxLength(30, ErrorMessage = "It's long enough now")]
        public string FirstName { get; set; }

        [DisplayName("Last name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "This person must have a last name!")]
        [MinLength(2, ErrorMessage = "Please give this person more than 1 letter in their name")]
        [MaxLength(30, ErrorMessage = "It's long enough now")]
        public string LastName { get; set; }

        [DisplayName("Birthday")]
        [Required]
        [Range(typeof(DateTime), "01/10/1900", "01/12/2019")]
        public DateTime DateOfBirth { get; set; }

        [DisplayName("Phone number")]
        public int PhoneNumber { get; set; }

        [DisplayName("Email adress")]
        public string Email { get; set; }

        [DisplayName("Adress")]
        [MaxLength(45, ErrorMessage = "Jeesus, are they living on Mars or something?")]
        public string Adress { get; set; }

        [DisplayName("Description")]
        [MaxLength(300, ErrorMessage = "We ran out of server space to store more info!")]
        public string Description { get; set; }

        [DisplayName("Categorie")]
        public IEnumerable<SelectListItem> Categories { get; set; } = new List<SelectListItem>()
        {
            new SelectListItem(){Text = "Family", Value = "Family"},
            new SelectListItem(){Text = "Colleague", Value = "Colleague"},
            new SelectListItem(){Text = "Friend", Value = "Friend"},
            new SelectListItem(){Text = "Enemy", Value = "Enemy"},
        };

        [DisplayName("Foto")]
        public IFormFile Avatar { get; set; }
        public string AvatarUrl { get; set; }
        public int ID { get; set; }
        public string ReturnUrl { get; set; }
    }
}
