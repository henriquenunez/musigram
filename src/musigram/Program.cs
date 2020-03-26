using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;



using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using musigram.Settings;
using System.IO;
using musigram.Bots;

namespace musigram
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var services = new ServiceCollection();
            ConfigureServices(services);
            var serviceProvider = services.BuildServiceProvider();
            
            TelegramBot _boto = serviceProvider.GetService<TelegramBot>();

            // create service collection

            // create service provider

            // entry to run app
            //await serviceProvider.GetService<App>().Run();

            Thread web_app = new Thread(new ThreadStart (CreateHostBuilder(args).Build().Run));
            Thread boto_thread = new Thread(new ThreadStart(_boto.StartBot));
            web_app.Start();
            boto_thread.Start();
            System.Diagnostics.Debug.WriteLine("Bot started!");
        }


        private static void ConfigureServices(IServiceCollection services)
        {
            // build config
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false)
                .AddEnvironmentVariables()
                .Build();

            services.AddOptions();

            SpotifyConfiguration _spotifyOptions;

            /*Configuring my clients to work*/
            services.Configure<TelegramConfiguration>(_telegramOptions =>
                                configuration.GetSection("telegram").Bind(_telegramOptions));
            _spotifyOptions = configuration.GetSection("spotify").Get<SpotifyConfiguration>();

            Spotify.AuthenticationHandler.clientId = _spotifyOptions.clientID;
            Spotify.AuthenticationHandler.secretId = _spotifyOptions.secretID;

            // add app
            services.AddSingleton<TelegramBot>();
        }

        //Web stuff
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<StartupWeb>();
                });
    }
}

/*
internal class Program
{
    public static async Task Main(string[] args)
    {
        // create service collection
        var services = new ServiceCollection();
        ConfigureServices(services);

        // create service provider
        var serviceProvider = services.BuildServiceProvider();

        // entry to run app
        await serviceProvider.GetService<App>().Run();
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        // configure logging
        services.AddLogging(builder =>
            builder
                .AddDebug()
                .AddConsole()
        );

        // build config
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", false)
            .AddEnvironmentVariables()
            .Build();

        services.AddOptions();
        services.Configure<AppSettings>(configuration.GetSection("App"));

        // add services:
        // services.AddTransient<IMyRespository, MyConcreteRepository>();

        // add app
        services.AddTransient<App>();
    }
}*/
