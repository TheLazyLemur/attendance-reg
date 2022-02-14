package main

import (
	"fmt"
	"github.com/aws/aws-lambda-go/events"
	"github.com/aws/aws-lambda-go/lambda"
	"net/http"
	"os"
	"strings"
)

func handler(request events.APIGatewayProxyRequest) (*events.APIGatewayProxyResponse, error) {

	body := request.Body

	url := "https://rapidprod-sendgrid-v1.p.rapidapi.com/mail/send"

	req, _ := http.NewRequest("POST", url, strings.NewReader(body))

	req.Header.Add("content-type", "application/json")
	req.Header.Add("x-rapidapi-host", os.Getenv("sendgrid_host"))
	req.Header.Add("x-rapidapi-key", os.Getenv("sendgrid_token"))

	res, _ := http.DefaultClient.Do(req)

	defer res.Body.Close()

	fmt.Println(res)
	fmt.Println(string(body))

	return &events.APIGatewayProxyResponse{
		StatusCode:        200,
		Headers:           map[string]string{"Content-Type": "text/plain"},
		MultiValueHeaders: http.Header{"Set-Cookie": {"Ding", "Ping"}},
		Body:              "ok",
		IsBase64Encoded:   false,
	}, nil
}

func main() {
	lambda.Start(handler)
}
