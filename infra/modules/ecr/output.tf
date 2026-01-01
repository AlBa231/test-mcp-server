output "lambda_image_uri" {
  value = "${aws_ecr_repository.lambda_repo.repository_url}@${docker_registry_image.lambda_repo_image.sha256_digest}"
}


output "ecs_image_uri" {
  value = "${aws_ecr_repository.ecs_repo.repository_url}@${docker_registry_image.ecs_repo_image.sha256_digest}"
}