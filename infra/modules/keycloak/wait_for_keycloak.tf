
resource "local_file" "keycloak_config" {
  filename = "${path.root}/keycloak-setup/terraform.tfvars"
  content  = <<EOT
    cloudfront_domain = "${var.cloudfront_domain}"
    keycloak_admin_username = "${var.keycloak_admin_username}"
    keycloak_admin_password = "${var.keycloak_admin_password}"
  EOT
}

resource "local_file" "wait_for_keycloak" {
  filename = "${path.root}/keycloak-setup/wait_for_keycloak.ps1"
  content  = <<EOT
        $maxTries = 30
        $delay = 5
        for ($i=0; $i -lt $maxTries; $i++) {
          try {
            $response = Invoke-WebRequest -Uri https://${var.cloudfront_domain}/auth/realms/master -UseBasicParsing -ErrorAction Stop
            Write-Host "Checking the URL.... Status - HTTP status code: $($response.StatusCode)"
            if ($response.StatusCode -eq 200) { exit 0 }
          } catch {
            Write-Host "Error. $($_.Exception.Message). Retrying..."
            Start-Sleep -Seconds $delay
          }
        }    
  EOT
}