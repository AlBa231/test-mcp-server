output "alb_dns_name" {
  value = aws_lb.this.dns_name
}


output "alb_sg_ids" {
  value = [aws_security_group.alb_allow_cloudfront.id, aws_security_group.alb_allow_cloudfront_ipv6.id]
}


output "target_group_arn" {
  value = aws_lb_target_group.ecs.arn
}

output "alb_http_listener_arn" {
  value = aws_lb_listener.http.arn
}

output "vpc_origin_id" {
  value = aws_cloudfront_vpc_origin.alb_origin.id
}
