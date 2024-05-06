using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Rent.ADO.NET;
using Rent.BLL;
using Rent.MVC.Filters;
using Rent.MVC.Middlewares;
using Serilog;
using System.Text;
using WebMarkupMin.AspNetCore5;
using WebMarkupMin.Core;


var builder = WebApplication.CreateBuilder(args);

IConfiguration config = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .Build();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(config)
    .CreateLogger();

builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.TokenValidationParameters = new TokenValidationParameters
    {
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:Secret"]!)),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});
builder.Services.AddAuthorization();

builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add(typeof(CustomExceptionFilter));
});

builder.Services.AddRazorPages()
    .AddJsonOptions(options => 
        options.JsonSerializerOptions.PropertyNamingPolicy = null).AddRazorRuntimeCompilation();

builder.Services.AddResponseCaching();
builder.Services.AddMemoryCache();

builder.Services.BllServiceInject();
builder.Services.AdoNetServiceInject();

builder.Services.AddHttpContextAccessor();

builder.Services.AddMvc();
builder.Services.AddWebMarkupMin(
        options =>
        {
            options.AllowMinificationInDevelopmentEnvironment = true;
            options.AllowCompressionInDevelopmentEnvironment = true;
        })
    .AddHtmlMinification(
        options =>
        {
            HtmlMinificationSettings settings = options.MinificationSettings;
            settings.RemoveRedundantAttributes = true;
            settings.RemoveHttpProtocolFromAttributes = true;
            settings.RemoveHttpsProtocolFromAttributes = true;
        });

builder.Services.AddCors(option => option.AddPolicy("Policy",
    policyBuilder =>
    {
        policyBuilder.AllowAnyHeader();
        policyBuilder.AllowAnyMethod();
    }));

builder.Host.UseSerilog(Log.Logger);

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseMiddleware<JwtMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.UseSerilogRequestLogging();

app.UseAuthorization();

app.UseStaticFiles();

//app.Use(async (context, next) => {
//    string path = context.Request.Path;
//    if (path.EndsWith(".css") || path.EndsWith(".js"))
//    {
//        TimeSpan maxAge = new TimeSpan(7, 0, 0, 0); context.Response.Headers.Append("Cache-Control", "max-age=" + maxAge.TotalSeconds.ToString("0"));
//    }
//    else
//    {
//        context.Response.Headers.Append("Cache-Control", "no-cache");
//        context.Response.Headers.Append("Cache-Control", "private, no-store");
//    }
//    await next();
//});


app.UseResponseCaching();

app.UseWebMarkupMin();

app.UseRouting();

app.MapRazorPages();

app.MapDefaultControllerRoute();

app.Run();
