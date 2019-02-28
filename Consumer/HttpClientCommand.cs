using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Steeltoe.CircuitBreaker.Hystrix;

namespace Consumer
{
    public class HttpClientCommand<TResult> : HystrixCommand<TResult>
    {
        private readonly HttpClient _client;
        private readonly Func<Task<TResult>> _requester;
        private readonly Func<Task<TResult>> _fallbacker;

        public HttpClientCommand(
            IHystrixCommandOptions options,
            HttpClient client,
            Func<Task<TResult>> requester,
            Func<Task<TResult>> fallback) : base(options)
        {
            _client = client;
            _requester = requester;
            _fallbacker = fallback;
        }

        protected async override Task<TResult> RunAsync()
            => await _requester();

        protected async override Task<TResult> RunFallbackAsync()
        {
            Console.WriteLine($"IsCircuitBreakerOpen: {IsCircuitBreakerOpen}");
            return await _fallbacker();
        }
             
    }
}
