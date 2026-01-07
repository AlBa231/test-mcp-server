data "aws_ec2_managed_prefix_list" "cloudfront_ipv4" {
  name = "com.amazonaws.global.cloudfront.origin-facing"
}

data "aws_ec2_managed_prefix_list" "cloudfront_ipv6" {
  name = "com.amazonaws.global.ipv6.cloudfront.origin-facing"
}
