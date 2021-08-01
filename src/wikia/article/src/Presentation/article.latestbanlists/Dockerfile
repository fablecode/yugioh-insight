FROM mcr.microsoft.com/dotnet/aspnet:5.0-alpine AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine AS build
WORKDIR /src

COPY . .
RUN dotnet restore "article.sln"

FROM build AS publish
RUN dotnet publish "Presentation/article.latestbanlists/article.latestbanlists.csproj" -c Release -o /app/publish --no-restore

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "article.latestbanlists.dll"]