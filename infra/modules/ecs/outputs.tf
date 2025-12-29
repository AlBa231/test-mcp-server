output "ecs_cluster_name" {
  value = aws_ecs_cluster.this.name
}

output "service_name" {
  value = aws_ecs_service.this.name
}

output "ecs_security_group" {
  value = aws_security_group.ecs.id
}

output "ecs_roles_arns" {
  value = [aws_ecs_task_definition.execution_role_arn, aws_ecs_task_definition.task_role_arn]
}