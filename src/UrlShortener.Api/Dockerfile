FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build-env
WORKDIR /app

COPY UrlShortener.sln ./

COPY src/UrlShortener.Domain/UrlShortener.Domain.csproj src/UrlShortener.Domain/
COPY src/UrlShortener.Infrastructure/UrlShortener.Infrastructure.csproj src/UrlShortener.Infrastructure/
COPY src/UrlShortener.Api/UrlShortener.Api.csproj src/UrlShortener.Api/
COPY src/UrlShortener.Application/UrlShortener.Application.csproj src/UrlShortener.Application/
COPY tests/UrlShortener.Tests/UrlShortener.Tests.csproj tests/UrlShortener.Tests/

RUN dotnet restore UrlShortener.sln

COPY src/UrlShortener.Domain/ src/UrlShortener.Domain/
COPY src/UrlShortener.Infrastructure/ src/UrlShortener.Infrastructure/
COPY src/UrlShortener.Api/ src/UrlShortener.Api/
COPY src/UrlShortener.Application/ src/UrlShortener.Application/
COPY tests/UrlShortener.Tests/ tests/UrlShortener.Tests/

RUN dotnet publish src/UrlShortener.Api/UrlShortener.Api.csproj -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build-env /app/out .

EXPOSE 8080

ENTRYPOINT ["dotnet", "UrlShortener.Api.dll"]
