FROM defradigital/dotnetcore-development:dotnet10.0 AS build-env
USER root
WORKDIR /src

# Copy restore inputs first so dependency restore remains cached when source changes.
COPY ["LoggingMicroservice.API/LoggingMicroservice.API.csproj", "LoggingMicroservice.API/"]
COPY ["Directory.Build.props", "stylecop.ruleset", "NuGet.Config", "./"]

RUN dotnet restore "LoggingMicroservice.API/LoggingMicroservice.API.csproj" --configfile "NuGet.Config"

COPY LoggingMicroservice.API/. LoggingMicroservice.API/.
RUN dotnet publish "LoggingMicroservice.API/LoggingMicroservice.API.csproj" \
    --no-restore \
    --configuration Release \
    --output /app/publish \
    /p:UseAppHost=false

# Keep the runtime image minimal; this image contains the ASP.NET Core runtime only.
FROM defradigital/dotnetcore:dotnet10.0

USER dotnet
WORKDIR /app

COPY --from=build-env /app/publish .

ARG PORT=3000
ENV ASPNETCORE_URLS=http://*:${PORT}
EXPOSE ${PORT}

ENTRYPOINT ["dotnet", "LoggingMicroservice.API.dll"]
