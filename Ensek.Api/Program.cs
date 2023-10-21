using Ensek.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace Ensek.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
         
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddApiVersioning(c =>
            {
                c.DefaultApiVersion = new ApiVersion(1,0);
                c.ReportApiVersions = true;
                c.AssumeDefaultVersionWhenUnspecified = true;
                c.ApiVersionReader = new HeaderApiVersionReader("api-version");
            });
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Ensek api",
                    Version = "v1",
                    Description = @"Upload test files to Meter/Load or Account.Load endpoints.<br/>
                    This creates a 'DataImporter' imternally<br/>
                    DataImporters details can be found on the importers GET endpoints<br/>
                    Using the DataImporters Id, type agnostic Validate, and Import operations can be ran<br/><hr/>
                    TLDR: Updload the CSV to 'load' endpoint, then validate it, then Import to put the data into the system<hr/>"

                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
            builder.Services.AddDomainServices();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();            }

            app.UseHttpsRedirection();
            app.MapControllers();
            app.Run();
        }
    }
}