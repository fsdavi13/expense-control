using ExpenseControl.Domain.Repositories;
using ExpenseControl.Infrastructure.Data;
using ExpenseControl.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExpenseControl.Infrastructure.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString(
            "DefaultConnection");

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException(
                "A connection string 'DefaultConnection' não foi configurada.");
        }

        services.AddDbContext<ExpenseControlDbContext>(options =>
            options.UseSqlite(connectionString));

        services.AddScoped<IPersonRepository, PersonRepository>();
        services.AddScoped<ITransactionRepository, TransactionRepository>();

        return services;
    }
}