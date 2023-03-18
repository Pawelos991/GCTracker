using GCTracker_Backend.Configure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options => 
    options.JsonSerializerOptions.PropertyNamingPolicy = null);

builder.Services.RegisterServices(builder.Configuration);

var app = builder.Build();
// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
});
app.UseAuthorization();

app.MapControllers();

app.Run();
