namespace Sudoku.Scraper.API.Configuration
{
    public class MessageBrokerConnectionOptions
    {
        public const string MessageBrokerConnection = "MessageBrokerConnection";

        public int Port { get; set; } = 3004;
        public string HostName { get; set; } = "192.168.1.16";
        public string UserName { get; set; } = "guest";
        public string Password { get; set; } = "guest";
    }
}
