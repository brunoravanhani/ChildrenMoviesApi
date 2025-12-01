# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

COPY *.sln ./
COPY src/ChildrenMoviesApi/*.csproj ./ChildrenMoviesApi/
COPY src/ChildrenMoviesApi.Application/*.csproj ./ChildrenMoviesApi.Application/
COPY src/ChildrenMoviesApi.Api/*.csproj ./ChildrenMoviesApi.Api/
COPY src/MoviesDataLoad/*.csproj ./MoviesDataLoad/

COPY . .

WORKDIR /app/ChildrenMoviesApi
RUN dotnet publish "ChildrenMoviesApi.csproj" -c Release -o /app --no-restore

# Etapa final - imagem Lambda
FROM public.ecr.aws/lambda/dotnet:9 AS runtime
WORKDIR /var/task

COPY --from=build /app /var/task

# Handler configurado
CMD ["ChildrenMoviesApi::ChildrenMoviesApi.Function::FunctionHandler"]