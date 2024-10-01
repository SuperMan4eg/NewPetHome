using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minio;
using NewPetHome.Applications.Database;
using NewPetHome.Applications.Files;
using NewPetHome.Applications.Messaging;
using NewPetHome.Applications.SpeciesManagement;
using NewPetHome.Applications.VolunteersManagement;
using NewPetHome.Infrastructure.BackgroundServices;
using NewPetHome.Infrastructure.DbContexts;
using NewPetHome.Infrastructure.Files;
using NewPetHome.Infrastructure.MessageQueues;
using NewPetHome.Infrastructure.Options;
using NewPetHome.Infrastructure.Providers;
using NewPetHome.Infrastructure.Repositories;
using FileInfo = NewPetHome.Applications.Files.FileInfo;

namespace NewPetHome.Infrastructure;

public static class Inject
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddDbContexts()
            .AddRepositories()
            .AddDatabase()
            .AddHostedServices()
            .AddMessageQueues()
            .AddServices()
            .AddServices()
            .AddMinio(configuration);

        return services;
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IFileCleanerService, FilesCleanerService>();

        return services;
    }

    private static IServiceCollection AddMessageQueues(this IServiceCollection services)
    {
        services.AddSingleton<IMessageQueue<IEnumerable<FileInfo>>, InMemoryMessageQueue<IEnumerable<FileInfo>>>();

        return services;
    }

    private static IServiceCollection AddHostedServices(this IServiceCollection services)
    {
        services.AddHostedService<FilesCleanerBackgroundServices>();

        return services;
    }

    private static IServiceCollection AddDatabase(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IVolunteersRepository, VolunteersRepository>();
        services.AddScoped<ISpeciesRepository, SpeciesRepository>();

        return services;
    }

    private static IServiceCollection AddDbContexts(this IServiceCollection services)
    {
        services.AddScoped<WriteDbContext>();
        services.AddScoped<IReadDbContext, ReadDbContext>();

        return services;
    }

    private static IServiceCollection AddMinio(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MinioOptions>(
            configuration.GetSection(MinioOptions.MINIO));

        services.AddMinio(options =>
        {
            var minioOptions = configuration.GetSection(MinioOptions.MINIO).Get<MinioOptions>()
                               ?? throw new ApplicationException("Missing minio configuration");

            options.WithEndpoint(minioOptions.Endpoint);
            options.WithCredentials(minioOptions.AccessKey, minioOptions.SecretKey);
            options.WithSSL(minioOptions.WithSSL);
        });

        services.AddScoped<IFileProvider, MinioProvider>();

        return services;
    }
}