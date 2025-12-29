locals {
  container_definitions = [
    {
      name         = var.app_name
      image        = var.task_image_uri
      portMappings = [{ containerPort = 8080 }]
      logConfiguration = {
        logDriver = "awslogs"
        options = {
          "awslogs-group"         = aws_cloudwatch_log_group.ecs_log_group.name
          "awslogs-region"        = var.region
          "awslogs-stream-prefix" = "ecs"
        }
      }
    }
  ]
}