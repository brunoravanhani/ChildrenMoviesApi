FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY *.sln ./

COPY src/ChildrenMoviesApi.Application/*.csproj src/ChildrenMoviesApi.Application/
COPY src/ChildrenMoviesApi.Api/*.csproj src/ChildrenMoviesApi.Api/
COPY src/ChildrenMoviesApi/*.csproj src/ChildrenMoviesApi/
COPY src/MoviesDataLoad/*.csproj src/MoviesDataLoad/
COPY test/ChildrenMoviesApi.Tests/*.csproj test/ChildrenMoviesApi.Tests/

RUN dotnet restore

COPY src/ ./src/

WORKDIR /src/src/ChildrenMoviesApi.Api
RUN dotnet publish -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080

ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "ChildrenMoviesApi.Api.dll"]
