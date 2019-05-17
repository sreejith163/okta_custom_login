using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Http.Extensions;

namespace okta_custom_login
{
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
            services.AddHttpsRedirection(options =>
            {
                options.RedirectStatusCode = StatusCodes.Status308PermanentRedirect;
                options.HttpsPort = Convert.ToInt32(Configuration["HTTPSRedirection:HTTPS_Port"]);
            });

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.Configure<Helpers.OktaConfig>(Configuration.GetSection("CustomConfig"));

            // Add authentication services
            services.AddAuthentication(options => {
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = "Okta";
                options.DefaultChallengeScheme = "Okta";
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCookie()
            .AddOpenIdConnect("Okta", options => {

                // Configure the Auth0 Client ID and Client Secret
                options.ClientId = Configuration["CustomConfig:Okta_ClientId"];
                options.ClientSecret = Configuration["CustomConfig:Okta_ClientSecret"];

                options.CallbackPath = "/authorize/callback";

                options.Authority = $"{Configuration["CustomConfig:Okta_OrgUri"]}/oauth2/{Configuration["CustomConfig:Okta_AuthServer"]}";
                options.ResponseType = OpenIdConnectResponseType.Code;
                options.ResponseMode = "form_post";

                // To save the tokens to the Authentication Properties we need to set this to true
                options.SaveTokens = true;

                // Set scope to openid
                options.Scope.Clear();
                options.Scope.Add("openid");
                options.Scope.Add("profile");

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = "name",
                    ValidateIssuer = true
                };

                options.GetClaimsFromUserInfoEndpoint = true;

                options.Events = new OpenIdConnectEvents
                {
                    OnRedirectToIdentityProvider = con =>
                    {
                        if ((con.ProtocolMessage.RequestType == OpenIdConnectRequestType.Authentication) && (con.Request.Method.ToUpper() != "POST") && (con.Request.Query["isAuthenticated"].ToString().ToUpper() != "TRUE"))
                        {
                            string url = $"{con.Request.Scheme}://{con.Request.Host}/account/index?path={con.Request.GetEncodedUrl()}";
                            con.Response.Redirect(url);
                            con.HandleResponse();
                        }

                        con.ProtocolMessage.SetParameter("prompt", "none");

                        return Task.FromResult(0);
                    }
                };

            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
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
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseStatusCodePagesWithReExecute("/Error/{0}");

            app.UseHttpsRedirection();
            app.UseStaticFiles(new StaticFileOptions()
            {
                OnPrepareResponse = (context) =>
                {
                    // Disable caching for all static files.
                    context.Context.Response.Headers["Cache-Control"] = Configuration["StaticFiles:Headers:Cache-Control"];
                    context.Context.Response.Headers["Pragma"] = Configuration["StaticFiles:Headers:Pragma"];
                    context.Context.Response.Headers["Expires"] = Configuration["StaticFiles:Headers:Expires"];
                }
            });

            app.UseCookiePolicy();
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Account}/{action=Index}/{id?}");
            });
        }
    }
}
