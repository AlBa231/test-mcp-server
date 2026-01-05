terraform {
  required_providers {
    keycloak = {
      source  = "keycloak/keycloak"
      version = "5.6.0"
    }
  }
}

provider "keycloak" {
  client_id = "admin-cli"
  username  = var.keycloak_admin_username
  password  = var.keycloak_admin_password
  url       = "https://${var.cloudfront_domain}/auth"
  realm     = "master"
}
