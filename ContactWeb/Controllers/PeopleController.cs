using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ContactWeb.Database;
using ContactWeb.Domain;
using ContactWeb.Models;
using ContactWeb.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ContactWeb.Controllers
{
    public class PeopleController : Controller
    {
        private readonly IPeopleDatabase _personsDatabase;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly PhotoService _photoService;

        public PeopleController(IPeopleDatabase db, IWebHostEnvironment hostingEnvironment)
        {
            _personsDatabase = db;
            _hostingEnvironment = hostingEnvironment;
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
                string uniqueFileName = null;
                if (createdPerson.Avatar != null)
                {
                    string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "Images");
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + createdPerson.Avatar.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        createdPerson.Avatar.CopyTo(stream);
                    }
                }
                Person person = new Person
                {
                    FirstName = createdPerson.FirstName,
                    LastName = createdPerson.LastName,
                    DateOfBirth = createdPerson.DateOfBirth,
                    Adress = createdPerson.Adress,
                    Description = createdPerson.Description,
                    PhoneNumber = createdPerson.PhoneNumber,
                    Email = createdPerson.Email,
                    AvatarPath = uniqueFileName
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
                ReturnUrl = returnUrl,
                AvatarUrl = toBeEdittedPerson.AvatarPath
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

                Person personFromDB = _personsDatabase.GetPerson(toBeEdittedPerson.ID);

                if (toBeEdittedPerson.Avatar == null)
                {
                    person.AvatarPath = personFromDB.AvatarPath;
                }
                else
                {
                    if (!string.IsNullOrEmpty(personFromDB.AvatarPath))
                    {
                        _photoService.DeletePicture(_hostingEnvironment.WebRootPath, personFromDB.AvatarPath);
                    }

                    string uniqueFileName = UploadContactPhoto(toBeEdittedPerson.Avatar);
                    person.AvatarPath = "/photos/" + uniqueFileName;
                }

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

        private string UploadContactPhoto(IFormFile photo)
        {
            string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(photo.FileName);
            string pathName = Path.Combine(_hostingEnvironment.WebRootPath, "photos");
            string fileNameWithPath = Path.Combine(pathName, uniqueFileName);

            using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
            {
                photo.CopyTo(stream);
            }

            return uniqueFileName;
        }
    }
}