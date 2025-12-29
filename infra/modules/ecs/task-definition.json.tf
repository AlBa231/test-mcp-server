resource "local_file" "ecs_task_definition_json" {
  filename = "${path.root}/ci/task-definition.json"
  content = jsonencode({
    family                  = aws_ecs_task_definition.this.family
    networkMode             = aws_ecs_task_definition.this.network_mode
    requiresCompatibilities = aws_ecs_task_definition.this.requires_compatibilities
    cpu                     = aws_ecs_task_definition.this.cpu
    memory                  = aws_ecs_task_definition.this.memory
    executionRoleArn        = aws_ecs_task_definition.this.execution_role_arn
    taskRoleArn             = aws_ecs_task_definition.this.task_role_arn
    containerDefinitions    = local.container_definitions
  })
}