using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace MyNewWebApplication
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            //  HttpConfiguration config = new HttpConfiguration();
            Configuration = configuration;
            //    var cors = new EnableCorsAttribute("*", "*", "*");
            //    config.EnableCors(cors);
            //
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //            services
            // .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            // .AddJwtBearer(options =>
            // {    
            //     var serverSecret = new SymmetricSecurityKey(Encoding.UTF8.
            //GetBytes(Configuration["JWT:key"]));
            //     options.TokenValidationParameters = new
            //    TokenValidationParameters
            //    {   
            //         IssuerSigningKey = serverSecret,
            //         ValidIssuer = Configuration["JWT:Issuer"],
            //         ValidAudience = Configuration["JWT:Audience"]
            //     };
            // });

            //services.AddEntityFrameworkNpgsql().AddDbContext<MyWebApiContext>(opt => opt.UseNpgsql(Configuration.GetConnectionString("MyWebApiConnection")));

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                builder =>
                {
                    builder.WithOrigins("*")
                           .AllowAnyHeader()
                           .AllowAnyMethod();
                });
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseCors("AllowAll");


            app.UseHttpsRedirection();

            app.UseMvc();

        }
    }
}
