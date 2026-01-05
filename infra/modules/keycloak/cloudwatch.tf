resource "aws_cloudwatch_log_group" "ec2_syslog" {
  name              = "/ec2/keycloak-syslog"
  retention_in_days = 14
}

resource "aws_cloudwatch_log_group" "ec2_cloud_init" {
  name              = "/ec2/keycloak-init"
  retention_in_days = 14
}

resource "aws_cloudwatch_log_group" "docker" {
  name              = "/ec2/keycloak-docker"
  retention_in_days = 14
}
