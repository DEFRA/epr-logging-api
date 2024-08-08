FROM defradigital/dotnetcore:dotnet6.0 AS base
USER root
ENV ASPNETCORE_URLS=http://*:8080
EXPOSE 8080
 
RUN apk update && apk --no-cache add icu-libs
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=0
 
FROM defradigital/dotnetcore-development:dotnet6.0 AS build
USER root
WORKDIR /src
COPY ["LoggingMicroservice.API/LoggingMicroservice.API.csproj", "LoggingMicroservice.API/"]
COPY ["stylecop.ruleset"]
COPY ["NuGet.Config", ""]
 
ARG PAT=""
RUN sed -i "s|</configuration>|<packageSourceCredentials><epr-packaging-common><add key=\"Username\" value=\"PAT\" /><add key=\"ClearTextPassword\" value=\"${PAT}\" /></epr-packaging-common></packageSourceCredentials></configuration>|" NuGet.Config
 
RUN dotnet restore "LoggingMicroservice.API/LoggingMicroservice.API.csproj" --configfile "./NuGet.Config"
 
COPY LoggingMicroservice.API/. ./LoggingMicroservice.API/.
COPY LoggingMicroservice.Core/. ./LoggingMicroservice.Core/.
WORKDIR "/src/LoggingMicroservice.API"
RUN dotnet build "LoggingMicroservice.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LoggingMicroservice.API.csproj" -c Release -o /app/publish

 
FROM base AS final
 
# Switch to the non-root user
USER dotnet
 
WORKDIR /app
COPY --from=publish /app/publish .
USER dotnet
ENTRYPOINT ["dotnet", "LoggingMicroservice.API.dll"]