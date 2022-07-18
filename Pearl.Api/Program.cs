using Pearl.Api;

Host.CreateDefaultBuilder()
    .ConfigureWebHostDefaults(builder => builder.UseStartup<Startup>())
    .Build().Run();