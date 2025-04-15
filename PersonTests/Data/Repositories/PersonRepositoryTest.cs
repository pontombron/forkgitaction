using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using person_wpf_demo.Data.Repositories;
using person_wpf_demo.Data.Repositories.Interfaces;
using person_wpf_demo.Model;

namespace PersonTests.Data.Repositories
{
    public class PersonRepositoryTest : IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly IPersonRepository _repository;

        public PersonRepositoryTest()
        {
            // Configuration d'une base de données en mémoire pour les tests
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _context = new ApplicationDbContext(options);

            // Initialisation du repository avec le contexte en mémoire
            _repository = new PersonRepository(_context);
        }

        // Méthode de teardown pour nettoyer la base en mémoire après chaque test
        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Fact]
        public void Save_ShouldAddPerson()
        {
            // Arrange
            var person = new Person
            {
                Prenom = "John",
                Nom = "Doe"
            };

            // Act
            _repository.Save(person);

            // Assert
            var persons = _context.Persons.ToList();
            Assert.Single(persons);
            Assert.Equal("John", persons[0].Prenom);
            Assert.Equal("Doe", persons[0].Nom);
        }

        [Fact]
        public void GetAll_ShouldReturnAllPersonsWithAddresses()
        {
            // Arrange
            var person = new Person
            {
                Prenom = "Alice",
                Nom = "Smith"
            };
            var address = new Address
            {
                Street = "123 Main St",
                City = "Cityville",
                PostalCode = "12345",
            };
            person.Addresses.Add(address);
            _context.Persons.Add(person);
            _context.SaveChanges();

            // Act
            var persons = _repository.GetAll();

            // Assert
            Assert.Single(persons);
            var retrievedPerson = persons.First();
            Assert.Equal("Alice", retrievedPerson.Prenom);
            Assert.NotEmpty(retrievedPerson.Addresses);
            Assert.Equal("123 Main St", retrievedPerson.Addresses.First().Street);
        }

        [Fact]
        public void Update_ShouldModifyExistingPersonAndAddresses()
        {
            // Arrange
            var person = new Person
            {
                Prenom = "Bob",
                Nom = "Marley"
            };
            var address1 = new Address
            {
                Street = "Old Street",
                City = "Old City",
                PostalCode = "00000"
            };
            person.Addresses.Add(address1);
            _context.Persons.Add(person);
            _context.SaveChanges();

            // Préparer une mise à jour : modification du prénom, mise à jour d'une adresse existante et ajout d'une nouvelle adresse
            var updatedPerson = new Person
            {
                Id = person.Id,
                Prenom = "Bobby",
                Nom = "Marley"
            };

            // Mise à jour de l'adresse existante
            // On utilise ici le même objet address1 qui possède déjà son Id attribué
            address1.Street = "New Street";
            updatedPerson.Addresses.Add(address1);

            // Ajout d'une nouvelle adresse
            var address2 = new Address
            {
                Street = "Another Street",
                City = "Another City",
                PostalCode = "11111"
            };
            updatedPerson.Addresses.Add(address2);

            // Act
            _repository.Update(updatedPerson);

            // Assert
            var personFromDb = _context.Persons.Include(p => p.Addresses).FirstOrDefault(p => p.Id == person.Id);
            Assert.NotNull(personFromDb);
            Assert.Equal("Bobby", personFromDb.Prenom);
            // Vérifier la mise à jour de l'adresse existante
            var updatedAddress1 = personFromDb.Addresses.FirstOrDefault(a => a.Id == address1.Id);
            Assert.NotNull(updatedAddress1);
            Assert.Equal("New Street", updatedAddress1.Street);
            // Vérifier que la nouvelle adresse a été ajoutée
            var newAddress = personFromDb.Addresses.FirstOrDefault(a => a.Street == "Another Street");
            Assert.NotNull(newAddress);
        }

        [Fact]
        public void Delete_ShouldRemovePerson()
        {
            // Arrange
            var person = new Person
            {
                Prenom = "Delete",
                Nom = "Me"
            };
            _context.Persons.Add(person);
            _context.SaveChanges();

            // Act
            _repository.Delete(person);

            // Assert
            var personFromDb = _context.Persons.FirstOrDefault(p => p.Id == person.Id);
            Assert.Null(personFromDb);
        }
    }
}
