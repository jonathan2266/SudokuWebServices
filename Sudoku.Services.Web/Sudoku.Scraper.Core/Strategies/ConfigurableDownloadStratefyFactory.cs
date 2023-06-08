using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Sudoku.Parser.Normalization;
using Sudoku.Parser.Readers;
using Sudoku.Parser.Web.Sudoku;
using Sudoku.Scraper.Core.Configuration;
using Sudoku.Scraper.Core.Version;
using System.Text;
using static Sudoku.Parser.Utilities.UnorderedCellUtilities;

namespace Sudoku.Scraper.Core.Strategies
{
    public class ConfigurableDownloadStratefyFactory : IDownloadStrategyFactory
    {
        private readonly ScrapeOptions _scrapeOptions;
        private readonly IServiceProvider _serviceProvider;

        private readonly INormalize _normalize = new HexaDecimalNormalizerWithSingleOffset();

        public ConfigurableDownloadStratefyFactory(IOptions<ScrapeOptions> scrapeOptions, IServiceProvider serviceProvider)
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
            var normalize = new HexaDecimalNormalizerWithSingleOffset(); //_serviceProvider.GetRequiredService<INormalize>();
            var clientfactory = _serviceProvider.GetRequiredService<IHttpClientFactory>();
            Boundary boundary = new(9);
            var retriever = new RetrievePartialWebFormattedPuzzle(normalize, boundary);

            var reader = new HtmlReaderOnce(clientfactory, url);
            //var puzzleFactory = _serviceProvider.GetRequiredService<IPuzzleRetrieverFactory>();
            return new Web1SudokuStrategy(boardNumber, retriever, reader);
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
