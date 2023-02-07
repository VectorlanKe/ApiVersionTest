using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace ApiVersionTest;

public class SwaggerConfigure : IConfigureOptions<SwaggerOptions>
{
    public void Configure(SwaggerOptions options)
    {
        options.PreSerializeFilters.Add((swagger, req) =>
        {
            //添加目标服务器地址
            swagger.Servers = new List<OpenApiServer>() { new() { Url = $"{req.Scheme}://{req.Host}" } };
        });
    }
}
public class SwaggerGenConfigure : IConfigureOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider _provider;
    public SwaggerGenConfigure(IApiVersionDescriptionProvider provider)
    {
        _provider = provider;
    }

    public void Configure(SwaggerGenOptions options)
    {
        foreach (ApiVersionDescription desc in _provider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(desc.GroupName, new OpenApiInfo
            {
                Title = $"Version Test API {desc.GroupName}",
                Version = desc.ApiVersion.ToString(),
            });
        }
        //添加token验证输入地方
        //string headerName = "Authorization";
        //options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        //{
        //    Description = $"JWT授权(数据将在请求头中进行传输) 直接在下框中输入{headerName}",
        //    Name = headerName, //jwt默认的参数名称
        //    In = ParameterLocation.Header, //jwt默认存放Authorization信息的位置(请求头中)
        //    Type = SecuritySchemeType.ApiKey
        //});
        //options.AddSecurityRequirement(new OpenApiSecurityRequirement
        //{
        //    {
        //        new OpenApiSecurityScheme
        //        {
        //            Reference = new OpenApiReference()
        //            {
        //                Id = "Bearer",
        //                Type = ReferenceType.SecurityScheme
        //            }
        //        },
        //        Array.Empty<string>()
        //    }
        //});
        //添加文档描述
        var docXml = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.xml");
        foreach (var doc in docXml)
            options.IncludeXmlComments(doc, true);
    }
}

public class SwaggerUiConfigure : IConfigureOptions<SwaggerUIOptions>
{
    private readonly IApiVersionDescriptionProvider _provider;
    public SwaggerUiConfigure(IApiVersionDescriptionProvider provider)
    {
        _provider = provider;
    }
    public void Configure(SwaggerUIOptions options)
    {
        foreach (ApiVersionDescription desc in _provider.ApiVersionDescriptions)
        {
            options.SwaggerEndpoint($"{desc.GroupName}/swagger.json", $"Version Test API {desc.GroupName}");
            //控制器展示方式(是否折叠)
            options.DocExpansion(DocExpansion.None);
            //模型展示深度
            //options.DefaultModelsExpandDepth(-1);

        }
    }
}