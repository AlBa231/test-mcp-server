variable "region" {
  type    = string
  default = "us-east-1"
}

variable "azs" {
  description = "Available Zones for the VPC"
  type        = list(string)
  default     = ["us-east-1a", "us-east-1b"]
}

variable "image_name" {
  type        = string
  description = "The name of the ECS image in the ECR repository"
}