using Microsoft.AspNetCore.Identity;
using Microsoft.Net.Http.Headers;
using Shop.API.ApiFilters;
using Shop.API.Configuration;
using Shop.Domain.Domain;
using Shop.Infrastructure.Configuration;
using Shop.Infrastructure.DbContexts;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddIdentity<User, IdentityRole<Guid>>()
            .AddEntityFrameworkStores<ShopDbContext>()
            .AddDefaultUI()
            .AddDefaultTokenProviders();

builder.Services.AddInfrastructureLayerConfiguration(builder.Configuration);
builder.Services.AddApiServiceLayerConfiguration(builder.Configuration);

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ApiExceptionFilter>();
}).AddNewtonsoftJson(opt =>
{
    opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
    opt.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.None;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerConfiguration();

builder.Services.AddCors(options => options.AddPolicy("ApiCorsPolicy", builder =>
{
    builder
        .AllowAnyOrigin()
        .WithHeaders(HeaderNames.AccessControlAllowHeaders, "Content-Type")
        .AllowAnyMethod();
}));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "ShopAPI");
    });
}

app.UseHttpsRedirection();

app.UseCors("ApiCorsPolicy");

app.UseAuthorization();

app.MapControllers();

app.Run();
