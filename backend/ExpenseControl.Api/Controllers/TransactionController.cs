using ExpenseControl.Application.DTOs.Transaction;
using ExpenseControl.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseControl.Api.Controllers;

[ApiController]
[Route("api/transactions")]
public sealed class TransactionController
    : ControllerBase
{
    private readonly ITransactionService
        _transactionService;

    public TransactionController(
        ITransactionService transactionService)
    {
        _transactionService = transactionService;
    }

    /// <summary>
    /// Cria uma nova transação para uma pessoa cadastrada.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(
        typeof(TransactionResponseDto),
        StatusCodes.Status201Created)]
    [ProducesResponseType(
        StatusCodes.Status400BadRequest)]
    [ProducesResponseType(
        StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Create(
        [FromBody] CreateTransactionDto dto)
    {
        var transaction =
            await _transactionService.CreateAsync(dto);

        return StatusCode(
            StatusCodes.Status201Created,
            transaction);
    }

    /// <summary>
    /// Retorna todas as transações cadastradas.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(
        typeof(List<TransactionResponseDto>),
        StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var transactions =
            await _transactionService.GetAllAsync();

        return Ok(transactions);
    }

    /// <summary>
    /// Exclui somente a transação selecionada.
    /// </summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(
        StatusCodes.Status204NoContent)]
    [ProducesResponseType(
        StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        await _transactionService.DeleteAsync(id);

        return NoContent();
    }
}