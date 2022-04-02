using Microsoft.AspNetCore.Mvc;
using Models;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace FileService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FileController : ControllerBase
    {
        private static readonly MemoryStream memoryStream;

        static FileController()
        {
            memoryStream = new MemoryStream();
        }

        [HttpPost("[action]"), ActionName("AddProduct")]
        public List<Product> AddProductProduct (Product product)
        {
            var productsFromStream = ReadFromStream();

            var newProducts = new List<Product>();


            if (productsFromStream.Count > 0)
            {
                newProducts.AddRange(productsFromStream);

                memoryStream.Seek(0, SeekOrigin.Begin);
            }

            newProducts.Add(product);

            WriteToStream(newProducts);

            return ReadFromStream();
        }

        [HttpPost("[action]"), ActionName("UpdateProduct")]
        public List<Product> UpdateProduct(Product product)
        {
            var products = ReadFromStream();

            int index = products.FindIndex(x => x.Id == product.Id);

            if (index != -1)
            {
                products[index].Metafields = product.Metafields;
            }

            memoryStream.Seek(0, SeekOrigin.Begin);

            WriteToStream(products);

            return products;
        }

        private List<Product> ReadFromStream()
        {
            memoryStream.Position = 0;

            var json = Encoding.ASCII.GetString(memoryStream.ToArray());

            var products = string.IsNullOrEmpty(json) ?
                new List<Product>() : JsonSerializer.Deserialize<List<Product>>(json);

            return products;
        }

        private void WriteToStream(List<Product> products)
        {
            var json = JsonSerializer.Serialize<List<Product>>(products);

            var writer = new StreamWriter(memoryStream);

            writer.Write(json);

            writer.Flush();
        }
    }
}
