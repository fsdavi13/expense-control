using ExpenseControl.Application.DTOs.Person;

namespace ExpenseControl.Application.Interfaces;

public interface IPersonService
{
    Task<PersonResponseDto> CreateAsync(CreatePersonDto dto);

    Task<List<PersonResponseDto>> GetAllAsync();

    Task<PersonResponseDto> UpdateAsync(
        int id,
        UpdatePersonDto dto);

    Task DeleteAsync(int id);
}