using System.ComponentModel.DataAnnotations;

namespace ExpenseControl.Application.DTOs.Person;

public sealed class CreatePersonDto
{
    [Required(ErrorMessage = "O nome é obrigatório.")]
    [MaxLength(100, ErrorMessage = "O nome deve possuir no máximo 100 caracteres.")]
    public string Name { get; set; } = string.Empty;

    [Range(0, 120, ErrorMessage = "Informe uma idade válida.")]
    public int Age { get; set; }
}