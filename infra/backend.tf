terraform {
  backend "s3" {
    bucket = "test-mcp-server-tfstate"
    key    = "terraform_state.tfstate"
    region = "us-east-1"
  }
}