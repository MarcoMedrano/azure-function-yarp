using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace proxy;

public static class Proxy
{
    private static readonly HttpClient _client;

    static Proxy()
    {
        var functionPath = new FileInfo(typeof(Proxy).Assembly.Location).Directory.Parent.FullName;
        Directory.SetCurrentDirectory(functionPath);
        var server = CreateServer(functionPath);
        _client = server.CreateClient();
    }

    private static TestServer CreateServer(string functionPath) =>
        new TestServer(WebHost
            .CreateDefaultBuilder()
            // .ConfigureAppConfiguration((builderContext, config) =>
            // {
            //     System.Console.WriteLine("ENV " + builderContext.HostingEnvironment.EnvironmentName);
            //     System.Console.WriteLine("PAth " + functionPath);
            //     config
            //         .SetBasePath(functionPath)
            //         .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            //         .AddJsonFile($"appsettings.{builderContext.HostingEnvironment.EnvironmentName}.json",
            //             optional: true, reloadOnChange: true)
            //         .AddEnvironmentVariables();
            // })
            .UseStartup<Startup>());

    [FunctionName("Proxy")]
    public static Task<HttpResponseMessage> Run([HttpTrigger(
                AuthorizationLevel.Anonymous,
                "get", "post", "put", "patch", "options",
                Route = "{*x:regex(^(?!admin|debug|monitoring).*$)}")] HttpRequestMessage req,
        TraceWriter log)
    {
        log.Info("***HTTP trigger - ASP.NET Core Proxy: function processed a request.");

        return _client.SendAsync(req);
    }
}