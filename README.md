# AWSMCPTestServer


# Testing with MCP Inspector


The Inspector runs directly through npx without requiring installation:

To run HTTP version, call

```bash
npx @modelcontextprotocol/inspector http://localhost:3001
```

To run console (STDIO) version, call

```bash
npx @modelcontextprotocol/inspector dotnet run --project ./AWSMCPTestServer/AWSMCPTestServer --no-build
```

