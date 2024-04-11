using FoodShareNetAPI.DTO.Beneficiary;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FoodShareNetAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BeneficiaryController : ControllerBase
    {
        public BeneficiaryController()
        {
            
        }

        [ProducesResponseType(typeof(IList<BeneficiaryDTO>),StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet]
        public async Task<ActionResult<List<BeneficiaryDTO>>> GetAllAsync()
        {
            return Ok();
        }




        [ProducesResponseType(typeof(BeneficiaryDTO),
        StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet]
        public async Task<ActionResult<BeneficiaryDTO>> GetAsync(int? id)
        {
            return Ok();
        }




        [ProducesResponseType(typeof(BeneficiaryDetailDTO),
            StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        public async Task<ActionResult<BeneficiaryDetailDTO>>
            CreateAsync(CreateBeneficiaryDTO createBeneficiaryDTO) //why not from body?
        {
            return Ok();
        }





        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPut]
        public async Task<IActionResult>
            EditAsync(int id, EditBeneficiaryDTO editBeneficiaryDTO)
        {
            return Ok();
        }




        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            return Ok();
        }


    }
}
