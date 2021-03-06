package main

import (
	"crypto/sha1"
	"encoding/base64"
	"fmt"
	"github.com/aws/aws-lambda-go/events"
	"github.com/aws/aws-lambda-go/lambda"
	"net/http"
	"os"
)

func GenerateSha256(bv []byte) string {
	hasher := sha1.New()
	hasher.Write(bv)
	sha := base64.URLEncoding.EncodeToString(hasher.Sum(nil))
	return sha
}

func handler(request events.APIGatewayProxyRequest) (*events.APIGatewayProxyResponse, error) {

	params := request.QueryStringParameters

	credentials := ""
	if params["credentials"] != "" {
		credentials = params["credentials"]
	} else {
		return &events.APIGatewayProxyResponse{
			StatusCode:        401,
			Headers:           map[string]string{"Content-Type": "text/plain"},
			MultiValueHeaders: http.Header{"Set-Cookie": {"Ding", "Ping"}},
			Body:              "Unauthorized",
			IsBase64Encoded:   false,
		}, nil
	}

	expected_hash := os.Getenv("password_hash")

	if GenerateSha256([]byte(credentials)) != expected_hash {
		return &events.APIGatewayProxyResponse{
			StatusCode:        401,
			Headers:           map[string]string{"Content-Type": "text/plain"},
			MultiValueHeaders: http.Header{"Set-Cookie": {"Ding", "Ping"}},
			Body:              "Unauthorized",
			IsBase64Encoded:   false,
		}, nil
	}

	token := os.Getenv("supabase_token")

	payload := fmt.Sprintf("{\"token\":\"%s\"}", token)

	return &events.APIGatewayProxyResponse{
		StatusCode:        200,
		Headers:           map[string]string{"Content-Type": "text/plain"},
		MultiValueHeaders: http.Header{"Set-Cookie": {"Ding", "Ping"}},
		Body:              payload,
		IsBase64Encoded:   false,
	}, nil
}

func main() {
	// Make the handler available for Remote Procedure Call
	lambda.Start(handler)
}
