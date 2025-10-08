# ExchangeRateAPI

!!! Important

**Note: This is not a complete project. Development has been stopped and it was created only for learning purposes. Some tasks were interrupted halfway through. I decided that further development of the project would no longer be as useful for learning, and I prefer to focus on new projects. I covered the most important concepts I wanted to learn in this project. There is a lot that can be improved, changed, and added here. However, this is not a commercial project.**

!!!

## Overview
ExchangeRateAPI is a sample ASP.NET Core Web API project designed to demonstrate basic concepts of API development, Entity Framework Core, and project structure in .NET.

## Features
- Currency exchange rate endpoints
- API key authentication (sample implementation)
- Entity Framework Core integration
- Basic separation of concerns (API, Core, Infrastructure)

## Project Structure
- `API/` - Main Web API project
- `Core/` - Domain entities and interfaces
- `Infrastructure/` - Data access and service implementations

## Requirements
- .NET 9.0 SDK
- SQL Server (for EF Core migrations)

## Getting Started
1. Clone the repository:
   ```powershell
   git clone https://github.com/DoubleWuOne/ExchangeRateAPI.git
   ```
2. Open the solution in Visual Studio or VS Code.
3. Restore NuGet packages:
   ```powershell
   dotnet restore
   ```
4. Build the solution:
   ```powershell
   dotnet build
   ```
5. Run the API project:
   ```powershell
   dotnet run --project API/API.csproj
   ```

## Usage
This API provides endpoints for retrieving currency exchange rates. See `API/Controllers/ExchangeController.cs` for example endpoints.

## Disclaimer
This project is for educational purposes only and is not maintained.
