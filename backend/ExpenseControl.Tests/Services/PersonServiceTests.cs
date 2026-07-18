using ExpenseControl.Application.DTOs.Person;
using ExpenseControl.Application.Exceptions;
using ExpenseControl.Application.Services.Implementations;
using ExpenseControl.Domain.Entities;
using ExpenseControl.Domain.Repositories;
using ExpenseControl.Tests.Support;
using Moq;

namespace ExpenseControl.Tests.Services;

public sealed class PersonServiceTests
{
    private readonly Mock<IPersonRepository> _personRepositoryMock;
    private readonly PersonService _personService;

    public PersonServiceTests()
    {
        _personRepositoryMock = new Mock<IPersonRepository>();

        _personService = new PersonService(
            _personRepositoryMock.Object);
    }

    [Fact]
    public async Task CreateAsync_ShouldCreatePerson_WhenDataIsValid()
    {
        Person? createdPerson = null;

        _personRepositoryMock
            .Setup(repository =>
                repository.AddAsync(It.IsAny<Person>()))
            .Returns((Person person) =>
            {
                createdPerson = person;

                return Task.FromResult(person);
            });

        var dto = new CreatePersonDto
        {
            Name = "  Ana Silva  ",
            Age = 25
        };

        var result = await _personService.CreateAsync(dto);

        Assert.NotNull(createdPerson);
        Assert.Equal("Ana Silva", createdPerson!.Name);
        Assert.Equal(25, createdPerson.Age);

        Assert.Equal("Ana Silva", result.Name);
        Assert.Equal(25, result.Age);

        _personRepositoryMock.Verify(
            repository =>
                repository.AddAsync(It.IsAny<Person>()),
            Times.Once);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public async Task CreateAsync_ShouldThrowBusinessException_WhenNameIsInvalid(
        string invalidName)
    {
        var dto = new CreatePersonDto
        {
            Name = invalidName,
            Age = 25
        };

        var exception = await Assert.ThrowsAsync<BusinessException>(
            () => _personService.CreateAsync(dto));

        Assert.Equal(
            "O nome da pessoa é obrigatório.",
            exception.Message);

        _personRepositoryMock.Verify(
            repository =>
                repository.AddAsync(It.IsAny<Person>()),
            Times.Never);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(121)]
    public async Task CreateAsync_ShouldThrowBusinessException_WhenAgeIsInvalid(
        int invalidAge)
    {
        var dto = new CreatePersonDto
        {
            Name = "Ana Silva",
            Age = invalidAge
        };

        var exception = await Assert.ThrowsAsync<BusinessException>(
            () => _personService.CreateAsync(dto));

        Assert.Equal(
            "Informe uma idade válida.",
            exception.Message);

        _personRepositoryMock.Verify(
            repository =>
                repository.AddAsync(It.IsAny<Person>()),
            Times.Never);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnMappedPeople()
    {
        var people = new List<Person>
        {
            TestEntityFactory.CreatePerson(
                id: 1,
                name: "Ana Silva",
                age: 25),

            TestEntityFactory.CreatePerson(
                id: 2,
                name: "Carlos Souza",
                age: 32)
        };

        _personRepositoryMock
            .Setup(repository => repository.GetAllAsync())
            .ReturnsAsync(people);

        var result = await _personService.GetAllAsync();

        Assert.Equal(2, result.Count);

        Assert.Equal(1, result[0].Id);
        Assert.Equal("Ana Silva", result[0].Name);
        Assert.Equal(25, result[0].Age);

        Assert.Equal(2, result[1].Id);
        Assert.Equal("Carlos Souza", result[1].Name);
        Assert.Equal(32, result[1].Age);
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeletePerson_WhenPersonExists()
    {
        var person = TestEntityFactory.CreatePerson(
            id: 1,
            name: "Ana Silva",
            age: 25);

        _personRepositoryMock
            .Setup(repository => repository.GetByIdAsync(1))
            .ReturnsAsync(person);

        _personRepositoryMock
            .Setup(repository => repository.DeleteAsync(person))
            .Returns(Task.CompletedTask);

        await _personService.DeleteAsync(1);

        _personRepositoryMock.Verify(
            repository => repository.DeleteAsync(person),
            Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ShouldThrowNotFoundException_WhenPersonDoesNotExist()
    {
        _personRepositoryMock
            .Setup(repository => repository.GetByIdAsync(999))
            .ReturnsAsync((Person?)null);

        var exception = await Assert.ThrowsAsync<NotFoundException>(
            () => _personService.DeleteAsync(999));

        Assert.Equal(
            "Pessoa não encontrada.",
            exception.Message);

        _personRepositoryMock.Verify(
            repository =>
                repository.DeleteAsync(It.IsAny<Person>()),
            Times.Never);
    }
}