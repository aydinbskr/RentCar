using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.RateLimiting;
using RentCarServer.Application;
using RentCarServer.Application.Services;
using RentCarServer.Infrastructure;
using RentCarServer.WebAPI;
using RentCarServer.WebAPI.Controllers;
using RentCarServer.WebAPI.Middlewares;
using RentCarServer.WebAPI.Modules;
using Scalar.AspNetCore;
using System.Runtime.ConstrainedExecution;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddRateLimiter(config =>
{
    config.AddFixedWindowLimiter("fixed",opt =>
    {
        opt.PermitLimit = 100;
        opt.QueueLimit = 100;
        opt.Window = TimeSpan.FromSeconds(1);
        opt.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;
    });

    config.AddFixedWindowLimiter("login-fixed", opt =>
    {
        opt.PermitLimit = 5;
        opt.Window = TimeSpan.FromMinutes(1);
        opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
    });

    config.AddFixedWindowLimiter("forgot-password-fixed", opt =>
    {
        opt.PermitLimit = 2;
        opt.Window = TimeSpan.FromMinutes(5);
        opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
    });
    config.AddFixedWindowLimiter("reset-password-fixed", opt =>
    {
        opt.PermitLimit = 3;
        opt.Window = TimeSpan.FromMinutes(1);
        opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
    });
});

builder.Services
            .AddControllers()
            .AddOData(opt =>
            opt.Select()
                .Filter()
                .Count()
                .Expand()
                .OrderBy()
                .SetMaxTop(null)
                .AddRouteComponents("odata", MainODataController.GetEdmModel())
            );

builder.Services.AddCors();
builder.Services.AddOpenApi("v1", options => { options.AddDocumentTransformer<BearerSecuritySchemeTransformer>(); });
builder.Services.AddExceptionHandler<ExceptionHandler>().AddProblemDetails();
builder.Services.AddResponseCompression(opt =>
{
    opt.EnableForHttps = true;
});

builder.Services.AddTransient<CheckTokenMiddleware>();
builder.Services.AddHostedService<CheckLoginTokenService>();

var app = builder.Build();

app.MapOpenApi();
app.MapScalarApiReference();

app.UseHttpsRedirection();
app.UseCors(opt=> 
 opt.AllowAnyHeader()
 .AllowAnyOrigin()
 .AllowAnyMethod()
 .SetPreflightMaxAge(TimeSpan.FromMinutes(10)));

app.UseResponseCompression();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();

app.UseRateLimiter();
app.UseExceptionHandler();
app.UseMiddleware<CheckTokenMiddleware>();

app.MapControllers().RequireRateLimiting("fixed").RequireAuthorization();
app.MapAuthEndpoint();
app.MapBranch();
app.MapRole();
app.MapPermission();
app.MapUser();
app.MapCategory();
app.MapProtectionPackage();
app.MapExtra();
app.MapVehicle();

app.MapGet("/", () => "Hello World").RequireAuthorization();

await app.CleanRemovedPermissionsFromRoleAsync();
app.Run();
