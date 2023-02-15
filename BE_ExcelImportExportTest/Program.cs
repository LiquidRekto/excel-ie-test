var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
#if DEBUG // If Debug Mode is on, enable Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
#endif
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

#if DEBUG // If Debug Mode is on, enable Swagger
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = string.Empty; // Base URl will be Swagger
    options.DocumentTitle = "Excel Demo API";
});
#endif
app.Run();
