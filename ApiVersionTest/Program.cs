using ApiVersionTest;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
//添加接口版本信息
builder.Services.AddApiVersioning(options =>
{
    options.ReportApiVersions = true;
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
//可配置header
//options.ApiVersionReader = ApiVersionReader.Combine(new UrlSegmentApiVersionReader(),
//    new HeaderApiVersionReader("X-Accept-version"),
//    new MediaTypeApiVersionReader("X-Accept-version"));
});
builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl= true;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
if (builder.Environment.IsDevelopment())
{
    //添加swagger配置信息
    builder.Services.AddSingleton<IConfigureOptions<SwaggerGenOptions>, SwaggerGenConfigure>();
    builder.Services.AddSingleton<IConfigureOptions<SwaggerOptions>, SwaggerConfigure>();
    builder.Services.AddSingleton<IConfigureOptions<SwaggerUIOptions>, SwaggerUiConfigure>();
    builder.Services.AddSwaggerGen();
}


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
