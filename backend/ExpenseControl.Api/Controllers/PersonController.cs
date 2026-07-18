using ExpenseControl.Application.DTOs.Person;
using ExpenseControl.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseControl.Api.Controllers;

[ApiController]
[Route("api/persons")]
public sealed class PersonController : ControllerBase
{
    private readonly IPersonService _personService;

    public PersonController(IPersonService personService)
    {
        _personService = personService;
    }

    /// <summary>
    /// Cria uma nova pessoa.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(
        typeof(PersonResponseDto),
        StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(
        [FromBody] CreatePersonDto dto)
    {
        var person = await _personService.CreateAsync(dto);

        return StatusCode(
            StatusCodes.Status201Created,
            person);
    }

    /// <summary>
    /// Retorna todas as pessoas cadastradas.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(
        typeof(List<PersonResponseDto>),
        StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var persons = await _personService.GetAllAsync();

        return Ok(persons);
    }

    /// <summary>
    /// Remove uma pessoa e suas transações relacionadas.
    /// </summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        await _personService.DeleteAsync(id);

        return NoContent();
    }
}