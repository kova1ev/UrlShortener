#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/UrlShortener.Api/UrlShortener.Api.csproj", "src/UrlShortener.Api/"]
COPY ["src/UrlShortener.Application/UrlShortener.Application.csproj", "src/UrlShortener.Application/"]
COPY ["src/UrlShortener.Application.Common/UrlShortener.Application.Common.csproj", "src/UrlShortener.Application.Common/"]
COPY ["src/UrlShortener.Domain/UrlShortener.Domain.csproj", "src/UrlShortener.Domain/"]
COPY ["src/UrlShortener.Data/UrlShortener.Data.csproj", "src/UrlShortener.Data/"]
COPY ["src/UrlShortener.ClientWasm/UrlShortener.ClientWasm.csproj", "src/UrlShortener.ClientWasm/"]
RUN dotnet restore "src/UrlShortener.Api/UrlShortener.Api.csproj"
COPY . .
WORKDIR "/src/src/UrlShortener.Api"
RUN dotnet build "UrlShortener.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "UrlShortener.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UrlShortener.Api.dll"]