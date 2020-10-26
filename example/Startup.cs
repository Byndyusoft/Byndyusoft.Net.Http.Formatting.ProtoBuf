namespace Byndyusoft.Net.Http.Formatting.ProtoBuf
{
    using Formaters;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;

    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddMvcCore(
                options =>
                {
                    options.EnableEndpointRouting = true;
                    options.OutputFormatters.Add(new ProtoBufOutputFormatter());
                    options.InputFormatters.Add(new ProtoBufInputFormatter());
                });
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseRouting();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}
