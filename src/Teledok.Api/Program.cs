using Teledok.Api.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAspNetCoreConfigureServicesModule(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.ConfigureAspNetCoreModule();

app.Run();