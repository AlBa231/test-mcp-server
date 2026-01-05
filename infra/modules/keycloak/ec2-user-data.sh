#!/bin/bash
set -eux

# Create swap only if it does not exist
if ! swapon --show | grep -q /swapfile; then
  fallocate -l 2G /swapfile
  chmod 600 /swapfile
  mkswap /swapfile
  swapon /swapfile
fi

# Persist swap across reboots
if ! grep -q "^/swapfile" /etc/fstab; then
  echo "/swapfile none swap sw 0 0" >> /etc/fstab
fi

apt update
apt install -y docker.io wget

wget https://s3.amazonaws.com/amazoncloudwatch-agent/ubuntu/amd64/latest/amazon-cloudwatch-agent.deb

dpkg -i amazon-cloudwatch-agent.deb

cat >/opt/aws/amazon-cloudwatch-agent/bin/config.json <<'EOF'
{
  "logs": {
    "logs_collected": {
      "files": {
        "collect_list": [
          {
            "file_path": "/var/log/cloud-init-output.log",
            "log_group_name": "/ec2/keycloak-init",
            "log_stream_name": "{instance_id}"
          },
          {
            "file_path": "/var/log/syslog",
            "log_group_name": "/ec2/keycloak-syslog",
            "log_stream_name": "{instance_id}"
          },
          {
            "file_path": "/var/lib/docker/containers/*/*-json.log",
            "log_group_name": "/ec2/keycloak-docker",
            "log_stream_name": "{instance_id}",
            "timezone": "UTC"
          }
        ]
      }
    }
  }
}
EOF

/opt/aws/amazon-cloudwatch-agent/bin/amazon-cloudwatch-agent-ctl \
    -a fetch-config \
    -m ec2 \
    -c file:/opt/aws/amazon-cloudwatch-agent/bin/config.json \
    -s

systemctl enable docker
systemctl start docker

docker run -d \
    --name keycloak \
    -p 8080:8080 \
    -e KC_BOOTSTRAP_ADMIN_USERNAME=${keycloak_admin_username} \
    -e KC_BOOTSTRAP_ADMIN_PASSWORD=${keycloak_admin_password} \
    -e KC_HTTP_ENABLED=true \
    -e KC_PROXY_HEADERS=xforwarded \
    -e KC_HOSTNAME=https://${cloudfront_domain}/auth \
    -e KC_HTTP_RELATIVE_PATH=/auth \
    -e KC_HOSTNAME_STRICT=true \
    -e JAVA_OPTS_APPEND="-Xms256m -Xmx768m" \
    keycloak/keycloak:26.4.7 \
    start \
    --http-host=0.0.0.0 \
    --http-port=8080