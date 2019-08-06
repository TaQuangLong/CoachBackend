FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["EverCoach/EverCoach.Api.csproj", "EverCoach/"]
COPY ["EverCoach.Domain/EverCoach.Domain.csproj", "EverCoach.Domain/"]
RUN dotnet restore "EverCoach/EverCoach.Api.csproj"
COPY . .
WORKDIR "/src/EverCoach"
RUN dotnet build "EverCoach.Api.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "EverCoach.Api.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "EverCoach.Api.dll"]