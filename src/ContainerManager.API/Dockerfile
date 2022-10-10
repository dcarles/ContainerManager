FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine AS build

ARG BUILDCONFIG=RELEASE
ARG VERSION=1.0.0

RUN apk add --no-cache icu-libs krb5-libs libgcc libintl libssl1.1 libstdc++ zlib

ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

# copy csproj and restore as distinct layers
COPY ./src/ContainerManager.API/ContainerManager.API.csproj ./ContainerManager.API/
COPY ./src/ContainerManager.Domain/ContainerManager.Domain.csproj ./ContainerManager.PaymentsCore/
COPY ./src/ContainerManager.Infrastructure/ContainerManager.Infrastructure.csproj ./ContainerManager.Infrastructure/

RUN dotnet restore ContainerManager.API/ContainerManager.API.csproj

# copy everything else and build
COPY ./src/ ./
WORKDIR /ContainerManager.API/
RUN dotnet build ContainerManager.API.csproj -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ContainerManager.API.csproj" -c Release -o /app/publish

ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false


# build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine AS base
FROM base AS final
RUN apk add --no-cache icu-libs krb5-libs libgcc libintl libssl1.1 libstdc++ zlib
WORKDIR /app
COPY --from=publish /app/publish .

EXPOSE 8080
EXPOSE 55100
EXPOSE 443
ENTRYPOINT ["dotnet", "ContainerManager.API.dll"]