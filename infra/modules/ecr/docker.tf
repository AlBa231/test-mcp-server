terraform {
  required_providers {
    docker = {
      source  = "kreuzwerker/docker"
      version = "~> 3.6.2"
    }
  }

  required_version = "~> 1.13"
}

data "aws_ecr_authorization_token" "this" {}

provider "docker" {
  registry_auth {
    address  = data.aws_ecr_authorization_token.this.proxy_endpoint
    username = data.aws_ecr_authorization_token.this.user_name
    password = data.aws_ecr_authorization_token.this.password
  }  
}

resource "aws_ecr_repository" "minimal_lambda_alb_go_echo" {
  name = "minimal-lambda-alb-go-echo"
}

resource "docker_image" "lambda_image" {
  name = "${aws_ecr_repository.minimal_lambda_alb_go_echo.repository_url}:latest"
  
  build {
    context    = "${path.root}/docker/minimal-lambda-alb-http-server"
    dockerfile = "Dockerfile"
  }

  keep_locally = false
}

resource "docker_registry_image" "lambda_image" {
  name = docker_image.lambda_image.name
}
