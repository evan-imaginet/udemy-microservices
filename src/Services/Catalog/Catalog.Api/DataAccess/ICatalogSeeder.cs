using Catalog.Api.Model;
using MongoDB.Driver;

namespace Catalog.Api.DataAccess
{
    public interface ICatalogSeeder
    {
        void Seed(IMongoCollection<Product> collection);
    }
}
