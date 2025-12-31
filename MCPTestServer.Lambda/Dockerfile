FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY MCPTestServer.Lambda.csproj ./
RUN dotnet restore

COPY . ./
RUN dotnet publish -c Release -o /app/publish

FROM public.ecr.aws/lambda/dotnet:10

WORKDIR /var/task
COPY --from=build /app/publish .

CMD ["MCPTestServer.Lambda"]