using Microsoft.EntityFrameworkCore;
using PlatformService.Data;
using PlatformService.SyncDataServices.Http;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Set up db context
if (builder.Environment.IsProduction())
{
    Console.WriteLine("Using SqlServer Db");
    builder.Services.AddDbContext<AppDbContext>(options =>
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("PlatformsConn"));
    });
}
else
{
    Console.WriteLine("Using InMem Db");
    builder.Services.AddDbContext<AppDbContext>(options => 
    {
        options.UseInMemoryDatabase("InMemory");
    });
}

builder.Services.AddScoped<IPlatformRepo, PlatformRepo>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddHttpClient<ICommandDataClient, HttpCommandDataClient>();

Console.WriteLine($"--> Command service endpoint: {builder.Configuration["CommandsService"]}");

var app = builder.Build();

PrepDb.PrepPopulation(app, builder.Environment.IsProduction());

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
