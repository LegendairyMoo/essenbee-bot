using Essenbee.Bot.Core.Interfaces;
using Essenbee.Bot.Infra.CognitiveServices;
using Essenbee.Bot.Infra.Discord;
using Essenbee.Bot.Infra.Hangfire;
using Essenbee.Bot.Infra.Slack;
using Essenbee.Bot.Infra.Twitch;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Essenbee.Bot.Web
{
    //`
    //` ![](BECF8D6358A570AF7A626A1C8EC9D254.png;;;0.00917,0.00839)

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => false;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.Configure<UserSecrets>(Configuration.GetSection(nameof(UserSecrets)));
            var secrets = Configuration.GetSection(nameof(UserSecrets)).Get<UserSecrets>();

            // Injected into ChatClients by DI for Hangfire scheduled actions
            var slackConfig = new SlackConfig { ApiKey = secrets.SlackApiKey };
            var discordConfig = new DiscordConfig { DiscordToken = secrets.DiscordToken };
            var twitchConfig = new TwitchConfig {
                Username = secrets.TwitchUsername,
                Token = secrets.TwitchToken,
                Channel = secrets.TwitchChannel,
            };

            services.AddSingleton(slackConfig);
            services.AddSingleton(discordConfig);
            services.AddSingleton(twitchConfig);

            services.AddSingleton<IConnectedClients, ConnectedClients>();
            services.AddSingleton<IBotSettings, BotSettings>();

            services.AddHangfire(x => x.UseSqlServerStorage(secrets.DatabaseConnectionString));

            services.AddDbContext<AppDataContext>(options =>
                options.UseSqlServer(secrets.DatabaseConnectionString));

            services.AddScoped<IRepository, EntityFrameworkRepository>();
            services.AddSingleton<IActionScheduler, HangfireActionScheduler>();
            services.AddSingleton<IAnswerSearchEngine, AnswerSearch>();
            services.AddSingleton<IBot, Core.Bot>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseXContentTypeOptions();
            app.UseReferrerPolicy(options => options.NoReferrer());
            app.UseXXssProtection(options => options.EnabledWithBlockMode());
            app.UseXfo(options => options.Deny());

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseHangfireDashboard();
            app.UseHangfireServer();

            app.UseMvc();
        }
    }
}
