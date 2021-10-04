using Catalog.Api.Model;
using MediatR;
using System.Collections.Generic;

namespace Catalog.Api.Services
{
    public class GetProducts: IRequest<IEnumerable<Product>>
    {
        public string Name { get; set; }
        public string Category { get; set; }
    }
}
