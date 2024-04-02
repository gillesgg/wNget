using CommandLine;

namespace wNget
{
    class Options
    {
        [Option('u', "Url", Required = true, HelpText = "URL to ping.")]
        public string url { get; set; }

        [Option('v', "Verbose", HelpText = "Print details during execution.", Default =false)]
        public bool Verbose { get; set; }

        [Option('r', "Retry", HelpText = "Number of retry.",Default =5)]
        public int Retry { get; set; }

        [Option('t', "RetryTime", HelpText = "Time in second between two retry.", Default = 30)]
        public int RetryTimeOut { get; set; }

        [Option('i', "Iteration", HelpText = "Number of iteration, if -1 iteration is infinite", Default = -1)]
        public int iteration { get; set; }

        [Option('m', "Time", HelpText = "Delay in ms between iteration", Default = 1000)]
        public int iterationTime { get; set; }

    }
}
