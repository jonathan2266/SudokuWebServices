using Sudoku.Scraper.Core.Common.Interfaces;

namespace Sudoku.Scraper.API.Services
{
    public class FastHttpClient : IRetrieveEndpointData
    {
        private readonly HttpClient _httpClient;

        public FastHttpClient(HttpClient client)
        {
                _httpClient = client;
        }

        public Task<HttpResponseMessage> Get(string endpoint)
        {
            return _httpClient.GetAsync(endpoint);
        }
    }
}
