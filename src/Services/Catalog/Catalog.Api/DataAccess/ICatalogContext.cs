using Catalog.Api.Model;
using MongoDB.Driver;

namespace Catalog.Api.DataAccess
{
    public interface ICatalogContext
    {
        IMongoCollection<Product> Products { get; }
    }
}
