terraform {
  backend "s3" {
    bucket = "test-mcp-server-tfstate"
    key    = "keycloak_state.tfstate"
    region = "us-east-1"
  }
}