# AWSMCPTestServer



# Testing with MCP Inspector


The Inspector runs directly through npx without requiring installation:

To run HTTP version, call

```bash
npx @modelcontextprotocol/inspector http://localhost:8080
```

To run console (STDIO) version, call

```bash
npx @modelcontextprotocol/inspector dotnet run --project ./AWSMCPTestServer/AWSMCPTestServer --no-build
```


To run HTTPS version, hosted on AWS, call

```bash
npx @modelcontextprotocol/inspector https://d1w0taajz0mgpa.cloudfront.net/test/
```

## Deployment on AWS

### Prerequisites
- AWS CLI installed and configured
- Docker installed
- Terraform installed

### Steps to Deploy
1. Review and update ./infra/terraform.tfvars with your App name, AWS Region and GitHub repo name.
1. Run the `deploy.cmd` script to set up the necessary AWS resources.
	```bash
	   ./deploy.cmd
	```

	This script will:
	* Do login if AWS CLI is not logged in.
	* Create S3 bucket for Terraform state
	* Initialize and apply Terraform configuration to create:
		1. ECR repository for Docker images
		1. ECS Cluster
		1. IAM Roles and Policies
		1. Application Load Balancer (ALB)
		1. CloudFront Distribution
		1. Generates `infra/ci/task-definition.json` file for ECS service auto-deployment via GitHub Actions.


#### For automatic deployment via GitHub Actions:
1. Add new generated `infra/ci/task-definition.json` file to GitHub repository.
1. Commit and push changes to GitHub repository to trigger GitHub Actions workflow for deploying the Docker container to ECS.
