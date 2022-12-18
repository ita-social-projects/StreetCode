FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

# adding curl and gpg for healthcheck
RUN apt-get update \
    && apt-get install -y --no-install-recommends \
    curl \
    gpg \
    && rm -rf /var/lib/apt/lists/*
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG Configuration=debug
WORKDIR /src

#restoring dependencies
COPY ./*.sln ./
COPY ./Streetcode.WebApi/*.csproj ./Streetcode.WebApi/
COPY ./Streetcode.DAL/*.csproj ./Streetcode.DAL/
COPY ./Streetcode.BLL/*.csproj ./Streetcode.BLL/
COPY ./Streetcode.XUnitTest/*.csproj ./Streetcode.XUnitTest/
RUN dotnet restore

# copying other neccessary data and building application
COPY ./ ./
RUN dotnet build -c $Configuration -o /app/build

# publishishing application
FROM build AS publish
RUN dotnet publish -c $Configuration -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish ./
ENTRYPOINT ["dotnet", "Streetcode.WebApi.dll"]