FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /source

# copy csproj and restore as distinct layers
COPY Sudoku.Services.Web/ ./Services
WORKDIR /source/Services/
ARG Password
RUN dotnet nuget add source http://192.168.1.17:3000/api/packages/Home/nuget/index.json -n internal.home -u jonathan -p $Password --store-password-in-clear-text
RUN dotnet restore

WORKDIR /source/Services/Sudoku.Scraper.API
RUN dotnet publish -c release -o /app --no-restore

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "Sudoku.Scraper.API.dll"]