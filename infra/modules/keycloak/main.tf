data "aws_ami" "ubuntu" {
  most_recent = true
  owners      = ["099720109477"]

  filter {
    name   = "name"
    values = ["ubuntu/images/hvm-ssd/ubuntu-jammy-22.04-amd64-server-*"]
  }
}

resource "aws_security_group" "keycloak" {
  name   = "keycloak-sg"
  vpc_id = var.vpc_id

  ingress {
    from_port       = 8080
    to_port         = 8080
    protocol        = "tcp"
    security_groups = var.alb_security_group_ids
  }

  egress {
    from_port   = 0
    to_port     = 0
    protocol    = "-1"
    cidr_blocks = ["0.0.0.0/0"]
  }
}

resource "aws_instance" "keycloak" {
  ami                  = data.aws_ami.ubuntu.id
  instance_type        = "t3.small"
  iam_instance_profile = aws_iam_instance_profile.cloudwatch_profile.name

  subnet_id = var.private_subnet_id

  vpc_security_group_ids = [aws_security_group.keycloak.id]

  user_data = templatefile("${path.module}/ec2-user-data.sh", {
    cloudfront_domain       = var.cloudfront_domain,
    keycloak_admin_username = var.keycloak_admin_username,
    keycloak_admin_password = var.keycloak_admin_password
  })

  user_data_replace_on_change = true

  tags = {
    Name = "keycloak"
  }
}
