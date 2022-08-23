using MailKit.Net.Smtp;
using MimeKit;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace Ecarta
{
    public class Listener
    {
        static Listener? _listener;
        public static Listener Instance => _listener ?? throw new Exception("Listener not initialised");

        IConnection connection;
        readonly IModel channel;
        const string queueName = "ecarta";
        readonly EventingBasicConsumer consumer;
        readonly InitialisationParameters _options;

        private Listener(InitialisationParameters options)
        {
            _options = options;
            if (options.Rabbithost == string.Empty || options.RabbitUser == string.Empty || options.RabbitPass == string.Empty)
            {
                throw new Exception("The following connection details need to be filled in for RabbitMQ: rabbithost, rabbituser, rabbitpass");
            }

            var factory = new ConnectionFactory()
            {
                HostName = options.Rabbithost,
                UserName = options.RabbitPass,
                Password = options.RabbitUser
            };

            connection = factory.CreateConnection();
            channel = connection.CreateModel();

            var args = new Dictionary<string, object?>();
            args.Add("x-message-ttl", 3600000);

            channel.QueueDeclare(queueName, true, false, false, args);

            consumer = new EventingBasicConsumer(channel);
            consumer.Received += Consumer_Received;

            channel.BasicConsume(queueName, false, consumer);
        }

        private void Consumer_Received(object? sender, BasicDeliverEventArgs e)
        {
            string rawContent = Encoding.UTF8.GetString(e.Body.ToArray());

            Client.MailOptions? mailOptions = JsonSerializer.Deserialize<Client.MailOptions>(rawContent);
            if (mailOptions != null)
            {
                MimeMessage message = new MimeMessage();
                message.From.Add(new MailboxAddress(_options.FromName, _options.FromAddress));
                message.To.Add(new MailboxAddress("", mailOptions.Recipient));
                message.Subject = mailOptions.Subject;
                message.Body = new TextPart(MimeKit.Text.TextFormat.Plain)
                {
                    Text = mailOptions.Body
                };

                using (var client = new SmtpClient())
                {
                    client.Connect(_options.EmailHost, _options.EmailPort);
                    client.Authenticate(_options.EmailUser, _options.EmailPass);
                    client.Send(message);
                    client.Disconnect(true);
                }
            }

            channel.BasicAck(e.DeliveryTag, false);
        }

        public static void Initialise(InitialisationParameters options)
        {
            _listener = new Listener(options);
        }
    }
}
