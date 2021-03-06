using Application;
using Contracts.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using WebApi.Middlewares;
using Persistence;
using Microsoft.AspNetCore.SpaServices.Extensions;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;

namespace WebApi {
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly("WebApi"))
            );

            services.AddTransient<IRoomsService, RoomsService>();
            services.AddTransient<ITeachersService, TeachersService>();
            services.AddTransient<IClassGroupsService, ClassGroupsService>();
            services.AddTransient<ISubjectsService, SubjectsService>();

            services.AddTransient<IScheduleService, ScheduleService>();
            services.AddTransient<IActivitiesService, ActivitiesService>();

            services.AddCors(o => o.AddPolicy("MyPolicy", builder => {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));

            services.AddControllers();
            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebApi", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApi v1"));
            }
            

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("MyPolicy");

            app.UseAuthorization();

            app.UseOptions();
            app.MapWhen(x => x.Request.Path.Value.StartsWith("/api"), builder => {
                app.UseEndpoints(endpoints => {
                    endpoints.MapControllers();
                });
            });
            app.MapWhen(x => !x.Request.Path.Value.StartsWith("/api"), builder => {
                app.UseSpa(spa => {
                    spa.Options.SourcePath = "../ClientApp/client-app";

                    if (env.IsDevelopment()) {
                        spa.UseReactDevelopmentServer(npmScript: "start");
                    }
                });
            });
           
        }
    }
}
