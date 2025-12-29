variable "app_name" {
  type        = string
  description = "The ECR repo name"
}

variable "ecs_roles_arns" {
  type        = list(string)
  description = "The roles ARNs for ECS tasks (execution and task roles)"
}

variable "github_repo" {
  type        = string
  description = "The GitHub repository name"
}