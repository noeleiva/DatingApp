using API.Extensions;
using API.Middleware;
using Microsoft.OpenApi.Models;
using System.Security.Claims;


namespace API
{
  public class Startup
  {
    private readonly IConfiguration _config;
    public Startup(IConfiguration config)
    {
      _config = config;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddApplicationServices(_config);
      services.AddControllers();
      services.AddCors();
      services.AddIdentityServices(_config);

      services.AddSwaggerGen(c =>
      {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebAPIv5", Version = "v1" });
      });


    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {

      app.UseMiddleware<ExceptionMiddleware>();

      app.UseHttpsRedirection();

      // To route our end points to the different controllers
      app.UseRouting();

      app.UseCors(policy => policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200"));

      app.UseAuthentication();

      app.UseAuthorization();

      // Map the URL endpoints to the Controllers specific Method
      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
      });
    }
  }
}
