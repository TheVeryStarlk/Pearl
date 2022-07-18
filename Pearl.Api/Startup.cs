using Microsoft.EntityFrameworkCore;
using Pearl.Api.Options;
using Pearl.Database;

namespace Pearl.Api;

public sealed class Startup
{
    private readonly IConfiguration configuration;

    public Startup(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();

        services.Configure<SecretsOptions>(configuration.GetSection(SecretsOptions.Secrets));

        services.AddDbContextPool<PearlContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString(nameof(PearlContext))));

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
    }

    public void Configure(IApplicationBuilder builder, IWebHostEnvironment environment)
    {
        if (environment.IsDevelopment())
        {
            builder.UseSwagger();
            builder.UseSwaggerUI();
        }

        builder.UseRouting();
        builder.UseHttpsRedirection();
        builder.UseAuthorization();

        builder.UseEndpoints(endpoints => endpoints.MapControllers());
    }
}