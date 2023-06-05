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

var connectionBuilder = new NpgsqlConnectionStringBuilder();
connectionBuilder.ConnectionString = builder.Configuration["ConnectionStrings:CandidatesManagementConnection"];
connectionBuilder.Username = builder.Configuration["UserID"];
connectionBuilder.Password = builder.Configuration["Password"];

builder.Services.AddLogging(loggingBuilder =>
{
    loggingBuilder.SetMinimumLevel(LogLevel.Debug);
    loggingBuilder.AddConsole();
    loggingBuilder.AddDebug();
});

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
//bool cmdLineInit = (app.Configuration["INITDB"] ?? "false") == "true";
app.UseSwagger();
app.UseSwaggerUI();
app.UseCors(m => m.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<CandidatesManagementDbContext>();

    // Apply any pending migrations
    dbContext.Database.Migrate();

    var initialDataSeeder = scope.ServiceProvider.GetRequiredService<InitialDataSeeder>();
    initialDataSeeder.SeedData();
}

if (app.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage();

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

//if (!cmdLineInit)
app.Run();