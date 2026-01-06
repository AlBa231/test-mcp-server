resource "keycloak_realm" "mcp" {
  realm   = "mcp"
  enabled = true
}

resource "keycloak_openid_client" "mcp" {
  realm_id = keycloak_realm.mcp.id

  client_id                    = "mcp"
  name                         = "MCP Client"
  enabled                      = true
  access_type                  = "PUBLIC"
  standard_flow_enabled        = true
  implicit_flow_enabled        = false
  direct_access_grants_enabled = false
  service_accounts_enabled     = false
  pkce_code_challenge_method   = "S256"
  login_theme                  = "keycloak.v2"


  valid_redirect_uris = [
    "https://${var.cloudfront_domain}/*",
    "http://localhost:6274/*",
    "http://127.0.0.1:6274/*"
  ]
  web_origins = [
    "https://${var.cloudfront_domain}/",
    "https://${var.cloudfront_domain}/lambda-auth",
    "http://localhost:6274",
    "http://127.0.0.1:6274"
  ]
}

### SCOPES ###

resource "keycloak_openid_client_scope" "mcp_tools" {
  realm_id = keycloak_realm.mcp.id
  name     = "mcp:tools"

  include_in_token_scope = true
}

resource "keycloak_openid_client_default_scopes" "mcp_tools_default_scopes" {
  realm_id  = keycloak_realm.mcp.id
  client_id = keycloak_openid_client.mcp.id

  default_scopes = [
    "profile",
    "email",
    keycloak_openid_client_scope.mcp_tools.name
  ]
}

resource "keycloak_openid_user_realm_role_protocol_mapper" "realm_role_mapper" {
  realm_id  = keycloak_realm.mcp.id
  client_id = keycloak_openid_client.mcp.id
  name      = "realm-role-mapper"

  claim_name          = "roles"
  multivalued         = true
  add_to_id_token     = true
  add_to_access_token = true
  add_to_userinfo     = true
}

resource "keycloak_openid_user_client_role_protocol_mapper" "client_role_mapper" {
  realm_id  = keycloak_realm.mcp.id
  client_id = keycloak_openid_client.mcp.id
  name      = "client-role-mapper"

  client_id_for_role_mappings = "mcp"
  claim_name                  = "roles"
  multivalued                 = true
  add_to_id_token             = true
  add_to_access_token         = true
}