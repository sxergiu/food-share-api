using FoodShareNet.Application.Interfaces;
using FoodShareNet.Domain.Entities;
using FoodShareNetAPI.DTO.Beneficiary;
using Microsoft.AspNetCore.Mvc;

namespace FoodShareNetAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BeneficiaryController : ControllerBase
    {
        private readonly IBeneficiaryService _beneficiaryService;

        public BeneficiaryController(IBeneficiaryService beneficiaryService)
        {
            _beneficiaryService = beneficiaryService;
        }

        [ProducesResponseType(typeof(IList<BeneficiaryDTO>),StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet]
        public async Task<ActionResult<List<BeneficiaryDTO>>> GetAllAsync()
        {

            var beneficiaries = await _beneficiaryService.GetAllBeneficiariesAsync();
                
            var beneficiariesDTO = beneficiaries
                .Select(b => new BeneficiaryDTO
                {
                    Id = b.Id,
                    Name = b.Name,
                    Address = b.Address,
                    Capacity = b.Capacity,
                    CityName = b.City.Name
                }).ToList();

            return Ok(beneficiariesDTO);
        }


        [ProducesResponseType(typeof(BeneficiaryDTO),
        StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet]
        public async Task<ActionResult<BeneficiaryDTO>> GetAsync(int id)
        {
            try
            {
                var b = await _beneficiaryService.GetBeneficiaryByIdAsync(id);

                var beneficiaryDTO = new BeneficiaryDTO
                {
                    Id = b.Id,
                    Name = b.Name,
                    Address = b.Address,
                    Capacity = b.Capacity,
                    CityName = b.City.Name
                };

                return Ok(beneficiaryDTO);
            }
            catch (Exception e) { return NotFound(e.Message); }
        }

        [ProducesResponseType(typeof(BeneficiaryDetailDTO),
            StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        public async Task<ActionResult<BeneficiaryDetailDTO>>
            CreateAsync(CreateBeneficiaryDTO createBeneficiaryDTO) 
        {
            if( !ModelState.IsValid )
            {
                return BadRequest(ModelState);
            }

            var beneficiary = new Beneficiary
            {
                Name = createBeneficiaryDTO.Name,
                Address = createBeneficiaryDTO.Address,
                CityId = createBeneficiaryDTO.CityId,
                Capacity = createBeneficiaryDTO.Capacity
            };

            var createdBeneficiary = await _beneficiaryService.CreateBeneficiaryAsync(beneficiary);

            var beneficiaryEntityDTO = new BeneficiaryDetailDTO
            {
                Id = createdBeneficiary.Id,
                Name = createdBeneficiary.Name,
                Address = createdBeneficiary.Address,
                CityId = createdBeneficiary.CityId,
                Capacity = createdBeneficiary.Capacity
            };

            return Ok(beneficiaryEntityDTO);
        }


        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPut]
        public async Task<IActionResult>
            EditAsync(int id, EditBeneficiaryDTO editBeneficiaryDTO)
        {

            if (id != editBeneficiaryDTO.Id)
            {
                return BadRequest("Invalid Id!");
            }

            var beneficiary = new Beneficiary
            {
                Name = editBeneficiaryDTO.Name,
                Address = editBeneficiaryDTO.Address,
                CityId = editBeneficiaryDTO.CityId,
                Capacity = editBeneficiaryDTO.Capacity
            };

            try
            {
                var editedBeneficiary = await _beneficiaryService.EditBeneficiaryAsync(id, beneficiary);
            }
            catch(Exception e)
            {
                return NotFound(e.Message);
            }
            return NoContent();

        }


        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            try
            {
              await _beneficiaryService.DeleteBeneficiaryAsync(id);
            }
            catch(Exception e)
            {
                return NotFound(e.Message);
            }
           
            return NoContent();
        }


    }
}
