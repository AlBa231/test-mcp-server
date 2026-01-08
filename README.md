# MCP Test Server on AWS

A reference implementation demonstrating the Model Context Protocol (MCP) on AWS. This repository provides multiple hosts and Terraform infrastructure to show how MCP Servers and Clients can be implemented, tested and deployed.

Target framework: `.NET 10`

## Repository layout

- `MCPTestServer.Core` — shared module containing Resources, Tools and Prompts used across hosts
- `MCPTestServer.Console` — simple STDIO-based MCP Server implementation
- `MCPTestServer.WebApi` — ASP.NET Web API implementation for ECS deployment
- `MCPTestServer.Lambda` — stateless ASP.NET project optimized for AWS Lambda
- `infra` — Terraform project containing AWS infrastructure
- `infra/keycloak-setup` — Terraform project used to set up Keycloak

## MCP features demonstrated

Server:
- Tools
- Resources
- Prompts

Client:
- Elicitation
- Sampling
- Roots

## Quick start (local)

Prerequisites:
- `.NET 10` SDK installed
- `Docker` (for container builds)
- `Node.js` (for running `@modelcontextprotocol/inspector`)

Build and run the console server:

```bash
dotnet build
dotnet run --project ./MCPTestServer.Console
```

If your workspace contains `AWSMCPTestServer.Console`, use `./AWSMCPTestServer.Console` instead.

Run the Web API locally:

```bash
dotnet run --project ./MCPTestServer.WebApi
```

Run the Lambda host locally:

```bash
dotnet run --project ./MCPTestServer.Lambda
```

Run unit tests:

```bash
dotnet test
```

Format code:

```bash
dotnet format
```

## Testing with MCP Inspector

Replace `[CLOUDFRONT_DISTRIBUTION_ID]` with your CloudFront distribution id (for example `d1w0taajz0mgpa`).

HTTP (local Web API):

```bash
npx @modelcontextprotocol/inspector http://localhost:8080
```

Console (STDIO) example(s):

```bash
npx @modelcontextprotocol/inspector dotnet run --project ./MCPTestServer.Console --no-build
# or
npx @modelcontextprotocol/inspector dotnet run --project ./AWSMCPTestServer.Console --no-build
```

HTTPS (hosted on AWS):

```bash
npx @modelcontextprotocol/inspector https://[CLOUDFRONT_DISTRIBUTION_ID].cloudfront.net/
```

Stateless HTTPS (Lambda):

```bash
npx @modelcontextprotocol/inspector https://[CLOUDFRONT_DISTRIBUTION_ID].cloudfront.net/lambda/
```

Open Keycloak UI:

```
https://[CLOUDFRONT_DISTRIBUTION_ID].cloudfront.net/auth/
```

## Deployment on AWS

### Prerequisites
- `AWS CLI` installed and configured
- `Docker` installed
- `Terraform` installed

### Deploy steps
1. Edit `./infra/terraform.tfvars` and set `app_name`, `aws_region` and your GitHub repo values.
2. Run the helper script:

```bash
./deploy.cmd
```

What the script does:
- Ensures AWS CLI login
- Creates S3 bucket for Terraform state
- Initializes and applies Terraform to create:
  - ECR repository for Docker images
  - ECS cluster and service definitions
  - IAM roles and policies
  - Application Load Balancer (ALB)
  - CloudFront distribution
  - Generates `infra/ci/task-definition.json` used by GitHub Actions for ECS deploy

#### Automatic deployment via GitHub Actions
1. Add the generated `infra/ci/task-definition.json` to the repository (after running `./deploy.cmd`).
2. Commit and push to trigger the workflow in `.github/workflows` which builds and deploys the container to ECS.

## Contributing & standards

This project enforces repository-wide standards to keep code and documentation consistent.

- Formatting and style: follow `.editorconfig`. Run `dotnet format` locally.
- Code reviews: open a pull request targeting `master`. Keep PRs small and include a clear description and any migration steps.
- Branching: use `feature/<short-description>` for features and `hotfix/<issue-number>` for urgent fixes.
- Testing: add unit tests for new behavior. Run `dotnet test` before pushing changes.
- CI: GitHub Actions are configured to build, test and deploy. Inspect `.github/workflows` for details.
- Documentation: update `README.md` and any project-specific READMEs for user-facing changes; small docs fixes may be included in the same PR.

Files to consult:
- `./.editorconfig` — repository formatting and style rules

## Troubleshooting & notes

- If Terraform fails to provision a resource, inspect the Terraform state in the S3 bucket created by `./deploy.cmd`.
- Use application logs from `MCPTestServer.WebApi` and ECS task logs to investigate runtime issues.
- Ensure deployment runtimes match `.NET 10`.
- For permission errors during deploy, confirm the AWS credentials and IAM policies used by the CLI have required permissions.

## Where to look next

- Source code for tools, resources and prompts: `MCPTestServer.Core`
- Web API configuration and extensions: `MCPTestServer.WebApi`
- CI/CD workflows: `.github/workflows`

---
