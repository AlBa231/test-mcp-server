# ---- build stage ----
FROM golang:1.22-alpine AS build

WORKDIR /app
COPY main.go .

RUN go mod init lambda-default \
 && go mod tidy \
 && CGO_ENABLED=0 GOOS=linux GOARCH=amd64 go build -o main main.go

# ---- runtime stage ----
FROM public.ecr.aws/lambda/go:1

COPY --from=build /app/main /var/task/main

CMD ["main"]
