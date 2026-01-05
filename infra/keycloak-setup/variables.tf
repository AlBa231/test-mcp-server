variable "cloudfront_domain" {
  type        = string
  description = "Existing CloudFront domain (dxxx.cloudfront.net)"
}

variable "keycloak_admin_username" {
  type    = string
  default = "admin"
}

variable "keycloak_admin_password" {
  type    = string
  default = "admin"
}
