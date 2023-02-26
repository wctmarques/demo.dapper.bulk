#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Demo.Dapper.Bulk/Demo.Dapper.Bulk.csproj", "Demo.Dapper.Bulk/"]
RUN dotnet restore "Demo.Dapper.Bulk/Demo.Dapper.Bulk.csproj"
COPY . .
WORKDIR "/src/Demo.Dapper.Bulk"
RUN dotnet build "Demo.Dapper.Bulk.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Demo.Dapper.Bulk.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Demo.Dapper.Bulk.dll"]