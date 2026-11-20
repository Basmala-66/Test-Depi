using AutoMapper;
using E_commerce_System.Data;
using E_commerce_System.DTOs.ProductDTOs;
using E_commerce_System.Entities;
using E_commerce_System.filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_commerce_System.DTOs.Controllers
{
    /// <summary>
    /// Provides endpoints for managing products.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [ServiceFilter(typeof(ExcptionHandlingFilter))]
    public class ProductController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<ProductController> _logger;
        private readonly IMapper _mapper;

        public ProductController(
            AppDbContext context,
            ILogger<ProductController> logger,
            IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Retrieves all products.
        /// </summary>
        /// <remarks>
        /// Returns all products including their category names.
        ///
        /// Sample response:
        ///
        ///     [
        ///         {
        ///             "productName": "Laptop",
        ///             "price": 1299.99,
        ///             "categoryName": "Electronics"
        ///         }
        ///     ]
        ///
        /// </remarks>
        /// <returns>A list of products.</returns>
        [HttpGet("products")]
        [ProducesResponseType(typeof(List<GetProductResponse>), StatusCodes.Status200OK)]
        [ServiceFilter(typeof (ActionExecutionLogingFilter))]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _context.Products
                .Include(product => product.Category)
                .ToListAsync();

            var getProducts = _mapper.Map<List<GetProductResponse>>(products);

            return Ok(getProducts);
        }

        /// <summary>
        /// Retrieves a product by its identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the product.</param>
        /// <returns>The requested product.</returns>
        /// <response code="200">The product was found.</response>
        /// <response code="404">No product exists with the specified identifier.</response>
        [HttpGet("GetById/{id:int}")]
        [ProducesResponseType(typeof(GetProductResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ServiceFilter(typeof(ActionTimingFilter))]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _context.Products
                .Include(product => product.Category)
                .FirstOrDefaultAsync(product => product.Id == id);

            if (product is null)
            {
                _logger.LogWarning(
                    "Product with ID {ProductId} was not found.",
                    id);

                return NotFound($"Product with ID {id} was not found.");
            }

            var response = _mapper.Map<GetProductResponse>(product);

            return Ok(response);
        }

        /// <summary>
        /// Creates a new product.
        /// </summary>
        /// <param name="request">The product information.</param>
        /// <returns>The newly created product.</returns>
        /// <response code="201">The product was created successfully.</response>
        /// <response code="400">The request body is invalid.</response>
        [HttpPost]
        [ProducesResponseType(typeof(Product), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateProduct(
            [FromBody] GetProductRequest request)
        {
            var product = new Product
            {
                Name = request.ProductName,
                Price = request.Price,
                CategoryId = request.CategoryId
            };

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetProductById),
                new { id = product.Id },
                product);
        }

        /// <summary>
        /// Updates an existing product.
        /// </summary>
        /// <param name="request">The updated product information.</param>
        /// <returns>The updated product.</returns>
        /// <response code="200">The product was updated successfully.</response>
        /// <response code="400">The request body is invalid.</response>
        /// <response code="404">No product exists with the specified identifier.</response>
        [HttpPut]
        [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateProduct(
            [FromBody] UpdateProductRequest request)
        {
            var product = await _context.Products
                .FirstOrDefaultAsync(product => product.Id == request.Id);

            if (product is null)
            {
                return NotFound("Product not found.");
            }

            product.Name = request.ProductName;
            product.Price = request.Price;
            product.CategoryId = request.CategoryId;

            await _context.SaveChangesAsync();

            return Ok(product);
        }

        /// <summary>
        /// Soft-deletes a product.
        /// </summary>
        /// <param name="id">The unique identifier of the product.</param>
        /// <returns>A confirmation message.</returns>
        /// <response code="200">The product was deleted successfully.</response>
        /// <response code="404">No product exists with the specified identifier.</response>
        [HttpDelete]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteProduct([FromQuery] int id)
        {
            var product = await _context.Products
                .FirstOrDefaultAsync(product => product.Id == id);

            if (product is null)
            {
                return NotFound("Product not found.");
            }

            product.IsDeleted = true;

            await _context.SaveChangesAsync();

            return Ok("Product deleted.");
        }
    }
}
