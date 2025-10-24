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

//threads services 
builder.Services.AddTransient<IDropInThreadService, DropInThreadService>();
 builder.Services.AddScoped<IDropInThreadProvider, DropInThreadProvider>();
 builder.Services.AddScoped<IDropInThreadRepository, DropInThreadRepository>();

 builder.Services.AddScoped<IDropInThreadCommentProvider, DropInThreadCommentProvider>();
 builder.Services.AddScoped<IDropInThreadCommentRepository, DropInThreadCommentRepository>();

 builder.Services.AddScoped<IDropInThreadLikeProvider, DropInThreadLikeProvider>();
 builder.Services.AddScoped<IDropInThreadLikeRepository, DropInThreadLikeRepository>();

 builder.Services.AddTransient<IAuthService, AuthService>();
 builder.Services.AddTransient<ITokenService, TokenService>();

 builder.Services.AddScoped<IAppUserProvider, AppUserProvider>();
 builder.Services.AddScoped<IAppUserRepository, AppUserRepository>();
 builder.Services.AddHttpClient();
 var redisHost = builder.Configuration.GetSection("Redis:host").Value;
 var redisPort = int.Parse(builder.Configuration["Redis:port"]);
 var redisUser = builder.Configuration.GetSection("Redis:user").Value;
 var redisPassword = builder.Configuration.GetSection("Redis:password").Value;

builder.Services.AddSignalR();
// Register your RedisProvider as a singleton
builder.Services.AddSingleton(new RedisProvider(redisHost, redisPort, redisUser, redisPassword));

// Register Repository and Service
 builder.Services.AddScoped<MessagingRepository>();
 builder.Services.AddScoped<MessagingService>();
 

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost",
        policy =>
        {
            policy.WithOrigins("http://localhost:3000","http://192.168.0.155:3000") // Replace with your React app's URL
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials(); 

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

    // 👇 This is required for SignalR token negotiation
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Query["access_token"];
            var path = context.HttpContext.Request.Path;

            // If the request is for the hub, grab token from query string
            if (!string.IsNullOrEmpty(accessToken) &&
                path.StartsWithSegments("/chathub"))
            {
                context.Token = accessToken;
            }

            return Task.CompletedTask;
        }
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
app.MapHub<ChatHub>("/chatHub");

app.MapControllers();

app.Run();
