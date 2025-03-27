using Microsoft.EntityFrameworkCore;
using PhoneDirectory.Infrastructure;
using PhoneDirectory.Infrastructure.Repositories;
using PhoneDirectory.Domain.Entities;
using PhoneDirectory.Infrastructure.Entities;

namespace PhoneDirectory.Tests.Repositories;

public class PersonRepositoryTests
{
    private PhoneDirectoryDbContext _context;
    private PersonRepository _repository;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<PhoneDirectoryDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new PhoneDirectoryDbContext(options);
        _repository = new PersonRepository(_context);
    }

    [Test]
    public async Task GetByIdAsync_ExistingPerson_ReturnsPerson()
    {
        // Arrange
        var person = new PersonEntity
        {
            Id = Guid.NewGuid(),
            FirstName = "John",
            LastName = "Doe",
            Company = "Test Corp"
        };
        _context.Persons.Add(person);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByIdAsync(person.Id);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.FirstName, Is.EqualTo("John"));
        Assert.That(result.LastName, Is.EqualTo("Doe"));
    }

    [Test]
    public void GetByIdAsync_NonExistingPerson_ThrowsKeyNotFoundException()
    {
        // Arrange
        var nonExistingId = Guid.NewGuid();

        // Act & Assert
        Assert.ThrowsAsync<KeyNotFoundException>(async () => 
            await _repository.GetByIdAsync(nonExistingId));
    }

    [Test]
    public void Add_ValidPerson_ReturnsSavedPerson()
    {
        // Arrange
        var person = new Person(
            firstName: "Jane",
            lastName: "Smith",
            company: "Test Inc"
        );

        // Act
        var result = _repository.Add(person);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.FirstName, Is.EqualTo("Jane"));
        Assert.That(_context.Persons.Count(), Is.EqualTo(1));
    }

    [Test]
    public void Add_NullPerson_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => _repository.Add(null!));
    }

    [Test]
    public void GetAll_ReturnsAllPersons()
    {
        // Arrange
        var persons = new List<PersonEntity>
        {
            new() { 
                Id = Guid.NewGuid(), 
                FirstName = "John", 
                LastName = "Doe", 
                Company = "Company A" 
            },
            new() { 
                Id = Guid.NewGuid(), 
                FirstName = "Jane", 
                LastName = "Smith", 
                Company = "Company B" 
            }
        };
        _context.Persons.AddRange(persons);
        _context.SaveChanges();

        // Act
        var result = _repository.GetAll().ToList();

        // Assert
        Assert.That(result, Has.Count.EqualTo(2));
    }

    [Test]
    public void Update_ValidPerson_ReturnsUpdatedPerson()
    {
        // Arrange
        var person = new Person("John", "Doe", "Test Corp");
        var savedPerson = _repository.Add(person);

        // Modify the person
        var updatedPerson = new Person("John", "Updated", "New Corp");
        // Set the Id using reflection since it's read-only
        typeof(Person).GetProperty("Id")?.SetValue(updatedPerson, savedPerson.Id);

        // Clear the context to avoid tracking conflicts
        _context.ChangeTracker.Clear();

        // Act
        var result = _repository.Update(updatedPerson);

        // Assert
        Assert.That(result.LastName, Is.EqualTo("Updated"));
        Assert.That(result.Company, Is.EqualTo("New Corp"));
    }

    [Test]
    public void Delete_ExistingPerson_RemovesPerson()
    {
        // Arrange
        var person = new PersonEntity
        {
            Id = Guid.NewGuid(),
            FirstName = "John",
            LastName = "Doe",
            Company = "Test Corp"
        };
        _context.Persons.Add(person);
        _context.SaveChanges();

        // Act
        _repository.Delete(person.Id);

        // Assert
        Assert.That(_context.Persons.Find(person.Id), Is.Null);
    }

    [TearDown]
    public void Cleanup()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
} 