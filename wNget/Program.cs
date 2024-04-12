using CommandLine;
using System;
using System.Collections.Generic;
using System.Threading;

namespace wNget
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var exitEvent = new ManualResetEvent(false);

            Console.CancelKeyPress += (sender, eventArgs) => {
                eventArgs.Cancel = true;
                exitEvent.Set();
                Environment.Exit(0);
            };

            CommandLine.Parser.Default.ParseArguments<Options>(args)
              .WithParsed(RunOptions)
              .WithNotParsed(HandleParseError);

            exitEvent.WaitOne();
            
        }
        static void RunOptions(Options opts)
        {
            if (opts.url != null)
            {
                
                if (opts.iteration == -1)
                {
                    Console.WriteLine("Press CTRL+C to exit");
                    while (true)
                    {
                        processQuery(opts);
                    }
                }
                else if (opts.iteration == 0)
                {
                    opts.iterationTime = 0;
                    processQuery(opts);
                }
                else
                {
                    Console.WriteLine("Press CTRL+C to exit");
                    for (var x = 0; x < opts.iteration; x++)
                    {
                        processQuery(opts);
                    }
                    Environment.Exit(0);
                }
            }

        }
        static void processQuery(Options opts)
        {
            httpclient httpclient = new httpclient(opts.url, opts.Retry, opts.RetryTimeOut, opts.Verbose);
            var response = httpclient.getAsync();
            if (opts.iterationTime > 0 )
                Thread.Sleep(opts.iterationTime);

            Environment.Exit(response.Result);
        }
        static void HandleParseError(IEnumerable<Error> errs)
        {
            Environment.Exit(0);
        }
    }
}
