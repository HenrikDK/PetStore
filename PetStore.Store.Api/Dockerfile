FROM mcr.microsoft.com/dotnet/core/sdk:5.0-alpine AS build-env
WORKDIR /app

# Copy everything and build
COPY . ./
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:5.0-alpine
RUN adduser --disabled-password \
  --home /app \
  --gecos '' dotnetuser && chown -R dotnetuser /app

WORKDIR /app
COPY --from=build-env /app/out .

USER dotnetuser
ENTRYPOINT ["dotnet", "PetStore.Store.Api.dll"]