using ContactWeb.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactWeb.Database
{
    public interface IPeopleDatabase
    {
        Person Insert(Person movie);
        IEnumerable<Person> GetPeople();
        Person GetPerson(int id);
        void Delete(int id);
        void Update(int id, Person movie);
    }

    public class PersonsDatabase : IPeopleDatabase
    {
        private int _counter;
        private readonly List<Person> _people;
        public PersonsDatabase()
        {
            if (_people == null)
            {
                _people = new List<Person>();
                LoadAllPeople();
            }
        }

        public Person GetPerson(int id)
        {
            return _people.FirstOrDefault(x => x.ID == id);
        }

        public IEnumerable<Person> GetPeople()
        {
            return _people;
        }

        public Person Insert(Person person)
        {
            _counter++;
            person.ID = _counter;
            _people.Add(person);
            return person;
        }

        public void Delete(int id)
        {
            var movie = _people.SingleOrDefault(x => x.ID == id);
            if (movie != null)
            {
                _people.Remove(movie);
            }
        }

        public void Update(int id, Person updatedPerson)
        {
            var person = _people.SingleOrDefault(x => x.ID == id);
            if (person != null)
            {
                person.FirstName = updatedPerson.FirstName;
                person.Description = updatedPerson.Description;
                person.LastName = updatedPerson.LastName;
                person.PhoneNumber = updatedPerson.PhoneNumber;
                person.Adress = updatedPerson.Adress;
                person.DateOfBirth = updatedPerson.DateOfBirth;
                person.Email = updatedPerson.Email;
            }
        }

        public void LoadAllPeople()
        {

        }
    }
}
