using ExpenseControl.Application.DTOs.Person;

namespace ExpenseControl.Application.Interfaces;

public interface IPersonService
{
    Task<PersonResponseDto> CreateAsync(CreatePersonDto dto);

    Task<List<PersonResponseDto>> GetAllAsync();

    Task DeleteAsync(int id);
}