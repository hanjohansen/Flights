# ----base image
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# ----build app
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

# install nodejs to run npx/tailwind
RUN apt-get update
RUN apt-get -y install curl gnupg
RUN curl -sL https://deb.nodesource.com/setup_20.x  | bash -
RUN apt-get -y install nodejs

COPY . .
WORKDIR /src/Flights.Client
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