using ExpenseControl.Application.DTOs.Person;
using ExpenseControl.Application.Exceptions;
using ExpenseControl.Application.Interfaces;
using ExpenseControl.Domain.Entities;
using ExpenseControl.Domain.Repositories;

namespace ExpenseControl.Application.Services.Implementations;

public sealed class PersonService : IPersonService
{
    private readonly IPersonRepository _personRepository;

    public PersonService(IPersonRepository personRepository)
    {
        _personRepository = personRepository;
    }

    public async Task<PersonResponseDto> CreateAsync(CreatePersonDto dto)
    {
        var person = new Person(
            dto.Name.Trim(),
            dto.Age
        );

        await _personRepository.AddAsync(person);

        return MapToResponse(person);
    }

    public async Task<List<PersonResponseDto>> GetAllAsync()
    {
        var persons = await _personRepository.GetAllAsync();

        return persons
            .Select(MapToResponse)
            .ToList();
    }

    public async Task DeleteAsync(int id)
    {
        var person = await _personRepository.GetByIdAsync(id);

        // Uma pessoa só pode ser removida caso exista no sistema.
        if (person is null)
        {
            throw new BusinessException("Pessoa não encontrada.");
        }

        await _personRepository.DeleteAsync(person);
    }

    private static PersonResponseDto MapToResponse(Person person)
    {
        return new PersonResponseDto
        {
            Id = person.Id,
            Name = person.Name,
            Age = person.Age
        };
    }
}