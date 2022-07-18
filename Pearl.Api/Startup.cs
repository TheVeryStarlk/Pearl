using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Pearl.Api.Options;
using Pearl.Api.Services;
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

        services.AddTransient(_ => new TokenValidationParameters()
        {
            IssuerSigningKey =
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(configuration.GetSection($"{SecretsOptions.Secrets}:Key").Value)),
            ValidAlgorithms = new[] { SecurityAlgorithms.HmacSha256 },
            ValidateIssuerSigningKey = true,
            ClockSkew = TimeSpan.Zero,
            ValidateAudience = false,
            ValidateIssuer = false
        });

        services.Configure<SecretsOptions>(configuration.GetSection(SecretsOptions.Secrets));

        services.AddTransient<AccessTokenService>();

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