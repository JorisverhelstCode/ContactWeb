using ContactWeb.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactWeb.Domain
{
    public class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Adress { get; set; }
        public string Description { get; set; }
        public int ID { get; set; }
        public string AvatarPath { get; set; }
        public string Category { get; set; }
    }
}
