$projectRoot = $PSScriptRoot

Start-Process powershell -ArgumentList `
    "-NoExit", `
    "-Command", `
    "Set-Location '$projectRoot\backend'; dotnet run --project ExpenseControl.Api"

Start-Process powershell -ArgumentList `
    "-NoExit", `
    "-Command", `
    "Set-Location '$projectRoot\frontend'; npm run dev"