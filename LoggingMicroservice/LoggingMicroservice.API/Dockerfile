FROM defradigital/dotnetcore:dotnet6.0 AS base
USER root
ENV ASPNETCORE_URLS=http://*:8080
EXPOSE 8080

FROM defradigital/dotnetcore-development:dotnet6.0 AS build
USER root
WORKDIR /app
COPY LoggingMicroservice.API/. ./LoggingMicroservice.API/.
COPY stylecop.ruleset ./
COPY NuGet.Config ./
COPY Directory.Build.props ./

RUN dotnet restore "LoggingMicroservice.API/LoggingMicroservice.API.csproj" --configfile "NuGet.Config"

# Build and publish a release
WORKDIR /app
RUN dotnet publish -c Release -o out

# Build runtime image
FROM defradigital/dotnetcore:dotnet6.0

# Switch to the non-root user
USER dotnet

COPY --from=build-env app/out .

CMD dotnet LoggingMicroservice.API.dll