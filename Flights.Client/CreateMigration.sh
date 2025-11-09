#!/bin/sh
echo Enter migration name:

read migName

echo "--------------------------------------------------------------------------------------------------------"
command="dotnet ef migrations add ${migName} --project ../Flights.Storage/Flights.Storage.Sqlite/Flights.Storage.Sqlite.csproj --context FlightsDbContext -- --Storage:DbProvider Sqlite --Storage:ConnectionString 'DataSource=app.db;Cache=Shared;foreign keys=true'"
echo "Sqlite -------------------------------------------------------------------------------------------------"
echo "${command}"
echo "--------------------------------------------------------------------------------------------------------"
echo ""
echo "Mysql -------------------------------------------------------------------------------------------------"
command="dotnet ef migrations add ${migName} --project ../Flights.Storage/Flights.Storage.MySql/Flights.Storage.MySql.csproj --context FlightsDbContext -- --Storage:DbProvider MySql --Storage:ConnectionString 'Server=192.168.170.110;Port=3306;Database=flights;Uid=flights;Pwd=flights;'"
echo "${command}"
echo "--------------------------------------------------------------------------------------------------------"
eval ${command}