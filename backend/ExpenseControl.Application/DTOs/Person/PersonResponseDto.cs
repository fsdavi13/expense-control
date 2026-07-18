namespace ExpenseControl.Application.DTOs.Person;

public sealed class PersonResponseDto
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public int Age { get; set; }
}