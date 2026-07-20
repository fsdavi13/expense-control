using ExpenseControl.Domain.Repositories;
using ExpenseControl.Infrastructure.Data;
using ExpenseControl.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExpenseControl.Infrastructure.DependencyInjection;

public static class InfrastructureDependencyInjection
{
        public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var databaseProvider =
            configuration["DatabaseProvider"] ?? "Sqlite";

        var connectionString =
            configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException(
                "A string de conexão do banco não foi configurada.");

        if (
            databaseProvider.Equals(
                "Postgres",
                StringComparison.OrdinalIgnoreCase)
        )
        {
            services.AddDbContext<PostgresExpenseControlDbContext>(
                options => options.UseNpgsql(connectionString));

            // Os repositórios continuam dependendo do contexto principal,
            // mas em produção recebem a instância configurada para PostgreSQL.
            services.AddScoped<ExpenseControlDbContext>(
                provider =>
                    provider.GetRequiredService<
                        PostgresExpenseControlDbContext>());
        }
        else
        {
            services.AddDbContext<ExpenseControlDbContext>(
                options => options.UseSqlite(connectionString));
        }

        services.AddScoped<IPersonRepository, PersonRepository>();

        services.AddScoped<
            ITransactionRepository,
            TransactionRepository>();

        return services;
    }
}