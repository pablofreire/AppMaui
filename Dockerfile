FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["SportSphere.API/SportSphere.API.csproj", "SportSphere.API/"]
COPY ["SportSphere.Shared/SportSphere.Shared.csproj", "SportSphere.Shared/"]
RUN dotnet restore "SportSphere.API/SportSphere.API.csproj"
COPY . .
WORKDIR "/src/SportSphere.API"
RUN dotnet build "SportSphere.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SportSphere.API.csproj" -c Release -o /app/publish /p:UseAppHost=false
# Install Entity Framework Core tools
RUN dotnet tool install --global dotnet-ef
ENV PATH="${PATH}:/root/.dotnet/tools"
# Copy migrations to the publish directory
RUN dotnet ef migrations script -o /app/publish/migrations.sql

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
# Install SQL Server tools for running migrations
RUN apt-get update && \
    apt-get install -y curl gnupg && \
    curl https://packages.microsoft.com/keys/microsoft.asc | apt-key add - && \
    curl https://packages.microsoft.com/config/debian/11/prod.list > /etc/apt/sources.list.d/mssql-release.list && \
    apt-get update && \
    ACCEPT_EULA=Y apt-get install -y mssql-tools18 unixodbc-dev
ENV PATH="${PATH}:/opt/mssql-tools18/bin"
ENTRYPOINT ["dotnet", "SportSphere.API.dll"] 