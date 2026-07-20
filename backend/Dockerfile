FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

COPY ExpenseControl.sln ./

COPY ExpenseControl.Api/ExpenseControl.Api.csproj \
    ExpenseControl.Api/

COPY ExpenseControl.Application/ExpenseControl.Application.csproj \
    ExpenseControl.Application/

COPY ExpenseControl.Domain/ExpenseControl.Domain.csproj \
    ExpenseControl.Domain/

COPY ExpenseControl.Infrastructure/ExpenseControl.Infrastructure.csproj \
    ExpenseControl.Infrastructure/

COPY ExpenseControl.Tests/ExpenseControl.Tests.csproj \
    ExpenseControl.Tests/

RUN dotnet restore ExpenseControl.sln

COPY . .

RUN dotnet publish \
    ExpenseControl.Api/ExpenseControl.Api.csproj \
    --configuration Release \
    --output /app/publish \
    --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_HTTP_PORTS=8080

EXPOSE 8080

ENTRYPOINT ["dotnet", "ExpenseControl.Api.dll"]