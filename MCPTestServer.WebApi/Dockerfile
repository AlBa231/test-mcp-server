# ---------- runtime ----------
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

# ---------- build ----------
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY MCPTestServer.WebApi/MCPTestServer.WebApi.csproj MCPTestServer.WebApi/
COPY MCPTestServer.Core/MCPTestServer.Core.csproj MCPTestServer.Core/

RUN dotnet restore MCPTestServer.WebApi/MCPTestServer.WebApi.csproj

COPY . .
WORKDIR /src/MCPTestServer.WebApi

RUN dotnet publish MCPTestServer.WebApi.csproj \
    -c $BUILD_CONFIGURATION \
    -o /app/publish \
    /p:UseAppHost=false

# ---------- final ----------
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "MCPTestServer.WebApi.dll"]
