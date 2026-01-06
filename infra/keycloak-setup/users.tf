
resource "keycloak_user" "user1" {
  realm_id = keycloak_realm.mcp.id
  username = "user-1"
  enabled  = true

  first_name     = "User 1"
  last_name      = "Test User"
  email_verified = true
  email          = "user-1@${var.cloudfront_domain}"

  initial_password {
    value     = "StrongPassword123!"
    temporary = false
  }
}

resource "keycloak_user" "user2" {
  realm_id = keycloak_realm.mcp.id
  username = "user-2"
  enabled  = true

  first_name     = "User 2"
  last_name      = "Test User"
  email_verified = true
  email          = "user-2@${var.cloudfront_domain}"

  initial_password {
    value     = "StrongPassword123!"
    temporary = false
  }
}
### ASSIGN ROLES TO USERS ###

resource "keycloak_user_roles" "user1_roles" {
  realm_id = keycloak_realm.mcp.id
  user_id  = keycloak_user.user1.id

  role_ids = [
    keycloak_role.mcp_tools.id,
    keycloak_role.mcp_resources.id
  ]
}

resource "keycloak_user_roles" "user2_resources_roles" {
  realm_id = keycloak_realm.mcp.id
  user_id  = keycloak_user.user2.id

  role_ids = [keycloak_role.mcp_resources.id]
}