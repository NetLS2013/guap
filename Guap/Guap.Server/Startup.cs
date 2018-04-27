using System.Threading.Tasks;
using Guap.Server.Data;
using Guap.Server.Data.Repositories;
using Guap.Server.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Guap.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IEmailSender, EmailSender>(provider => new EmailSender(Configuration));
            services.AddTransient<ITokenProvider, TokenProvider>();
            
            services.AddTransient<IUserRepository, UserRepository>();
            
            services.AddSingleton<INotification, Notification>();
            
            services.AddMvc();
            
            
            var notificationService = services.BuildServiceProvider()
                .GetService<INotification>();

            Task.Run(async () => await notificationService.WatchChain());
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}