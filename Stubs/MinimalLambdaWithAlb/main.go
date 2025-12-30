package main

import (
  "github.com/aws/aws-lambda-go/events"
  "github.com/aws/aws-lambda-go/lambda"
)

func handler(req events.ALBTargetGroupRequest) (events.ALBTargetGroupResponse, error) {
  return events.ALBTargetGroupResponse{
    StatusCode: 200,
    Body:       "OK",
    Headers: map[string]string{
      "content-type": "text/plain",
    },
  }, nil
}

func main() {
  lambda.Start(handler)
}
