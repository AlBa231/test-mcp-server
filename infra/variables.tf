variable "region" {
  type    = string
  default = "us-east-1"
}

variable "azs" {
  description = "Available Zones for the VPC"
  type        = list(string)
  default     = ["us-east-1a", "us-east-1b"]
}

variable "app_name" {
  type        = string
  description = "The name of the App to use in the ECS image in the ECR repository and as a part of other resource names"
}

variable "github_repo" {
  type        = string
  description = "The GitHub repository name"
}

variable "keycloak_admin_username" {
  type        = string
  description = "The Keycloak admin username"
}

variable "keycloak_admin_password" {
  type        = string
  description = "The Keycloak admin password"
}

variable "is_windows" {
  type    = bool
  default = true
}