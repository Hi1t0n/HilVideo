using AuthService.Infrastructure;
using Microsoft.AspNetCore.CookiePolicy;
using UserService.Infrastructure.Extensions;
using UserSevice.Host.Routing;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(nameof(JwtOptions)));
builder.Services.AddBusinessLogic(builder.Configuration, connectionString);
const string allowCorsPolicy = "allowCorsPolicy";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: allowCorsPolicy, policyBuilder =>
    {
        policyBuilder.WithOrigins("https://localhost:3000");
        policyBuilder.AllowAnyHeader();
        policyBuilder.AllowAnyMethod();
        policyBuilder.AllowCredentials();
    } );
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors(allowCorsPolicy);

app.AddUserRouter();
app.AddAuthRouting();
app.AddMovieRouting(); 
app.AddGenreRouting();
app.AddDirectorRouting();
app.AddMovieTypeRouting();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });
}

app.UseCookiePolicy(new CookiePolicyOptions
{
    HttpOnly = HttpOnlyPolicy.Always,
    MinimumSameSitePolicy = SameSiteMode.Strict,
    Secure = CookieSecurePolicy.Always,
    
});

app.UseAuthentication();
app.UseAuthorization();

app.Run();