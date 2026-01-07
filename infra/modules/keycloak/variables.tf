variable "alb_http_listener_arn" {
  type        = string
  description = "The HTTP listener ARN from the ALB"
}

variable "cloudfront_domain" {
  type        = string
  description = "Existing CloudFront domain (dxxx.cloudfront.net)"
  default     = null
}

variable "vpc_id" {
  type        = string
  description = "The VPC ID where the Keycloak instance will be deployed."
}

variable "alb_security_group_ids" {
  type        = list(string)
  description = "The security group IDs associated with the ALB."
}

variable "private_subnet_id" {
  type        = string
  description = "The private subnet ID where the Keycloak instance will be deployed."
}

variable "keycloak_admin_username" {
  type    = string
  default = "admin"
}

variable "keycloak_admin_password" {
  type    = string
  default = "admin"
}
