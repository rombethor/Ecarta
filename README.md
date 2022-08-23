# Ecarta

A simple email messaging service.

One deployment per sender as defined SMTP server.

Receive email details via RabbitMQ.

Send details to the desired client.

## Deployment

For use with kubernetes refer to the yaml file [here](ecarta-deployment.yaml) for a deployment template.

Details of the required configuration as environment variables is below:

| Key | Description |
| --- | ----------- |
| rabbithost | The host address of the Rabbit MQ message queue |
| rabbituser | The username for the RabbitMQ service |
| rabbitpass | The password for the RabbitMQ service |
| fromname | The name to appear in the FROM header |
| fromaddress | The address which emails are sent from |
| emailhost | The host address of the SMTP server |
| emailport | An integer value of the SMTP port |
| emailuser | The username for the SMTP server |
| emailpass | The password for the SMTP server |

## Sending

Install package as: `Install-Package Ecarta.Client`

Then initialise by passing in the message queue configuration and queue emails with the `Send` method:

```c#
var ecarta = new Ecarta(rabbithost, rabbituser, rabbitpass);

ecarta.Send(new MailOptions(){
	Recipient = "ecarta@djt.mx",
	Subject = "Test email",
	Body = "This is a test notification"
});
```

