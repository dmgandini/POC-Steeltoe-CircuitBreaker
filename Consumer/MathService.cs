using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Steeltoe.CircuitBreaker.Hystrix;

namespace Consumer
{
    public class MathService
    {
        private readonly HttpClient _client;
        private readonly IHystrixCommandOptions _options;

        public MathService(IHystrixCommandOptions options, int port)
        {
            _client = new HttpClient()
            {
                BaseAddress = new Uri($"http://localhost:{port}/"),
                Timeout = new TimeSpan(0, 0, 0, 0, 100),
            };
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _options = options;
        }

        public async Task<string> Resolve(decimal firstTerm, decimal secondTerm, int operation)
        {
            var command = new HttpClientCommand<string>(
                _options,
                _client,
                async () =>
                  {
                      var response = await _client.GetAsync($"api/math/{firstTerm}/{secondTerm}/{operation}/");
                      if (response.IsSuccessStatusCode)
                      {
                          return await response.Content.ReadAsStringAsync();
                      }
                      else
                      {
                          Console.WriteLine("Error");
                          throw new Exception(await response.Content.ReadAsStringAsync());
                      }
                          
                  },
                () => Task.FromResult("Fallback"));

            return await command.ExecuteAsync();
        }
    }
}
