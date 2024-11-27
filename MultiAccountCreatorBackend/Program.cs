using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using MultiAccountCreatorBackend.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add services to the container.
builder.Services.AddDbContext<AccountsDataContext>(options =>
    options.UseMySQL(
        builder.Configuration.GetConnectionString("DevConnection"),
        mysqlOptions =>
        {
            // Configure retry logic for transient faults
            mysqlOptions.EnableRetryOnFailure(
                maxRetryCount: 5, // Number of retry attempts
                maxRetryDelay: TimeSpan.FromSeconds(5), // Maximum delay between retries
                errorNumbersToAdd: [1042, 1045] // Add specific MySQL error codes for retries
            );
        })
        .EnableDetailedErrors() // Provides detailed EF Core error messages
        .LogTo(Console.WriteLine, LogLevel.Error) // Logs EF Core events and SQL queries
);

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.MapOpenApi();
}
app.UseSwagger();
app.UseSwaggerUI(options => // UseSwaggerUI is called only in Development.
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = string.Empty;
});
app.MapSwagger();

//app.UseHttpsRedirection();

//app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AccountsDataContext>();
    try
    {
        db.Database.Migrate();
    }
    catch (Exception) { }
}

app.Run();