using FoodShareNet.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using FoodShareNetAPI.DTO.Donor;
using FoodShareNetAPI.DTO.Donation;
using FoodShareNet.Application.Interfaces; // Ensure you have the corresponding DTO namespace

namespace FoodShareNetAPI.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class DonorController : ControllerBase
{
    private readonly IDonorService _donorService;
    public DonorController(IDonorService donorService)
    {
        _donorService = donorService;
    }

    [ProducesResponseType(type: typeof(List<DonorDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet]
    public async Task<ActionResult<IList<DonorDTO>>> GetAllAsync()
    {
        try
        {
            var donors = await _donorService.GetAllDonorsAsync();
            var donorDTOS = donors
            .Select(d => new DonorDTO
            {
                Id = d.Id,
                Name = d.Name,
                Address = d.Address,
                CityName = d.City.Name,
                Donations = d.Donations.Select(donation => new DonationDTO
                {
                    Id = donation.Id,
                    Product = donation.Product.Name,
                    Quantity = donation.Quantity,
                    Status = donation.Status.Name,
                    ExpirationDate = donation.ExpirationDate
                }).ToList()

            }).ToList();

            return Ok(donorDTOS);
        }
        catch(Exception e) { return NotFound(e.Message); }
    }


    [ProducesResponseType(type: typeof(List<DonorDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet()]
    public async Task<ActionResult<DonorDTO>> GetAsync(int id)
    {
        try
        {
            var donor = await _donorService.GetDonorByIdAsync(id);

            var donorDTO = new DonorDTO
            {
                Id = donor.Id,
                Name = donor.Name,
                CityName = donor.City.Name,
                Address = donor.Address,
                Donations = donor.Donations.Select(d => new DonationDTO
                {
                    Id = d.Id,
                    Product = d.Product.Name,
                    Quantity = d.Quantity,
                    ExpirationDate = d.ExpirationDate,
                    Status = d.Status.Name,

                }).ToList()
            };
            return Ok(donorDTO);
        }
        catch( Exception e ) { return NotFound(e.Message); };
    }

    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPost]
    public async Task<ActionResult<DonorDetailDTO>> CreateAsync( CreateDonorDTO createDonorDTO)
    {
        if( !ModelState.IsValid )
        {
            return BadRequest();
        }

        var donor = new Donor
        {
            Name = createDonorDTO.Name,
            Address = createDonorDTO.Address,
            CityId = createDonorDTO.CityId
        };

        var createdDonor = await _donorService.CreateDonorAsync(donor);

        var donorEntityDTO = new DonorDetailDTO
        {
            Id = createdDonor.Id,
            Name = createdDonor.Name,
            Address = createdDonor.Address,
            CityId = createdDonor.CityId,
        };

        return Ok(donorEntityDTO);
    }

    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPut()]
    public async Task<IActionResult> EditAsync(int id,EditDonorDTO editDonorDTO)
    {
        if( id != editDonorDTO.Id )
            return BadRequest("Invalid Donor Id!");

        var donor = new Donor
        {
            Name = editDonorDTO.Name,
            Address = editDonorDTO.Address,
            CityId = editDonorDTO.CityId,
        };

        try
        {
            var editedDonor = await _donorService.EditDonorAsync(id, donor);
        }
        catch (Exception e) {

            return NotFound(e.Message);
        }

        return NoContent();
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpDelete()]
    public async Task<IActionResult> DeleteAsync(int id)
    {
         try
         {
            await _donorService.DeleteDonorAsync(id);   

         }catch(Exception e) {
            return BadRequest(e.Message);
         }

        return NoContent(); 
    }
}
