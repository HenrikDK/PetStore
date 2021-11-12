using System;
using System.Diagnostics;
using System.Linq;
using Lamar.Microsoft.DependencyInjection;
using Microsoft.AspNetCore.Hosting;

namespace PetStore.Pet.Api
{
    public class Program
    {
        public static bool Debug = false;

        public static void Main(string[] args)
        {
            if (args.Contains("debug") || Debugger.IsAttached || Environment.GetEnvironmentVariable("debug") != null )
            {
                Debug = true;
            }
            
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseLamar()
                .UseStartup<Startup>()
                .UseUrls("http://*:8080")
                .Build();
            
            host.Run();
        }
    }
}
