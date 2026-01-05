output "AuthorizationServerUrl" {
  value = "https://${var.cloudfront_domain}/auth/realms/${keycloak_realm.mcp.realm}"
}