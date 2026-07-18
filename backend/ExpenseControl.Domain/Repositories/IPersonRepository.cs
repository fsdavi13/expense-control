using ExpenseControl.Domain.Entities;

namespace ExpenseControl.Domain.Repositories;

public interface IPersonRepository
{
    Task<Person> AddAsync(Person person);

    Task<List<Person>> GetAllAsync();

    Task<Person?> GetByIdAsync(int id);

    Task<bool> ExistsAsync(int id);

    Task DeleteAsync(Person person);
}