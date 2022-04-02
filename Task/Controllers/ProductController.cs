using Microsoft.AspNetCore.Mvc;
using Models;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Task.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly string idGeneratorServiceUrl = "https://localhost:5001/IdGenerator";

        private readonly string addProductUrl = "https://localhost:44348/File/AddProduct";

        private readonly string updateProductUrl = "https://localhost:44348/File/UpdateProduct";

        [HttpPost]
        public async System.Threading.Tasks.Task Post(Product product)
        {
            product = await GenerateIds(product);

            var products = product.ToUpdate ? await UpdateProductRequest(product) : await AddProductRequest(product);
        }

        private async Task<Product> GenerateIds(Product product)
        {
            var response = await SendRequest(product, idGeneratorServiceUrl);

            return JsonSerializer.Deserialize<Product>(response);
        }

        private async Task<List<Product>> AddProductRequest(Product product)
        {
            var response = await SendRequest(product, addProductUrl);

            return JsonSerializer.Deserialize<List<Product>>(response);
        }

        private async Task<List<Product>> UpdateProductRequest(Product product)
        {
            var response = await SendRequest(product, updateProductUrl);

            return JsonSerializer.Deserialize<List<Product>>(response);
        }

        private async Task<string> SendRequest(Product product, string url)
        {
            WebRequest request = WebRequest.Create(url);

            request.Method = "POST";

            var json = JsonSerializer.Serialize(product);

            var byteArray = Encoding.UTF8.GetBytes(json);

            request.ContentType = "application/json";

            using (Stream dataStream = request.GetRequestStream())
            {
                dataStream.Write(byteArray, 0, byteArray.Length);
            }

            var response = await request.GetResponseAsync();

            string responseJson;

            using (Stream stream = response.GetResponseStream())
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    responseJson = reader.ReadToEnd();
                }
            }
            response.Close();

            return responseJson;
        }
    }
}
