FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine AS build-env
WORKDIR /app

# Copy everything and build
COPY . ./
RUN dotnet publish -c Release -o out

# Build runtime image from SDK image so we have dotnet tooling available
#FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine
FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine
RUN adduser --disabled-password \
  --home /app \
  --gecos '' dotnetuser && \
  chown -R dotnetuser /app

# Need to install libxml2 for code coverage measurement
RUN apk update \
    && apk add libxml2 \
    && rm -rf /var/cache/apk/*

USER dotnetuser
WORKDIR /app
COPY --chown=dotnetuser:dotnetuser --from=build-env /app/out .

RUN dotnet tool install -g dotnet-coverage

#ENTRYPOINT ["dotnet", "PetStore.Pet.Api.dll"]
ENTRYPOINT [".dotnet/tools/dotnet-coverage", "collect", "-s", "coverage/coverage_settings.xml", "-o", "coverage/coverage.xml", "-f", "cobertura", "dotnet", "PetStore.Pet.Api.dll"]
