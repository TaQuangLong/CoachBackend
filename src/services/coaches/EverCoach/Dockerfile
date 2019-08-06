FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["src/services/coaches/EverCoach/EverCoach.Api.csproj", "src/services/coaches/EverCoach/"]
COPY ["src/services/coaches/EverCoach.Infrastructure/EverCoach.Infrastructure.csproj", "src/services/coaches/EverCoach.Infrastructure/"]
COPY ["src/services/coaches/EverCoach.Domain/EverCoach.Domain.csproj", "src/services/coaches/EverCoach.Domain/"]
RUN dotnet restore "src/services/coaches/EverCoach/EverCoach.Api.csproj"
COPY . .
WORKDIR "/src/src/services/coaches/EverCoach"
RUN dotnet build "EverCoach.Api.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "EverCoach.Api.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "EverCoach.Api.dll"]