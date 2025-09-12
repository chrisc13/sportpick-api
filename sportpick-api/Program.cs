using sportpick_dal;
using sportpick_bll;
using sportpick_api.DTO;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IDatabaseProvider, DatabaseProvider>();
 //drop in services
 builder.Services.AddTransient<IDropEventService, DropEventService>();
 builder.Services.AddScoped<IDropEventProvider, DropEventProvider>();
 builder.Services.AddScoped<IDropEventRepository, DropEventRepository>();
 //profile services
builder.Services.AddTransient<IProfileService, ProfileService>();
 builder.Services.AddScoped<IProfileProvider, ProfileProvider>();
 builder.Services.AddScoped<IProfileRepository, ProfileRepository>();

 builder.Services.AddTransient<IAuthService, AuthService>();
 builder.Services.AddTransient<ITokenService, TokenService>();

 builder.Services.AddScoped<IAppUserProvider, AppUserProvider>();
 builder.Services.AddScoped<IAppUserRepository, AppUserRepository>();
 builder.Services.AddHttpClient();


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost",
        policy =>
        {
            policy.WithOrigins("http://localhost:3000") // Replace with your React app's URL
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});
var jwtSettings = builder.Configuration.GetSection("Jwt");
 builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["TokenSecret"]))
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
// Redirect HTTP → HTTPS only in production
if (app.Environment.IsProduction())
{
    app.UseHttpsRedirection();
}
app.UseCors("AllowLocalhost");

app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.Run();
