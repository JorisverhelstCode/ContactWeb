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
            PeopleDeletePersonViewModel toBeDeletedPersonView = new PeopleDeletePersonViewModel
            {
                ID = toBeDeletedPerson.ID,
                FirstName = toBeDeletedPerson.FirstName,
                LastName = toBeDeletedPerson.LastName,
                ReturnUrl = returnUrl
            };
            return View(toBeDeletedPersonView);
        }

        [HttpPost]
        public IActionResult DeletePerson(PeopleDeletePersonViewModel toBeDeletedPerson)
        {
            _personsDatabase.Delete(toBeDeletedPerson.ID);
            return RedirectToAction("Index");
        }

        public IActionResult EditPerson(int id, string returnUrl)
        {
            Person toBeEdittedPerson = _personsDatabase.GetPerson(id);
            PeopleEditPersonViewModel toBeEdittedPersonView = new PeopleEditPersonViewModel
            {
                FirstName = toBeEdittedPerson.FirstName,
                LastName = toBeEdittedPerson.LastName,
                Adress = toBeEdittedPerson.Adress,
                PhoneNumber = toBeEdittedPerson.PhoneNumber,
                DateOfBirth = toBeEdittedPerson.DateOfBirth,
                Description = toBeEdittedPerson.Description,
                Email = toBeEdittedPerson.Email,
                ID = toBeEdittedPerson.ID,
                ReturnUrl = returnUrl
            };
            return View(toBeEdittedPersonView);
        }

        [HttpPost]
        public IActionResult EditPerson(PeopleEditPersonViewModel toBeEdittedPerson)
        {
            if (!TryValidateModel(toBeEdittedPerson))
            {
                return View(toBeEdittedPerson);
            }
            else
            {
                Person person = new Person
                {
                    FirstName = toBeEdittedPerson.FirstName,
                    LastName = toBeEdittedPerson.LastName,
                    DateOfBirth = toBeEdittedPerson.DateOfBirth,
                    Adress = toBeEdittedPerson.Adress,
                    Description = toBeEdittedPerson.Description,
                    PhoneNumber = toBeEdittedPerson.PhoneNumber,
                    Email = toBeEdittedPerson.Email
                };

                _personsDatabase.Update(toBeEdittedPerson.ID, person);
                return RedirectToAction(toBeEdittedPerson.ReturnUrl, new { id = toBeEdittedPerson.ID });
            }
        }

        public IActionResult PersonDetails(int id)
        {
            Person person = _personsDatabase.GetPerson(id);
            PeoplePersonDetailsViewModel ppdvm = new PeoplePersonDetailsViewModel
            {
                FirstName = person.FirstName,
                LastName = person.LastName,
                DateOfBirth = person.DateOfBirth,
                Adress = person.Adress,
                Description = person.Description,
                PhoneNumber = person.PhoneNumber,
                Email = person.Email
            };
            return View(ppdvm);
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