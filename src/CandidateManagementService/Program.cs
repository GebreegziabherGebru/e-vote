using CandidateManagementService.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;
using CandidateManagementService.Repository;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<MvcNewtonsoftJsonOptions>(opts =>
{
    opts.SerializerSettings.NullValueHandling
    = Newtonsoft.Json.NullValueHandling.Ignore;
});

// builder.Services.Configure<JsonOptions>(opts =>
// {
//     opts.JsonSerializerOptions.DefaultIgnoreCondition
//     = JsonIgnoreCondition.WhenWritingNull;
// });

var connectionBuilder = new NpgsqlConnectionStringBuilder();
connectionBuilder.ConnectionString = builder.Configuration["ConnectionStrings:CandidatesManagementConnection"];
connectionBuilder.Username = builder.Configuration["UserID"];
connectionBuilder.Password = builder.Configuration["Password"];

builder.Services.AddDbContext<CandidatesManagementDbContext>(opts =>
{
    opts.UseNpgsql(connectionBuilder.ConnectionString);
    opts.EnableSensitiveDataLogging(true);
});

builder.Services.AddCors();

builder.Services.AddTransient<InitialDataSeeder>();
builder.Services.AddScoped<ICandidateRepository, CandidateRepository>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

//Change the default asp.net logger. Serilog is structured!
builder.Host.UseSerilog((context, config) =>
{
    config.WriteTo.Console();
});

var app = builder.Build();

bool cmdLineInit = (app.Configuration["INITDB"] ?? "false") == "true";
if (app.Environment.IsDevelopment() || cmdLineInit)
{
    app.UseSwagger();
    app.UseSwaggerUI();

    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<CandidatesManagementDbContext>();

        // Apply any pending migrations
        dbContext.Database.Migrate();

        var initialDataSeeder = scope.ServiceProvider.GetRequiredService<InitialDataSeeder>();
        initialDataSeeder.SeedData();
    }
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

if (!cmdLineInit)
    app.Run();