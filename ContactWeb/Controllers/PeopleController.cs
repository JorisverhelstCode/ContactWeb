using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContactWeb.Database;
using ContactWeb.Domain;
using ContactWeb.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ContactWeb.Controllers
{
    public class PeopleController : Controller
    {
        private readonly IPeopleDatabase _personsDatabase;

        public PeopleController(IPeopleDatabase db)
        {
            _personsDatabase = db;
        }

        public IActionResult Index()
        {
            return View(CreateList());
        }

        public IActionResult CreateNewPerson()
        {
            PeopleCreateNewPersonViewModel pcnpvm = new PeopleCreateNewPersonViewModel();
            return View(pcnpvm);
        }

        [HttpPost]
        public IActionResult CreateNewPerson(PeopleCreateNewPersonViewModel createdPerson)
        {
            if (!TryValidateModel(createdPerson))
            {
                return View(createdPerson);
            } else
            {
                Person person = new Person
                {
                    FirstName = createdPerson.FirstName,
                    LastName = createdPerson.LastName,
                    DateOfBirth = createdPerson.DateOfBirth,
                    Adress = createdPerson.Adress,
                    Description = createdPerson.Description,
                    PhoneNumber = createdPerson.PhoneNumber,
                    Email = createdPerson.Email
                };

                _personsDatabase.Insert(person);
                return RedirectToAction("Index");
            }
        }

        public IActionResult DeletePerson(int id, string returnUrl)
        {
            Person toBeDeletedPerson = _personsDatabase.GetPerson(id);
            PersonDeleteViewModel toBeDeletedPersonView = new PersonDeleteViewModel
            {
                ID = toBeDeletedPerson.ID,
                FirstName = toBeDeletedPerson.FirstName,
                LastName = toBeDeletedPerson.LastName,
                ReturnUrl = returnUrl
            };
            return View(toBeDeletedPersonView);
        }

        [HttpPost]
        public IActionResult DeletePerson(PersonDeleteViewModel toBeDeletedPerson)
        {
            _personsDatabase.Delete(toBeDeletedPerson.ID);
            return RedirectToAction("Index");
        }

        public IActionResult EditPerson()
        {
            return View();
        }

        public IActionResult PersonDetails()
        {
            return View();
        }

        public List<PeopleIndexViewModel> CreateList()
        {
            IEnumerable<Person> peopleFromDB = _personsDatabase.GetPeople();
            List<PeopleIndexViewModel> people = new List<PeopleIndexViewModel>();
            foreach (Person person in peopleFromDB)
            {
                people.Add(new PeopleIndexViewModel() { FirstName = person.FirstName, LastName = person.LastName, ID = person.ID });
            }
            return people;
        }
    }
}