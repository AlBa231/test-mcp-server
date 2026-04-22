@echo off

SET AWS_REGION=us-east-1
SET AWS_PROFILE=default
SET AWS_SDK_LOAD_CONFIG=1

aws sts get-caller-identity >nul 2>&1

IF %ERRORLEVEL% NEQ 0 (
    echo AWS credentials expired or missing. Logging in...
    aws login

    echo Set your AWS credentials. Looking for cached config. Copy the credentials if found:

    for %%F in ("%USERPROFILE%\.aws\login\cache\*.json") do (
        type "%%F"
    )

    echo 

    aws configure

) ELSE (
    echo AWS credentials are valid.
)

aws s3api head-bucket --bucket "test-mcp-server-tfstate" 2>nul || aws s3 mb s3://test-mcp-server-tfstate

@echo For terraform apply command, you will be prompted to enter "yes" to confirm the changes. Please review the changes before confirming.
@echo It is intentional to have this confirmation step to prevent accidental changes to your infrastructure.

docker desktop start

cd infra

terraform init

terraform apply

if %ERRORLEVEL% NEQ 0 (
    echo Terraform apply failed. Please check the error messages above and resolve any issues before proceeding.
    pause
    exit /b %ERRORLEVEL%
)

cd keycloak-setup
powershell ./wait_for_keycloak.ps1

terraform init

terraform apply -auto-approve

cd ..\..

pause