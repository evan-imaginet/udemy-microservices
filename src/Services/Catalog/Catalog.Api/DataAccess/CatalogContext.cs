using Catalog.Api.Model;
using Catalog.Api.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;

namespace Catalog.Api.DataAccess
{
    public class CatalogContext : ICatalogContext
    {
        private readonly ILogger<CatalogContext> logger;
        private readonly IOptions<MongoDbSettings> mongoDbOptions;
        private readonly ICatalogSeeder seeder;

        public CatalogContext(ILogger<CatalogContext> logger, IOptions<MongoDbSettings> mongoDbOptions, ICatalogSeeder seeder)
        {
            this.logger = logger;
            this.mongoDbOptions = mongoDbOptions;
            this.seeder = seeder;

            Initialize();
        }

        private void Initialize()
        {
            var settings = mongoDbOptions.Value;
            logger.LogDebug($"Creating CatalogContext with\tServer: {settings.Server}\tDatabase: {settings.Database}\tCollection: {settings.Collection}");

            var client = new MongoClient(settings.Server);
            var database = client.GetDatabase(settings.Database);

            Products = database.GetCollection<Product>(settings.Collection);
            seeder.Seed(Products);
        }


        public IMongoCollection<Product> Products { get; private set; }
    }
}
