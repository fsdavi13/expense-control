using ExpenseControl.Application.Interfaces;
using ExpenseControl.Application.Services.Implementations;
using Microsoft.Extensions.DependencyInjection;

namespace ExpenseControl.Application.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        services.AddScoped<IPersonService, PersonService>();

        services.AddScoped<ITransactionService, TransactionService>();

        services.AddScoped<ITotalsService, TotalsService>();

        return services;
    }
}