output "cloudfront_url" {
  value = "https://${module.cloudfront.cloudfront_domain_name}/test"
}