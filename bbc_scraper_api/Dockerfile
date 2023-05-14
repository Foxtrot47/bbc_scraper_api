#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["bbc_scraper_api/bbc_scraper_api.csproj", "bbc_scraper_api/"]
RUN dotnet restore "bbc_scraper_api/bbc_scraper_api.csproj"
COPY . .
WORKDIR "/src/bbc_scraper_api"
RUN dotnet build "bbc_scraper_api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "bbc_scraper_api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "bbc_scraper_api.dll"]