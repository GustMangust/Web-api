using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models;
using System;

namespace Task2.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class IdGeneratorController : ControllerBase
    {
        private readonly ILogger logger;

        public IdGeneratorController(ILogger<IdGeneratorController> logger)
        {
            this.logger = logger;
        }

        [HttpPost]
        public Product Post(Product product)
        {
            var rand = new Random();

            if (product.Id != null)
            {
                product.ToUpdate = true;
                logger.LogInformation($"Product Id:{product.Id}." + "Product to update!");
            }
            else
            {
                product.Id = rand.Next(0, 1000000);
                logger.LogInformation($"Product Id:{product.Id}." + " Service generated ids successfully!");
            }

            foreach (var metafield in product.Metafields)
            {
                metafield.Key = rand.Next(0, 1000000);
            }

           

            return product;
        }
    }
}
