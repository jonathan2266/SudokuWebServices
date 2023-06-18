FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /source

# copy csproj and restore as distinct layers
COPY Sudoku.Services.Web/ ./Services
WORKDIR /source/Services/
RUN dotnet restore

WORKDIR /source/Services/Sudoku.Scraper.API
RUN dotnet publish -c release -o /app --no-restore

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "Sudoku.Scraper.API.dll"]