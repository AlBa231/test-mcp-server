
resource "aws_cloudfront_vpc_origin" "alb_origin" {

  vpc_origin_endpoint_config {
    name                   = "cloudfront-alb-origin"
    arn                    = aws_lb.this.arn
    http_port              = 80
    https_port             = 443
    origin_protocol_policy = "http-only"

    origin_ssl_protocols {
      items    = ["TLSv1.2"]
      quantity = 1
    }
  }
}
