using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Ecarta.Client
{
    /// <summary>
    /// Handles messages to be sent via email.
    /// </summary>
    public class Ecarta
    {

        readonly IConnection connection;
        readonly IModel channel;

        /// <summary>
        /// Initialise a new Ecarta email messenger instance
        /// </summary>
        /// <param name="host">RabbitMQ hostname</param>
        /// <param name="user">RabbitMQ username</param>
        /// <param name="password">RabbitMQ password</param>
        public Ecarta(string host, string user, string password)
        {
            var factory = new ConnectionFactory()
            {
                HostName = host,
                UserName = user,
                Password = password
            };
            connection = factory.CreateConnection();
            channel = connection.CreateModel();
        }

        /// <summary>
        /// Send an email via the message queue
        /// </summary>
        /// <param name="options">Email detail</param>
        public void Send(MailOptions options)
        {
            var data = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(options));

            channel.BasicPublish("", "ecarta", null, data);
        }
    }
}