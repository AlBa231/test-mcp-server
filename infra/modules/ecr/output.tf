output "lambda_image_uri" {
  value = aws_ecr_repository.lambda_repo.repository_url
}

output "lambda_initial_image_uri" {
  value = "${aws_ecr_repository.minimal_lambda_alb_go_echo.repository_url}@${docker_registry_image.lambda_image.sha256_digest}"
}