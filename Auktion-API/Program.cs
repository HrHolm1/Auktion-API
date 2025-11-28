using System.Text;
using Auktion_API.DataAccess;
using Auktion_API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Auktion_API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        builder.Services.AddCors();
        
        builder.Services.AddControllers();
        
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "emptydomain.com",
                    ValidAudience = "emptydomain.com",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("5f1bd9c614b1dcf074313362f2ceb290037f83a39a4f4ff5183de85fea183ace"))
                };
            });
        
        builder.Services.AddAuthorization();
        
        // Add services to the container.
        builder.Services.AddScoped<AuctionService>();
        builder.Services.AddScoped<LotService>();
        builder.Services.AddScoped<BidService>();
        builder.Services.AddScoped<AuthService>();
        
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
        
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}