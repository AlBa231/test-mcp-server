
resource "aws_ecr_repository" "ecs_repo" {
  name = var.app_name

  image_scanning_configuration {
    scan_on_push = true
  }

  tags = {
    Name = var.app_name
  }
}

resource "aws_ecr_lifecycle_policy" "ecs_repo" {
  repository = aws_ecr_repository.ecs_repo.name

  policy = jsonencode({
    rules = [
      {
        rulePriority = 1
        description  = "Keep last 5 images"
        selection = {
          tagStatus   = "any"
          countType   = "imageCountMoreThan"
          countNumber = 5
        }
        action = {
          type = "expire"
        }
      }
    ]
  })
}

resource "docker_image" "ecs_repo_image" {
  name = "${aws_ecr_repository.ecs_repo.repository_url}:latest"

  build {
    context    = "${path.root}/../"
    dockerfile = "MCPTestServer.WebApi/Dockerfile"
  }

  keep_locally = false
}

resource "docker_registry_image" "ecs_repo_image" {
  name = docker_image.ecs_repo_image.name
}
