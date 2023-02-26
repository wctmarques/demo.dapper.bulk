using Demo.Dapper.Bulk.Models.Context;
using Demo.Dapper.Bulk.Models.Data;
using Demo.Dapper.Bulk.Models.Data.Interfaces;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Demo.Dapper.Bulk API",
        Description = "Demostração de inserção em massa.",
        Contact = new OpenApiContact
        {
            Name = "Github Repository",
            Url = new Uri("https://github.com/wctmarques/demo.dapper.bulk")
        }
    });
});

// DI
builder.Services.AddSingleton<DefaultContext>();
builder.Services.AddScoped<ISampleRepository, SampleRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
