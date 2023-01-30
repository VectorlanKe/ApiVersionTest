using ApiVersionTest;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
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
    builder.Services.AddSingleton<IConfigureOptions<SwaggerGenOptions>, SwaggerConfigure>();
    builder.Services.AddSingleton<IConfigureOptions<SwaggerUIOptions>, SwaggerUiConfigure>();
    builder.Services.AddSwaggerGen();
}


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(options =>
    {
        //options.PreSerializeFilters.Add((swagger, req) =>
        //{
        //    //添加目标服务器地址
        //    swagger.Servers = new List<OpenApiServer>() { new OpenApiServer() { Url = $"{req.Scheme}://{req.Host}" } };
        //});
    });
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
