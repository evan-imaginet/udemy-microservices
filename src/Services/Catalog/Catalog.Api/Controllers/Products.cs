using Catalog.Api.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace Catalog.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Products : ControllerBase
    {
        private readonly ILogger<Products> _logger;

        public Products(ILogger<Products> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<ProductViewModel> Get()
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        public IEnumerable<ProductViewModel> GetByName([FromQuery] string name)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        public IEnumerable<ProductViewModel> GetByCategoryName([FromQuery] string category)
        {
            throw new NotImplementedException();
        }

        [HttpGet("{id}")]
        public IEnumerable<ProductViewModel> GetById(string id)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public IEnumerable<ProductViewModel> Post()
        {
            throw new NotImplementedException();
        }

        [HttpDelete]
        public IEnumerable<ProductViewModel> Delete()
        {
            throw new NotImplementedException();
        }

    }
}
