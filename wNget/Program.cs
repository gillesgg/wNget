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
                Console.WriteLine("Press CTRL+C to exit");
                if (opts.iteration == -1)
                {
                    while (true)
                    {
                        processQuery(opts);
                    }
                }
                else
                {
                    for (var x = 0; x < opts.iteration; x++)
                    {
                        processQuery(opts);
                    }
                }
            }

        }
        static void processQuery(Options opts)
        {
            httpclient httpclient = new httpclient(opts.url, opts.Retry, opts.RetryTimeOut, opts.Verbose);
            var response = httpclient.getAsync();
            Thread.Sleep(opts.iterationTime);
        }
        static void HandleParseError(IEnumerable<Error> errs)
        {
            //handle errors
        }
    }
}
