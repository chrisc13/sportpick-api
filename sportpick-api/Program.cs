using sportpick_dal;
using sportpick_bll;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//fut services

builder.Services.AddSingleton<IDatabaseProvider, DatabaseProvider>();
 //drop in services
 builder.Services.AddScoped<IDropEventService, DropEventService>();
 builder.Services.AddScoped<IDropEventProvider, DropEventProvider>();
 builder.Services.AddScoped<IDropEventRepository, DropEventRepository>();

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

//app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors("AllowLocalhost");

app.MapControllers();

app.Run();
