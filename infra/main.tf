terraform {
  required_providers {
    aws = {
      source  = "hashicorp/aws"
      version = "~> 6.27.0"
    }
    keycloak = {
      source  = "keycloak/keycloak"
      version = "5.6.0"
    }
  }

  required_version = "~> 1.14"
}

provider "aws" {
  region  = var.region
  profile = "default"
}


module "vpc" {
  source = "./modules/vpc"
  azs    = var.azs
}

module "ecr" {
  source   = "./modules/ecr"
  app_name = var.app_name
}

module "alb" {
  source  = "./modules/alb"
  vpc_id  = module.vpc.vpc_id
  subnets = module.vpc.public_subnets
}

module "ecs" {
  source               = "./modules/ecs"
  vpc_id               = module.vpc.vpc_id
  private_subnets      = module.vpc.private_subnets
  alb_sg_ids           = module.alb.alb_sg_ids
  target_group_arn     = module.alb.target_group_arn
  region               = var.region
  app_name             = var.app_name
  task_image_uri       = module.ecr.ecs_image_uri
  cloudfront_domain    = module.cloudfront.cloudfront_domain_name
  enable_authorization = var.enable_authorization
}

module "cloudfront" {
  source       = "./modules/cloudfront"
  alb_dns_name = module.alb.alb_dns_name
}

module "ci" {
  source         = "./modules/ci"
  ecs_roles_arns = module.ecs.ecs_roles_arns
  app_name       = var.app_name
  github_repo    = var.github_repo
}

module "lambda" {
  source                = "./modules/lambda"
  app_name              = var.app_name
  alb_http_listener_arn = module.alb.alb_http_listener_arn
  lambda_image_uri      = module.ecr.lambda_image_uri
}

module "keycloak" {
  source = "./modules/keycloak"

  cloudfront_domain       = module.cloudfront.cloudfront_domain_name
  alb_security_group_ids  = module.alb.alb_sg_ids
  alb_http_listener_arn   = module.alb.alb_http_listener_arn
  vpc_id                  = module.vpc.vpc_id
  private_subnet_id       = element(module.vpc.private_subnets, 0)
  keycloak_admin_username = var.keycloak_admin_username
  keycloak_admin_password = var.keycloak_admin_password
}

resource "local_file" "keycloak_config" {
  filename = "${path.root}/keycloak-setup/terraform.tfvars"
  content  = <<EOT
    cloudfront_domain = "${module.cloudfront.cloudfront_domain_name}"
    keycloak_admin_username = "${var.keycloak_admin_username}"
    keycloak_admin_password = "${var.keycloak_admin_password}"
  EOT
}

resource "local_file" "wait_for_keycloak" {
  filename = "${path.root}/keycloak-setup/wait_for_keycloak.ps1"
  content  = <<EOT
        $maxTries = 30
        $delay = 5
        for ($i=0; $i -lt $maxTries; $i++) {
          try {
            $response = Invoke-WebRequest -Uri https://${module.cloudfront.cloudfront_domain_name}/auth/realms/master -UseBasicParsing -ErrorAction Stop
            Write-Host "Checking the URL.... Status - HTTP status code: $($response.StatusCode)"
            if ($response.StatusCode -eq 200) { exit 0 }
          } catch {
            Write-Host "Error. $($_.Exception.Message). Retrying..."
            Start-Sleep -Seconds $delay
          }
        }    
  EOT
}