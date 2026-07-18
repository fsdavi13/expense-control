using ExpenseControl.Application.DTOs.Totals;
using ExpenseControl.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseControl.Api.Controllers;

[ApiController]
[Route("api/totals")]
public sealed class TotalsController : ControllerBase
{
    private readonly ITotalsService _totalsService;

    public TotalsController(ITotalsService totalsService)
    {
        _totalsService = totalsService;
    }

    /// <summary>
    /// Retorna os totais de cada pessoa e o total geral.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(
        typeof(TotalsResponseDto),
        StatusCodes.Status200OK)]
    public async Task<IActionResult> Get()
    {
        var totals = await _totalsService.GetAsync();

        return Ok(totals);
    }
}