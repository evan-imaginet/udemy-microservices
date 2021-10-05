using Catalog.Api.Model;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;

namespace Catalog.Api.Services
{
    public class UpdateProduct: IRequest<Product>
    {
        public string Id { get; set; }
        public JsonPatchDocument<Product> Updates { get; set; }
    }
}
