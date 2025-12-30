variable "vpc_id" {
  type        = string
  description = "The VPC ID to deploy lambda function into"
}

variable "app_name" {
  type        = string
  description = "The name of app to use to construct resource names"
}

variable "alb_http_listener_arn" {
  type        = string
  description = "The HTTP listener ARN from the ALB"
}

variable "lambda_image_uri" {
  type        = string
  default     = "alba231/minimal-lambda-alb-go-echo:latest"
  description = "The ECR image URI for the Lambda function. By default, it points to a public image with echo response."
}
