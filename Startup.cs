using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Sqlite;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using TeamAPI.Data;
using TeamAPI.Models;
using TeamAPI.Service;

namespace TeamAPI
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
            services.AddDbContext<DataContext>(opt => opt.UseSqlite(Configuration.GetConnectionString("DataContext")));
            services.AddDbContext<LibraryDbContext>(
                config => config.UseSqlite(Configuration.GetConnectionString("DefaultContext")));
            //optionBuilder => optionBuilder.MigrationAssembly(typeof(Startup).Assembly.GetName().Name)));

            //services.AddScoped<UserService, UserServiceImpl>();
            
            
            services.AddScoped<TeamService, TeamServiceImpl>();
            services.AddScoped<TeamUserService, TeamUserServiceImpl>();
            
            services.AddCors(c =>
            {
                c.AddPolicy("all", builder =>
                {
                    builder.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "TeamAPI", Version = "v1"}); 
                //c.IncludeXmlComments(AppDomain.CurrentDomain.BaseDirectory+"/TeamAPI.xml");
            });
            services.AddIdentity<IUser, IRole>(options => { options.Password.RequireDigit = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequiredLength = 6;
                })
                .AddEntityFrameworkStores<LibraryDbContext>();
            var tokenConfigSection = Configuration.GetSection("Security:Token");
            services.AddAuthentication(c =>
                {
                    
                    c.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    c.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(c =>
                {
                    c.SaveToken = true;
                    c.TokenValidationParameters = new TokenValidationParameters
                    {
                        
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        
                        ValidAudience = tokenConfigSection["Audience"],
                        ValidIssuer = tokenConfigSection["Issuer"],
                        IssuerSigningKey =
                            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenConfigSection["Key"])),
                        ClockSkew = TimeSpan.Zero
                    };
                
        });
            services.AddAuthorization(Options =>
            {
                Options.AddPolicy("Admin", policy => policy.RequireClaim("Admin"));
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                
            }
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TeamAPI v1"));
            //app.UseHttpsRedirection();
            app.UseCors("all");//跟上面的字符串一样
            app.UseRouting();

            app.UseAuthentication();
            
            app.UseAuthorization();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}