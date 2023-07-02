using RabbitMQ.Client;
using Sudoku.Scraper.Core.Common.Interfaces;
using Sudoku.Scraper.Core.DTO.NotifyMessages;

namespace Sudoku_Scraper.RabbitMQ.Senders
{
    public class NewPuzzleFoundSender : SenderBase, INotify<NewPuzzleFound>
    {

        public NewPuzzleFoundSender(IModel model) : base(model, string.Empty, Exchanges.NewPuzzle)
        {

        }

        public async ValueTask Send(NewPuzzleFound message)
        {
            var serializedMessage = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(message);

            await Send(serializedMessage);
        }
    }
}