resource "aws_security_group" "alb_allow_cloudfront" {
  name   = "alb-sg"
  vpc_id = var.vpc_id

  ingress {
    from_port       = 80
    to_port         = 80
    protocol        = "tcp"
    prefix_list_ids = [data.aws_ec2_managed_prefix_list.cloudfront_ipv4.id]
  }

  egress {
    from_port   = 0
    to_port     = 0
    protocol    = "-1"
    cidr_blocks = ["0.0.0.0/0"]
  }
}

resource "aws_security_group" "alb_allow_cloudfront_ipv6" {
  name   = "alb-sg-ipv6"
  vpc_id = var.vpc_id

  ingress {
    from_port       = 80
    to_port         = 80
    protocol        = "tcp"
    prefix_list_ids = [data.aws_ec2_managed_prefix_list.cloudfront_ipv6.id]
  }
}

resource "aws_lb" "this" {
  name               = "ecs-alb"
  internal           = true
  load_balancer_type = "application"
  subnets            = var.subnets
  security_groups    = [aws_security_group.alb_allow_cloudfront.id, aws_security_group.alb_allow_cloudfront_ipv6.id]
}

resource "aws_lb_target_group" "ecs" {
  name        = "ecs-tg"
  port        = 8080
  protocol    = "HTTP"
  target_type = "ip"
  vpc_id      = var.vpc_id

  health_check {
    path = "/health"
  }
}

resource "aws_lb_listener" "http" {
  load_balancer_arn = aws_lb.this.arn
  port              = 80
  protocol          = "HTTP"

  default_action {
    type             = "forward"
    target_group_arn = aws_lb_target_group.ecs.arn
  }
}