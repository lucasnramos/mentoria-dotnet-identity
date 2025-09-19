FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy csproj files for restore (only the projects referenced by the API)
COPY Services/Identity.API/Identity.API.csproj Services/Identity.API/
COPY Modules/Identity.Application/Identity.Application.csproj Modules/Identity.Application/
COPY Modules/Identity.Domain/Identity.Domain.csproj Modules/Identity.Domain/
COPY Modules/Identity.Infrastructure.IoC/Identity.Infrastructure.IoC.csproj Modules/Identity.Infrastructure.IoC/
COPY Modules/Identity.Infrastructure.Repositories/Identity.Infrastructure.Repositories.csproj Modules/Identity.Infrastructure.Repositories/
COPY Adapters/Authentication.Adapter/Authentication.Adapter.csproj Adapters/Authentication.Adapter/

# Restore only for the API project (faster)
RUN dotnet restore Services/Identity.API/Identity.API.csproj

# Copy the rest of the source and publish
COPY . .

RUN dotnet publish Services/Identity.API/Identity.API.csproj -c Release -o /app/publish /p:UseAppHost=false

# Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Identity.API.dll"]