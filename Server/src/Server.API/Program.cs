using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Serilog;
using Server.Data;
using Server.Data.DbContexts;

namespace Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args)
                .EnsureDatabaseCreated<VideoPanzerDbContext>()
                .Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseSerilog((hostingContext, loggerConfiguration) => loggerConfiguration
                    .ReadFrom.Configuration(hostingContext.Configuration)
                    .Enrich.FromLogContext()
                    .WriteTo.Console()
                    .WriteTo.File(@"D:\home\LogFiles\http\RawLogs\log-{Date}.txt",
                                    fileSizeLimitBytes: 1_000_000,
                                    rollOnFileSizeLimit: true,
                                    shared: true,
                                    flushToDiskInterval: TimeSpan.FromSeconds(1)))
                .Build();
    }
}
