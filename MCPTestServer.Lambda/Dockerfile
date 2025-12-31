# ---------- build ----------
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY MCPTestServer.Lambda/MCPTestServer.Lambda.csproj MCPTestServer.Lambda/
COPY McpTestServer.Core/McpTestServer.Core.csproj McpTestServer.Core/

RUN dotnet restore MCPTestServer.Lambda/MCPTestServer.Lambda.csproj

COPY . .
WORKDIR /src/MCPTestServer.Lambda

RUN dotnet publish MCPTestServer.Lambda.csproj \
    -c $BUILD_CONFIGURATION \
    -o /app/publish

# ---------- final ----------
FROM public.ecr.aws/lambda/dotnet:10

WORKDIR /var/task
COPY --from=build /app/publish .
CMD ["Amazon.Lambda.AspNetCoreServer.Hosting::Amazon.Lambda.AspNetCoreServer.Hosting.LambdaEntryPoint::FunctionHandlerAsync"]
