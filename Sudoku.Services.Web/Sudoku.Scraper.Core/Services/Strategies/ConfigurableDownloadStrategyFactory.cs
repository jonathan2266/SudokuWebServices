using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Sudoku.Parser;
using Sudoku.Parser.Normalization;
using Sudoku.Parser.Readers;
using Sudoku.Parser.Web.Sudoku;
using Sudoku.Scraper.Core.Configuration;
using Sudoku.Scraper.Core.DTO.NotifyMessages;
using Sudoku.Scraper.Core.Repositories;
using Sudoku.Scraper.Core.Services.Version;
using System.Text;
using static Sudoku.Parser.Utilities.UnorderedCellUtilities;

namespace Sudoku.Scraper.Core.Services.Strategies
{
    public class ConfigurableDownloadStrategyFactory : IDownloadStrategyFactory
    {
        private readonly ScrapeOptions _scrapeOptions;
        private readonly IServiceProvider _serviceProvider;

        private readonly INormalize _normalize = new HexaDecimalNormalizerWithSingleOffset();
        private readonly Boundary _boundary = new(9);

        public ConfigurableDownloadStrategyFactory(IOptions<ScrapeOptions> scrapeOptions, IServiceProvider serviceProvider)
        {
            _scrapeOptions = scrapeOptions.Value;
            _serviceProvider = serviceProvider;
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

            return new Web1SudokuStrategy(boardNumber, GetRetriever(), CreateReaderForStrategy(url), unitOfWork, notifier);
        }

        private IReader CreateReaderForStrategy(string url)
        {
            var clientfactory = _serviceProvider.GetRequiredService<IHttpClientFactory>();
            return new HtmlReaderOnce(clientfactory, url);
        }

        private IRetrievePuzzle GetRetriever()
        {
            return new RetrievePartialWebFormattedPuzzle(_normalize, _boundary);
        }
    }

    public class HtmlReaderOnce : IReader, IDisposable
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _url;
        private readonly SemaphoreSlim _semaphore = new(1);
        private string _urlContents = string.Empty;

        private readonly List<Stream> _createdStreams = new();

        public HtmlReaderOnce(IHttpClientFactory httpClientFactory, string url)
        {
            _httpClientFactory = httpClientFactory;
            _url = url;
        }

        public Encoding StreamEncoding => Encoding.UTF8;

        public async Task<Stream> GetStream()
        {
            try
            {
                await _semaphore.WaitAsync();
                await ReadFromUrlOnce();
                return CreateStreamFromContents();

            }
            finally
            {
                _semaphore.Release();
            }
        }

        public void Dispose()
        {
            foreach (var streamToDispose in _createdStreams)
            {
                streamToDispose?.Dispose();
            }
        }

        private async Task ReadFromUrlOnce()
        {
            if (!string.IsNullOrEmpty(_urlContents))
            {
                return;
            }

            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync(_url);
            response.EnsureSuccessStatusCode();

            _urlContents = await response.Content.ReadAsStringAsync();
        }

        private Stream CreateStreamFromContents()
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(_urlContents);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }
}
