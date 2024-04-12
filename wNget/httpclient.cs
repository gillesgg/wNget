using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Http;
using Polly;
using Polly.Extensions.Http;


namespace wNget
{
    public static class ExceptionFormatterExtension
    {
        public static string GetDump(this Exception e)
        {
            var exceptions = new Stack<Exception>();
            while (e != null)
            {
                exceptions.Push(e);
                e = e.InnerException;
            }
            StringBuilder sb = new StringBuilder();
            while (exceptions.Count > 0)
            {
                e = exceptions.Pop();
                sb.AppendLine(e.GetType().Name + ": " + e.Message);
                sb.AppendLine(e.StackTrace);
                sb.AppendLine();
            }

            return sb.ToString().Trim();
        }
    }
    public class httpclient
    {
        Uri         _uri = null;
        HttpClient  _client = null;
        bool        _verbose = false;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        public httpclient(string url, int retrycount, int retryTimeout, bool verbose)
        {
            var policy = HttpPolicyExtensions.HandleTransientHttpError().CircuitBreakerAsync(retrycount, TimeSpan.FromSeconds(retryTimeout));
            var pollyHandler = new PolicyHttpMessageHandler(policy);
            pollyHandler.InnerHandler = new HttpClientHandler();
            _uri = new Uri(url);
            _client = new HttpClient(pollyHandler);
            _verbose = verbose;
        }
        public async Task<int> getAsync()
        {
            try
            {
             // Create an HttpRequestMessage for a GET request
             using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, _uri))
             {
                 // Send the request using SendAsync
                 HttpResponseMessage response = await _client.SendAsync(request);

                 if (response.IsSuccessStatusCode)
                 {
                     // Read the response content
                     string content = await response.Content.ReadAsStringAsync();
                     if (_verbose == true)
                     {
                        log.Info(content);
                     }
                     log.Info($"uri : {_uri.AbsoluteUri} Response : {response.StatusCode}");

                    }
                 else
                 {
                    log.Info($"Error: {response.StatusCode}");
                 }
             }
            }
            catch (System.Net.Http.HttpRequestException httpex)
            {
                log.Error(ExceptionFormatterExtension.GetDump(httpex));
                return httpex.HResult;
            }

            catch (Exception ex)
            {
                log.Error(ExceptionFormatterExtension.GetDump(ex));
                return ex.HResult;
            }
            return 0;
        }
    }
}
