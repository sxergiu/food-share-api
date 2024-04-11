using Microsoft.AspNetCore.Mvc;
using FoodShareNetAPI.DTO.Product;


[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{

    public ProductController()
    {

    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet]
    public async Task<ActionResult<IList<ProductDTO>>> GetAllAsync()
    {
        return Ok();
    }

}
