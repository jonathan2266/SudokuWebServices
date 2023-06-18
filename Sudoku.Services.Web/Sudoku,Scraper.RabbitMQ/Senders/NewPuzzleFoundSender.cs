using RabbitMQ.Client;
using Sudoku.Scraper.Core.DTO.NotifyMessages;
using Sudoku.Scraper.Core.Services;

namespace Sudoku_Scraper.RabbitMQ.Senders
{
    public class NewPuzzleFoundSender : SenderBase, INotify<NewPuzzleFound>
    {

        public NewPuzzleFoundSender(IModel model) : base(model, string.Empty, "new_puzzle")
        {

        }

        public async ValueTask Send(NewPuzzleFound message)
        {
            var serializedMessage = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(message);
            await Send(serializedMessage);
        }
    }
}
