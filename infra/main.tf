terraform {
  required_providers {
    aws = {
      source  = "hashicorp/aws"
      version = "~> 6.3.0"
    }
  }

  required_version = "~> 1.13"
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
  source    = "./modules/ecr"
  repo_name = var.image_name
}

module "alb" {
  source  = "./modules/alb"
  vpc_id  = module.vpc.vpc_id
  subnets = module.vpc.public_subnets
}

module "ecs" {
  source           = "./modules/ecs"
  vpc_id           = module.vpc.vpc_id
  private_subnets  = module.vpc.private_subnets
  alb_sg_id        = module.alb.alb_sg_id
  target_group_arn = module.alb.target_group_arn
  region           = var.region
  log_group_name   = var.image_name
}

module "cloudfront" {
  source       = "./modules/cloudfront"
  alb_dns_name = module.alb.alb_dns_name
}

module "ci" {
  source         = "./modules/ci"
  ecs_roles_arns = module.ecs.ecs_roles_arns
  repo_name      = var.image_name
}