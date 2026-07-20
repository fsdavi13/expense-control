using ExpenseControl.Application.DTOs.Person;
using ExpenseControl.Application.Exceptions;
using ExpenseControl.Application.Interfaces;
using ExpenseControl.Domain.Entities;
using ExpenseControl.Domain.Enums;
using ExpenseControl.Domain.Repositories;

namespace ExpenseControl.Application.Services.Implementations;

public sealed class PersonService : IPersonService
{
    private readonly IPersonRepository _personRepository;
    private readonly ITransactionRepository _transactionRepository;

    public PersonService(
        IPersonRepository personRepository,
        ITransactionRepository transactionRepository)
    {
        _personRepository = personRepository;
        _transactionRepository = transactionRepository;
    }

    public async Task<PersonResponseDto> CreateAsync(
        CreatePersonDto dto)
    {
        ValidatePerson(dto.Name, dto.Age);

        var person = new Person(
            dto.Name.Trim(),
            dto.Age);

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

    public async Task<PersonResponseDto> UpdateAsync(
        int id,
        UpdatePersonDto dto)
    {
        ValidatePerson(dto.Name, dto.Age);

        var person = await _personRepository.GetByIdAsync(id);

        if (person is null)
        {
            throw new NotFoundException(
                "Pessoa não encontrada.");
        }

        if (dto.Age < 18)
        {
            // Antes de transformar uma pessoa adulta em menor de idade,
            // verificamos se existem receitas incompatíveis com essa regra.
            var transactions =
                await _transactionRepository.GetByPersonIdAsync(id);

            var hasIncome = transactions.Any(
                transaction =>
                    transaction.Type == TransactionType.Income);

            // Bloquear a alteração mantém os dados existentes coerentes:
            // menores de idade podem possuir somente despesas.
            if (hasIncome)
            {
                throw new BusinessException(
                    "Não é possível alterar a pessoa para menor de idade porque ela possui receitas cadastradas.");
            }
        }

        person.Update(
            dto.Name.Trim(),
            dto.Age);

        await _personRepository.UpdateAsync(person);

        return MapToResponse(person);
    }

    public async Task DeleteAsync(int id)
    {
        var person = await _personRepository.GetByIdAsync(id);

        if (person is null)
        {
            throw new NotFoundException(
                "Pessoa não encontrada.");
        }

        await _personRepository.DeleteAsync(person);
    }

    private static void ValidatePerson(
        string name,
        int age)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new BusinessException(
                "O nome da pessoa é obrigatório.");
        }

        if (age < 0 || age > 120)
        {
            throw new BusinessException(
                "Informe uma idade válida.");
        }
    }

    private static PersonResponseDto MapToResponse(
        Person person)
    {
        return new PersonResponseDto
        {
            Id = person.Id,
            Name = person.Name,
            Age = person.Age
        };
    }
}