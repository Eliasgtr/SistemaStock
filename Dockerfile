# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /source

# Copy solution and project files for restore caching
COPY *.sln .
COPY Dominio/SistemaStock.Domain.csproj Dominio/
COPY Aplicacion/SistemaStock.Application.csproj Aplicacion/
COPY Infraestructura/SistemaStock.Infrastructure.csproj Infraestructura/
COPY Presentacion/SistemaStock.Web.csproj Presentacion/
RUN dotnet restore Presentacion/SistemaStock.Web.csproj

# Copy code files and publish
COPY Dominio/ Dominio/
COPY Aplicacion/ Aplicacion/
COPY Infraestructura/ Infraestructura/
COPY Presentacion/ Presentacion/
WORKDIR /source/Presentacion
RUN dotnet publish -c Release -o /app --no-restore

# Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app .

# Configure ASP.NET Core environment and expose port
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=true
EXPOSE 8080

ENTRYPOINT ["dotnet", "SistemaStock.Web.dll"]
