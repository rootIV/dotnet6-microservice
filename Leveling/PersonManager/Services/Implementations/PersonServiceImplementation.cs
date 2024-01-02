using Rest.API.Model;
using System.Collections.Generic;
using System.Threading;

namespace Rest.API.Services.Implementations
{
    public class PersonServiceImplementation : IPersonService
    {
        private volatile int count;

        public Person Create(Person person)
        {
            return person;
        }

        public void Delete(long id)
        {

        }

        public List<Person> FindAll()
        {
            List<Person> persons = new List<Person>();

            for (int i = 0; i < 8; i++)
            {
                Person person = MockPerson(i);

                persons.Add(person);
            }

            return persons;
        }

        public Person FindById(long id)
        {
            return new Person { 
                Id = IncrementAndGet(),
                FirstName = "Vitor",
                LastName = "rootIV",
                Address = "Some place in Brazil",
                Gender = "Male"
            };
        }

        public Person Update(Person person)
        {
            return person;
        }

        private Person MockPerson(int i)
        {
            return new Person
            {
                Id = IncrementAndGet(),
                FirstName = "Person Name" + i,
                LastName = "Person Last Name" + i,
                Address = "Somewhere" + i,
                Gender = "Unidentified"
            };
        }

        private long IncrementAndGet()
        {
            return Interlocked.Increment(ref count);
        }
    }
}
