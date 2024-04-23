using FoodShareNet.Domain.Entities;
using FoodShareNet.Repository.Data;
using FoodShareNetAPI.DTO.Beneficiary;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FoodShareNetAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BeneficiaryController : ControllerBase
    {
        private readonly FoodShareNetDbContext _context;

        public BeneficiaryController(FoodShareNetDbContext context)
        {
            _context = context;
        }

        [ProducesResponseType(typeof(IList<BeneficiaryDTO>),StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet]
        public async Task<ActionResult<List<BeneficiaryDTO>>> GetAllAsync()
        {

            var beneficiaries = await _context.Beneficiaries
                .Include(b => b.City)
                .Select(b => new BeneficiaryDTO
                {
                    Id = b.Id,
                    Name = b.Name,
                    Address = b.Address,
                    Capacity = b.Capacity,
                    CityName = b.City.Name

                }).ToListAsync();

            return Ok(beneficiaries);
        }


        [ProducesResponseType(typeof(BeneficiaryDTO),
        StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet]
        public async Task<ActionResult<BeneficiaryDTO>> GetAsync(int? id)
        {
            var beneficiaryDTO = await _context.Beneficiaries
                .Select(b => new BeneficiaryDTO
                {
                    Id = b.Id,
                    Name= b.Name,
                    Address= b.Address,
                    Capacity= b.Capacity,
                    CityName = b.City.Name
                }).FirstOrDefaultAsync(a => a.Id == id);

            if( beneficiaryDTO == null )
            {
                return NotFound();
            }

            return Ok(beneficiaryDTO);
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

            _context.Add(beneficiary);
            await _context.SaveChangesAsync();

            var beneficiaryEntityDTO = new BeneficiaryDetailDTO
            {
                Id = beneficiary.Id,
                Name = beneficiary.Name,
                Address = beneficiary.Address,
                CityId = beneficiary.CityId,
                Capacity = beneficiary.Capacity
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

            if( id != editBeneficiaryDTO.Id )
            {
                return BadRequest("Invalid Id!");
            }

            var beneficiary = await _context.Beneficiaries
                .FirstOrDefaultAsync(b => b.Id == editBeneficiaryDTO.Id);

            if( beneficiary == null )
            {
                return NotFound();
            }

            beneficiary.Name = editBeneficiaryDTO.Name;
            beneficiary.Address = editBeneficiaryDTO.Address;
            beneficiary.CityId = editBeneficiaryDTO.CityId;
            beneficiary.Capacity = editBeneficiaryDTO.Capacity;

            await _context.SaveChangesAsync();  

            return NoContent();
        }


        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var beneficiary = await _context.Beneficiaries.FindAsync(id);

            if( beneficiary == null ) { return NotFound(); }    

            _context.Beneficiaries.Remove(beneficiary);
            await _context.SaveChangesAsync();

            return NoContent();
        }


    }
}
