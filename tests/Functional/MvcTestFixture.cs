namespace System.Net.Http.Functional
{
    using System;
    using Formatting;
    using Headers;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Net;
    using Sockets;

    public abstract class MvcTestFixture : IDisposable
    {
        private IHost _host;
        private HttpClient _client;

        protected MvcTestFixture()
        {
            var url = $"http://localhost:{FreeTcpPort()}";
           _host =
                Host.CreateDefaultBuilder()
                    .ConfigureWebHostDefaults(webBuilder =>
                                              {
                                                  webBuilder.UseUrls(url);
                                                  webBuilder.ConfigureServices(ConfigureServices);
                                                  webBuilder.Configure(Configure);
                                              })
                    .Build();
            _host.Start();
            _client = new HttpClient { BaseAddress = new Uri(url) };
            _client.DefaultRequestHeaders.Accept.Add(ProtoBufMediaTypeHeaderValues.ApplicationProtoBuf);
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseRouting();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging(c => c.ClearProviders());
            services.AddControllers();
            services.AddMvcCore(ConfigureMvc);
        }

        protected abstract void ConfigureMvc(MvcOptions options);

        protected HttpClient Client => _client;

        public virtual void Dispose()
        {
            _host?.Dispose();
            _host = null;

            _client?.Dispose();
            _client = null;
        }

        private static int FreeTcpPort()
        {
            var listener = new TcpListener(IPAddress.Loopback, 0);
            listener.Start();
            int port = ((IPEndPoint) listener.LocalEndpoint).Port;
            listener.Stop();
            return port;
        }
    }
}