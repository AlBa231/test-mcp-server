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
- Deployed infrastructure from [AWS Guidance](https://github.com/aws-solutions-library-samples/guidance-for-deploying-model-context-protocol-servers-on-aws)

### Steps to Deploy
1. Create AWS Role with necessary permissions for Lambda and API Gateway. (see .deploy/role-policy.json)
1. Create ECR repository to store Docker image.
```bash
	aws ecr create-repository \
	  --repository-name mcp-test-web-api \
	  --region us-east-1
```

1. Setup Application Load Balancer (ALB) with HTTPS listener and target group.
	* Add Target Group for mcp-test-web-api ECS service. Map to port 8080.
	* Edit HTTPS listener to forward requests to the target group. (use path pattern /test/\*)
1. Update CI pipeline to build and push Docker image to ECR, and deploy ECS service. 
	* Replace Account ID in .github/\*.yml and .deploy/\*.json
	* Replace Resource IDs in .deploy/\*.json and .github/\*.yml
	* Replace Role Arns in .deploy/\*.json and .github/\*.yml