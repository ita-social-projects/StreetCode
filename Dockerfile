FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

# adding curl and gpg for healthcheck
RUN apt-get update \
    && apt-get install -y --no-install-recommends \
    curl \
    gpg \
    && rm -rf /var/lib/apt/lists/*
EXPOSE 5000
EXPOSE 5001
EXPOSE 80
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG Configuration=debug

#restoring dependencies
COPY ./Streetcode/*.sln ./
COPY ./Streetcode/Streetcode.WebApi/*.csproj ./Streetcode.WebApi/
COPY ./Streetcode/Streetcode.BLL/*.csproj ./Streetcode.BLL/
COPY ./Streetcode/Streetcode.DAL/*.csproj ./Streetcode.DAL/
COPY ./Streetcode/Streetcode.XUnitTest/*.csproj ./Streetcode.XUnitTest/
COPY ./Streetcode/Streetcode.XIntegrationTest/*.csproj ./Streetcode.XIntegrationTest/
COPY ./Streetcode/DbUpdate/*.csproj ./DbUpdate/
COPY ./houses.zip ./
RUN dotnet restore

# copying other neccessary data and building application
COPY ./Streetcode/ ./
RUN dotnet build -c $Configuration -o /app/build

# publishishing application
FROM build AS publish
RUN dotnet publish -c $Configuration -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish ./

LABEL atom="Streetcode"
ENTRYPOINT ["dotnet", "Streetcode.WebApi.dll", "--environment=Production"]