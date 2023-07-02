using Sudoku.Parser.Readers;
using Sudoku.Scraper.Core.Common.Interfaces;
using System.Text;

namespace Sudoku.Scraper.Core.Services.Readers
{
    public class HtmlReaderOnce : IReader
    {
        private readonly IRetrieveEndpointData _retriever;
        private readonly string _url;
        private readonly SemaphoreSlim _semaphore = new(1);
        private string _urlContents = string.Empty;

        public HtmlReaderOnce(IRetrieveEndpointData retriever, string url)
        {
            _retriever = retriever;
            _url = url;
        }

        public Encoding StreamEncoding => Encoding.UTF8;

        public async Task<Stream> GetStream()
        {
            await ReadFromUrlOnce();
            return CreateStreamFromContents();
        }

        private async Task ReadFromUrlOnce()
        {
            if (!string.IsNullOrEmpty(_urlContents))
            {
                return;
            }

            try
            {
                await _semaphore.WaitAsync();

                if (!string.IsNullOrEmpty(_urlContents))
                {
                    return;
                }

                var response = await _retriever.Get(_url);
                response.EnsureSuccessStatusCode();

                _urlContents = await response.Content.ReadAsStringAsync();
            }
            finally
            {
                _semaphore.Release();
            }


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
