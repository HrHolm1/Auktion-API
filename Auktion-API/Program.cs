using Auktion_API.DataAccess;
using Auktion_API.Services;
using Microsoft.EntityFrameworkCore;

namespace Auktion_API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        builder.Services.AddCors();
        
        builder.Services.AddControllers();
        
        // Add services to the container.
        builder.Services.AddScoped<AuctionService>();
        builder.Services.AddScoped<LotService>();
        builder.Services.AddScoped<BidService>();
        
        // Swagger
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        
        //builder.Services.AddOpenApi();
        builder.Services.AddDbContext<AuctionContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));
        
        var app = builder.Build();
        
        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<AuctionContext>(); // your context type
            db.Database.Migrate(); // applies any pending migrations, creates DB/tables if needed
        }

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            
            app.UseSwaggerUI(options => // UseSwaggerUI is called only in Development.
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                options.RoutePrefix = string.Empty;
            });
        }
        
        app.UseCors(options => options.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
        
        //app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}