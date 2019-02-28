using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using Steeltoe.CircuitBreaker.Hystrix;
using Steeltoe.CircuitBreaker.Hystrix.Strategy;

namespace Consumer
{
    class Program
    {
        static int Main(string[] args)
        {
            var config = BuildConfiguration();
            var mathService = CreateMathService();
            return Execute(mathService).Result;
        }

        private static IConfiguration BuildConfiguration()
        {
            var builder =
                new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json");

            return builder.Build();
        }

        private static MathService CreateMathService()
        {
            Console.WriteLine("Site port:");
            int.TryParse(Console.ReadLine(), out int port);

            return
                new MathService(
                    GenerateHystrixCommandOptions<MathService>("MathCommand", BuildConfiguration()),
                    port);
        }

        private static IHystrixCommandOptions GenerateHystrixCommandOptions<TService>(string groupKeyName, IConfiguration config)
        {
            var groupKey = HystrixCommandGroupKeyDefault.AsKey(groupKeyName);
            var strategy = HystrixPlugins.OptionsStrategy;
            var dynOpts = strategy.GetDynamicOptions(config);

            var threadPoolKey = HystrixThreadPoolKeyDefault.AsKey(groupKey.Name);
            var commandKey = HystrixCommandKeyDefault.AsKey(typeof(TService).Name);

            var opts = new HystrixCommandOptions(commandKey, null, dynOpts)
            {
                GroupKey = groupKey,
                ThreadPoolKey = threadPoolKey
            };
            opts.ThreadPoolOptions = new HystrixThreadPoolOptions(threadPoolKey, null, dynOpts);
            return opts;
        }

        static async Task<int> Execute(MathService mathService)
        {
            var shouldContinue = true;
            var r = new Random();

            do
            {
                Console.WriteLine("How many times:");
                int.TryParse(Console.ReadLine(), out int times);

                for (int i = 0; i < times; i++)
                {
                    var first = r.Next(0, 100);
                    var second = r.Next(0, 100);
                    var op = r.Next(0, 3);

                    var result = await mathService.Resolve(first, second, op);
                    Console.WriteLine($"Result of {result}");
                }

            } while (shouldContinue);

            return 0;
        }
    }
}
