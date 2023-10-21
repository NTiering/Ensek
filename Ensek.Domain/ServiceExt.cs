using Ensek.Domain.App;
using Ensek.Domain.Data.Domain;
using Ensek.Domain.Data.System;
using Ensek.Domain.Repositories;
using Ensek.Domain.Validators;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Ensek.Domain;

public static class ServiceExt
{
    public static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        //todo : Remove prior to production
        services.AddDbContext<DomainDbContext>(options => options.UseInMemoryDatabase(databaseName: "Domain"));
        services.AddDbContext<SystemDbContext>(options => options.UseInMemoryDatabase(databaseName: "System"));

        services.AddScoped<IDataImpoterFactory, DataImpoterFactory>();
        services.AddScoped<IDataImporterRepository, DataImporterRepository>();
        services.AddScoped<IMeterReadingRepository, MeterReadingRepository>();
        services.AddScoped<IAccountUpdateRepository, AccountUpdateRepository>();
        services.AddScoped<IImporterErrorRepository, ImporterErrorRepository>();
        services.AddScoped<IValidator<Data.Domain.MeterReading>, MeterReadingValidator>();
        services.AddScoped<IValidator<AccountUpdate>, AccountUpdateValidator>();
        services.AddScoped<ISystemRepository, SystemRepository>();
        services.AddScoped<IDateTimeService, DateTimeService>();

        return services;
    }
}
