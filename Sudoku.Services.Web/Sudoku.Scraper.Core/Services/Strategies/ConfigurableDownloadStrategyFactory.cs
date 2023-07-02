using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Sudoku.Parser;
using Sudoku.Parser.Normalization;
using Sudoku.Parser.Readers;
using Sudoku.Parser.Web.Sudoku;
using Sudoku.Scraper.Core.Common.Interfaces;
using Sudoku.Scraper.Core.Common.Interfaces.Repositories;
using Sudoku.Scraper.Core.Configuration;
using Sudoku.Scraper.Core.DTO.NotifyMessages;
using Sudoku.Scraper.Core.Services.Readers;
using Sudoku.Scraper.Core.Services.Version;
using Sudoku.Serialization;
using static Sudoku.Parser.Utilities.UnorderedCellUtilities;

namespace Sudoku.Scraper.Core.Services.Strategies
{
    public class ConfigurableDownloadStrategyFactory : IDownloadStrategyFactory
    {
        private readonly ScrapeOptions _scrapeOptions;
        private readonly IServiceProvider _serviceProvider;
        private readonly ISerializeBoards<SudokuBoard> _serializer;

        private readonly INormalize _normalize = new HexaDecimalNormalizerWithSingleOffset();
        private readonly Boundary _boundary = new(9); //Expected Boundary from website.

        public ConfigurableDownloadStrategyFactory(IOptions<ScrapeOptions> scrapeOptions, IServiceProvider serviceProvider,
            ISerializeBoards<SudokuBoard> serializeBoards)
        {
            _scrapeOptions = scrapeOptions.Value;
            _serviceProvider = serviceProvider;
            _serializer = serializeBoards;
        }

        public Task<IStrategy> Create()
        {
            return Task.FromResult(CreateStrategyFromOption(_scrapeOptions));
        }

        private IStrategy CreateStrategyFromOption(ScrapeOptions option)
        {
            switch (option.EndpointType)
            {
                case EndpointType.Web1Sudoku:
                    return CreateSudokuStrategy(option.EndPoint);
                default:
                    throw new NotImplementedException();
            }
        }

        private IStrategy CreateSudokuStrategy(string url)
        {
            var boardNumber = _serviceProvider.GetRequiredService<IProvideBoardNumber>();
            var unitOfWork = _serviceProvider.GetRequiredService<IUnitOfWork>();
            var notifier = _serviceProvider.GetRequiredService<INotify<NewPuzzleFound>>();

            return new Web1SudokuStrategy(boardNumber, GetRetriever(), CreateReaderForStrategy(url), unitOfWork, notifier, _serializer);
        }

        private IReader CreateReaderForStrategy(string url)
        {
            var retriever = _serviceProvider.GetRequiredService<IRetrieveEndpointData>();
            return new HtmlReaderOnce(retriever, url);
        }

        private IRetrievePuzzle GetRetriever()
        {
            return new RetrievePartialWebFormattedPuzzle(_normalize, _boundary);
        }
    }
}
