FROM defradigital/dotnetcore-development:latest AS build-env

# Expose the app on a defined port, configurable via a build argument
ARG PORT=3000
ENV ASPNETCORE_URLS=http://*:${PORT}
EXPOSE ${PORT}

# Copy everything
COPY --chown=dotnet LoggingMicroservice.API/. ./LoggingMicroservice.API/.
COPY --chown=dotnet stylecop.ruleset ./
COPY --chown=dotnet NuGet.Config ./
COPY --chown=dotnet Directory.Build.props ./

RUN dotnet restore "LoggingMicroservice.API/LoggingMicroservice.API.csproj" --configfile "NuGet.Config"

# Build and publish a release
WORKDIR /home/dotnet/LoggingMicroservice.API
RUN dotnet publish -c Release -o out

# Build runtime image
FROM defradigital/dotnetcore:latest

# Switch to the non-root user
USER dotnet

COPY --from=build-env --chown=dotnet /home/dotnet/LoggingMicroservice.API/out .

# Expose the app on a defined port, configurable via a build argument
ARG PORT=3000
ENV ASPNETCORE_URLS=http://*:${PORT}
EXPOSE ${PORT}

CMD dotnet LoggingMicroservice.API.dll
