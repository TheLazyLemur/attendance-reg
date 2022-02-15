package main

import (
	"log"
	"net/http"
	"os"
	"strings"

	"github.com/aws/aws-lambda-go/events"
	"github.com/aws/aws-lambda-go/lambda"
)

func handler(request events.APIGatewayProxyRequest) (*events.APIGatewayProxyResponse, error) {
	if request.HTTPMethod != "POST" {
		return &events.APIGatewayProxyResponse{
			StatusCode: 405,
			Headers:    map[string]string{"Content-Type": "text/plain"},
			Body:       "Not Allowed",
		}, nil
	}

	body := request.Body
	url := "https://rapidprod-sendgrid-v1.p.rapidapi.com/mail/send"

	email_request, err := http.NewRequest("POST", url, strings.NewReader(body))
	if err != nil {
		log.Fatal(err)
		return nil, err
	}

	email_request.Header.Add("content-type", "application/json")
	email_request.Header.Add("x-rapidapi-host", os.Getenv("sendgrid_host"))
	email_request.Header.Add("x-rapidapi-key", os.Getenv("sendgrid_token"))

	result, err := http.DefaultClient.Do(email_request)
	if err != nil {
		log.Fatal(err)
		return nil, err
	}

	defer result.Body.Close()

	return &events.APIGatewayProxyResponse{
		StatusCode: 200,
		Headers:    map[string]string{"Content-Type": "text/plain"},
		Body:       "ok",
	}, nil
}

func main() {
	lambda.Start(handler)
}
