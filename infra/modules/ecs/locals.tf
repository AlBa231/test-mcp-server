locals {
  container_definitions = [
    {
      name         = var.app_name
      image        = var.task_image_uri
      portMappings = [{ containerPort = 8080 }]
      logConfiguration = {
        logDriver = "awslogs"
        options = {
          "awslogs-group"         = "/ecs/${var.app_name}"
          "awslogs-region"        = var.region
          "awslogs-stream-prefix" = "ecs"
        }
      }
    }
  ]
}