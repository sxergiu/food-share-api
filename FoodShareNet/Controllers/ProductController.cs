using Microsoft.AspNetCore.Mvc;
using FoodShareNetAPI.DTO.Product;
using FoodShareNet.Application.Interfaces;


[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;
    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet]
    public async Task<ActionResult<IList<ProductDTO>>> GetAllAsync()
    {
        try
        {
            var products = await _productService.GetAllProductsAsync();

            var productDTOS = products.Select( p => new ProductDTO
            {
                Id = p.Id,
                Name = p.Name
            }).ToList();

            return Ok(productDTOS);
        }
        catch(Exception e) { 
            return NotFound(e.Message); 
        }
    }

}
