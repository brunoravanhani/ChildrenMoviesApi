# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY *.sln ./
COPY ChildrenMoviesApi/*.csproj ./ChildrenMoviesApi/
COPY ChildrenMoviesApi.Application/*.csproj ./ChildrenMoviesApi.Application/

RUN dotnet restore

# Copia o restante dos arquivos
COPY . .

# Publica
RUN dotnet publish -c Release -o /app --no-restore

# Etapa final - imagem Lambda
FROM public.ecr.aws/lambda/dotnet:9 AS runtime
WORKDIR /var/task

COPY --from=build /app /var/task

# Handler configurado
CMD ["ChildrenMoviesApi::ChildrenMoviesApi.Function::FunctionHandler"]