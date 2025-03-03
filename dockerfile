# ----base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 443

# ----build app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY . .
WORKDIR /src/Flights.Client

# restore and build app
RUN dotnet restore "Flights.Client.csproj"
RUN dotnet build "Flights.Client.csproj" -c Release -o /app/build

# ----publish
FROM build AS publish
RUN dotnet publish "Flights.Client.csproj" -c Release -o /app/publish

# ----run
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Flights.Client.dll", "--environment=Production"]