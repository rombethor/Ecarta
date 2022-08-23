namespace Ecarta
{
    public class InitialisationParameters
    {
        public InitialisationParameters(IConfiguration config)
        {
            Rabbithost = config["rabbithost"];
            RabbitUser = config["rabbituser"];
            RabbitPass = config["rabbitpass"];
            FromName = config["fromname"];
            FromAddress = config["fromaddress"];
            EmailHost = config["emailhost"];
            EmailPort = Convert.ToInt32(config["emailport"]);
            EmailUser = config["emailuser"];
            EmailPass = config["emailpass"];
        }

        public string? Rabbithost;
        public string? RabbitUser;
        public string? RabbitPass;
        public string? FromName;
        public string? FromAddress;
        public string? EmailHost;
        public int EmailPort;
        public string? EmailUser;
        public string? EmailPass;
    }
}
