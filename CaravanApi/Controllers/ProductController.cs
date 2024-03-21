using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using CaravanApi.Models;
using System.Net;
using Microsoft.EntityFrameworkCore;
using CaravanApi.Context;

namespace RichShopAuthApi.Controllers
{

    //Manages products.
    [ApiController]
    [Route("[controller]")]
    public class ProductController : Controller
    {
        private readonly ILogger<ProductController> _logger;
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;

        //Creates a controller to manage product.
        public ProductController(ILogger<ProductController> logger, AppDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            Console.WriteLine($"{nameof(ProductController)}.ctor()");
            _logger = logger;
            _db = db;
            _webHostEnvironment = webHostEnvironment;
        }


        //Gets all products.
        [HttpGet("getAllProducts")]
        public async Task<IEnumerable<Product>> GetAllProducts(string? name = null, string? category = null)
        {
            _logger.LogDebug($"{nameof(GetAllProducts)}()");

            IQueryable<Product> query = _db.Products;

            if (!string.IsNullOrWhiteSpace(name))
            {
                query = query.Where(product => EF.Functions.Like(product.Name, $"%{name}%"));
            }

            if (!string.IsNullOrWhiteSpace(category))
            {
                query = query.Where(product => EF.Functions.Like(product.Category, $"%{category}%"));
            }

            return await query
                    .AsNoTracking()
                    .Select(product => new Product
                    {
                        Id = product.Id,
                        Name = product.Name,
                        Category = product.Category,
                        Description = product.Description,
                        Price = product.Price,
                        FilePath = product.FilePath,
                    })
                    .ToListAsync();
        }

        //<summary> Creates new product.
        [HttpPost("createProduct")]
        public async Task<IActionResult> CreateProduct(IFormFile photo, IFormCollection formCollection)
        {
            var product = JsonConvert.DeserializeObject<Product>(formCollection["product"]);
            if (photo != null)

            {
                var path = Path.Combine("C:\\Users\\faridun.turdiev\\Here\\Programming\\Projects\\Shop\\MyShop\\Rich-Shop\\src\\assets", "Images",
                    photo.FileName);

                product.FilePath = path.Replace("C:\\Users\\faridun.turdiev\\Here\\Programming\\Projects\\Shop\\MyShop\\Rich-Shop\\src\\", "").Replace("\\", "/");

                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    photo.CopyTo(fileStream);
                }
            }


            await _db.AddAsync(product);
            await _db.SaveChangesAsync();

            return Created(Url.Action("GetAllProducts", new { name = product.Name })!, product);
        }




        //Updates existing products data.
        [HttpPut("updateProduct")]
        public async Task<IActionResult> UpdateProduct(string name, Product changed)
        {
            _logger.LogDebug($"{nameof(UpdateProduct)}({name}, {changed})");

            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest($"Invalid product name: {name}");
            }

            var stored = await _db.Products.FirstOrDefaultAsync(product => product.Name == name);

            if (stored == null)
            {
                return base.NotFound();
            }

            stored.Name = changed.Name;
            stored.Category = changed.Category;
            stored.Description = changed.Description;
            stored.Price = changed.Price;

            await _db.SaveChangesAsync();

            return NoContent();
        }

        //Deletes a specific product.
        [HttpDelete("deleteProduct")]
        public async Task<IActionResult> DeleteProduct(string name)
        {
            _logger.LogDebug($"{nameof(DeleteProduct)}({name})");

            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest($"Invalid product name: {name}");
            }


            var product = await _db.Products
                .Where(p => p.Name == name)
                .FirstOrDefaultAsync();

            if (product == null)
            {
                return NotFound($"Product not found: {name}");
            }

            var imagePath = Path.Combine("C:\\Users\\faridun.turdiev\\Here\\Programming\\Projects\\Shop\\MyShop\\Rich-Shop\\src\\", product.FilePath);

            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }


            _db.Products.Remove(product);
            await _db.SaveChangesAsync();

            return NoContent();
        }

    }
}

