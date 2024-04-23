using FoodShareNet.Domain.Entities;
using FoodShareNet.Repository.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FoodShareNetAPI.DTO.Donor;
using FoodShareNetAPI.DTO.Donation; // Ensure you have the corresponding DTO namespace

namespace FoodShareNetAPI.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class DonorController : ControllerBase
{
    private readonly FoodShareNetDbContext _context;
    public DonorController(FoodShareNetDbContext context)
    {
        _context = context;
    }

    [ProducesResponseType(type: typeof(List<DonorDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet]
    public async Task<ActionResult<IList<DonorDTO>>> GetAllAsync()
    {
        var donors = await _context.Donors
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

            }).ToListAsync();

        if( donors.Count == 0 )
        {
            return NotFound();
        }

        return Ok(donors); ///handling 1 to many
    }


    [ProducesResponseType(type: typeof(List<DonorDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet()]
    public async Task<ActionResult<DonorDTO>> GetAsync(int id)
    {
        var donorDTO = await _context.Donors.Select(d => new DonorDTO
        {
            Id = d.Id,
            Name = d.Name,
            Address = d.Address,
            CityName = d.City.Name,
            Donations = d.Donations.Select( donation => new DonationDTO
            {
                Id = donation.Id,
                Product = donation.Product.Name,
                ExpirationDate = donation.ExpirationDate,
                Status = donation.Status.Name,
            }).ToList()

        }).FirstOrDefaultAsync( a => a.Id == id );

        if( donorDTO == null )
        {
            return NotFound();
        }

        return Ok(donorDTO); //handling 1to many
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

        _context.Donors.Add( donor );
        await _context.SaveChangesAsync();

        var donorEntityDTO = new DonorDetailDTO
        {
            Id = donor.Id,
            Name = donor.Name,
            Address = donor.Address,
            CityId = donor.CityId,
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

        var donor = await _context.Donors.FirstOrDefaultAsync( d=>d.Id == editDonorDTO.Id);
        if( donor == null)
        {
            return NotFound();
        }

        donor.Name = editDonorDTO.Name;
        donor.Address = editDonorDTO.Address;
        donor.CityId = editDonorDTO.CityId;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpDelete()]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        var donor = await _context.Donors.FirstOrDefaultAsync( d=> d.Id == id);

        if( donor == null) { return NotFound();  }
        _context.Remove(donor);
        await _context.SaveChangesAsync();

        return NoContent(); //deleting a donor means deleting also his donations?
    }
}
