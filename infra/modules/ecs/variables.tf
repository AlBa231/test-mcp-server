variable "vpc_id" {
  type = string
}


variable "private_subnets" {
  type = list(string)
}


variable "alb_sg_id" {
  type = string
}


variable "target_group_arn" {
  type = string
}


variable "task_image_uri" {
  type        = string
  default     = "alpine:latest"
  description = "The ECR image URI should be as follows: {data.aws_caller_identity.current.account_id}.dkr.ecr.{data.aws_region.current.region}.amazonaws.com/{var.app_name}:{var.image_tag}"
}

variable "task_image_command" {
  type        = list(string)
  default     = ["tail", "-f", "/dev/null"]
  description = "The command to run in the container"
}

variable "app_name" {
  type        = string
  description = "The name of the application"
}

variable "region" {
  type = string
}
