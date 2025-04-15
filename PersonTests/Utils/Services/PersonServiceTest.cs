using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using person_wpf_demo.Data.Repositories.Interfaces;
using person_wpf_demo.Model;
using person_wpf_demo.Utils.Services;
using person_wpf_demo.Utils.Services.Interfaces;

namespace PersonTests.Utils.Services
{
    public class PersonServiceTest
    {
        private readonly IPersonService _personService;
        private readonly Mock<IPersonRepository> _personRepositoryMock;

        public PersonServiceTest()
        {
            _personRepositoryMock = new Mock<IPersonRepository>();
            _personService = new PersonService(_personRepositoryMock.Object);
        }

        [Fact]
        public void Add_ShouldCallSavePersonMethod()
        {
            Person person = new Person
            {
                Prenom = "John",
                Nom = "Doe"
            };

            _personService.Add(person);

            _personRepositoryMock.Verify(repository => repository.Save(person), Times.Once);
        }

        [Fact]
        public void FindAll_ShouldReturnAllPersonsFromRepository()
        {
            var fakePersons = new List<Person>
            {
                new Person { Prenom = "John", Nom = "Doe" },
                new Person { Prenom = "Jane", Nom = "Doe" }
            };

            _personRepositoryMock.Setup(repository => repository.GetAll()).Returns(fakePersons);

            var result = _personService.FindAll();

            Assert.Equal(2, fakePersons.Count);
        }

        [Fact]
        public void Remove_ShouldCallDeleteMethod()
        {
            //N'importe quelle personne
            Person person = It.IsAny<Person>();

            _personService.Remove(person);

            _personRepositoryMock.Verify(repository => repository.Delete(person), Times.Once);
        }

    }
}
