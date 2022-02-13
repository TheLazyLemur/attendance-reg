package main

import (
	"fmt"
	"github.com/aws/aws-lambda-go/events"
	"github.com/aws/aws-lambda-go/lambda"
	"net/http"
	"net/smtp"
	"os"
)

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

	if credentials != "daniel" {
		return &events.APIGatewayProxyResponse{
			StatusCode:        401,
			Headers:           map[string]string{"Content-Type": "text/plain"},
			MultiValueHeaders: http.Header{"Set-Cookie": {"Ding", "Ping"}},
			Body:              "Unauthorized",
			IsBase64Encoded:   false,
		}, nil
	}

	from := "from@gmail.com"
	password := "<Email Password>"

	to := []string{
		"sender@example.com",
	}

	smtpHost := "smtp.gmail.com"
	smtpPort := "587"

	message := []byte("This is a test email message.")

	auth := smtp.PlainAuth("", from, password, smtpHost)

	err := smtp.SendMail(smtpHost+":"+smtpPort, auth, from, to, message)
	if err != nil {
		fmt.Println(err)
	}
	fmt.Println("Email Sent Successfully!")

	return &events.APIGatewayProxyResponse{
		StatusCode:        200,
		Headers:           map[string]string{"Content-Type": "text/plain"},
		MultiValueHeaders: http.Header{"Set-Cookie": {"Ding", "Ping"}},
		Body:              "Sent email",
		IsBase64Encoded:   false,
	}, nil
}

func main() {
	// Make the handler available for Remote Procedure Call
	lambda.Start(handler)
}
