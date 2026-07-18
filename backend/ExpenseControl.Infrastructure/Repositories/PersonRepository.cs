using ExpenseControl.Domain.Entities;
using ExpenseControl.Domain.Repositories;
using ExpenseControl.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ExpenseControl.Infrastructure.Repositories;

public sealed class PersonRepository : IPersonRepository
{
    private readonly ExpenseControlDbContext _context;

    public PersonRepository(ExpenseControlDbContext context)
    {
        _context = context;
    }

    public async Task<Person> AddAsync(Person person)
    {
        await _context.Persons.AddAsync(person);
        await _context.SaveChangesAsync();

        return person;
    }

    public async Task<List<Person>> GetAllAsync()
    {
        return await _context.Persons
            .AsNoTracking()
            .OrderBy(person => person.Name)
            .ToListAsync();
    }

    public async Task<Person?> GetByIdAsync(int id)
    {
        return await _context.Persons
            .FirstOrDefaultAsync(person => person.Id == id);
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Persons
            .AnyAsync(person => person.Id == id);
    }

    public async Task DeleteAsync(Person person)
    {
        _context.Persons.Remove(person);

        await _context.SaveChangesAsync();
    }
}