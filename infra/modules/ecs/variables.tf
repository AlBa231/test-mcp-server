variable "vpc_id" {
  type = string
}


variable "private_subnets" {
  type = list(string)
}


variable "alb_sg_ids" {
  type = list(string)
}


variable "target_group_arn" {
  type = string
}


variable "task_image_uri" {
  type        = string
  default     = "alba231/minimal-health-echo:latest"
  description = "The ECR image URI should be as follows: {data.aws_caller_identity.current.account_id}.dkr.ecr.{data.aws_region.current.region}.amazonaws.com/{var.app_name}:{var.image_tag}"
}

variable "app_name" {
  type        = string
  description = "The name of the application"
}

variable "region" {
  type = string
}

variable "cloudfront_domain" {
  type        = string
  description = "The CloudFront domain name to be used by the ECS tasks"
}

variable "enable_authorization" {
  type        = bool
  description = "Whether to enable authorization"
}