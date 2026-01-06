resource "keycloak_role" "mcp_tools" {
  realm_id  = keycloak_realm.mcp.id
  client_id = keycloak_openid_client.mcp.id

  name = "tools"
}

resource "keycloak_role" "mcp_resources" {
  realm_id  = keycloak_realm.mcp.id
  client_id = keycloak_openid_client.mcp.id

  name = "resources"
}
