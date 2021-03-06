﻿using IdentityServer4;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace IdentityServerApp
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            //**/ TODO for production purposes use secure persistance storage and cryptography
            // https://identityserver4.readthedocs.io/en/release/quickstarts/8_entity_framework.html#refentityframeworkquickstart
            // https://identityserver4.readthedocs.io/en/release/topics/crypto.html#refcrypto

            // configure identity server with in-memory stores, keys, clients and scopes
            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddInMemoryIdentityResources(Config.GetIdentityResources())
                .AddInMemoryApiResources(Config.GetApiResources())
                .AddInMemoryClients(Config.GetClients())
                .AddTestUsers(Config.GetUsers());

            var googleDataJson = System.IO.File.ReadAllText(@".\SensitiveData.json");
            var googleData = JsonConvert.DeserializeObject<SensitiveData>(googleDataJson);

            //**/ TODO Note: for Google authentication you need to register your local quickstart identityserver using the Google developer console. 
            // https://console.developers.google.com/
            // As a redirect URL, use the URL of your local identityserver and add /signin-google. 
            // If your IdentityServer is running on port 5000 - you can use the above client id/secret which is pre-registered.
            services.AddAuthentication()
              .AddGoogle("Google", options =>
              {
                  options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
                  //**/ TODO do not commit specific data !!!
                  // How to activate google account https://identityserver4.readthedocs.io/en/release/quickstarts/4_external_authentication.html
                  options.ClientId = googleData.GoogleClientId;
                  options.ClientSecret = googleData.GoogleClientSecret;
              });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseIdentityServer();

            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();
        }
    }
}
