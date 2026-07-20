using ExpenseControl.Api.Middleware;
using ExpenseControl.Application.DependencyInjection;
using ExpenseControl.Infrastructure.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

var databaseProvider =
    builder.Configuration["DatabaseProvider"] ?? "não configurado";

Console.WriteLine(
    $"Database provider configurado: {databaseProvider}");

const string FrontendCorsPolicy = "Frontend";

var railwayPort = Environment.GetEnvironmentVariable("PORT");

if (!string.IsNullOrWhiteSpace(railwayPort))
{
    // O Railway fornece a porta dinamicamente em produção.
    builder.WebHost.UseUrls($"http://0.0.0.0:{railwayPort}");
}

var frontendUrl =
    builder.Configuration["FrontendUrl"]
    ?? "http://localhost:5173";

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        FrontendCorsPolicy,
        policy =>
        {
            policy
                .WithOrigins(frontendUrl.TrimEnd('/'))
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// O Swagger permanece disponível no deploy para facilitar
// a avaliação e os testes da API.
app.UseSwagger();
app.UseSwaggerUI();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseCors(FrontendCorsPolicy);

app.MapControllers();

app.MapGet(
    "/health",
    () => Results.Ok(new
    {
        status = "healthy"
    }));

app.Run();