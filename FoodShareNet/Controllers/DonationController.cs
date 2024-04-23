using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FoodShareNet.Domain.Entities;
using FoodShareNet.Repository.Data;
using FoodShareNetAPI.DTO.Donation;

namespace FoodShareNetAPI.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class DonationController : ControllerBase
{
    private readonly FoodShareNetDbContext _context;
    public DonationController(FoodShareNetDbContext context)
    {
        _context = context;
    }

    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPost]
    public async Task<IActionResult> CreateDonation([FromBody] CreateDonationDTO donationDTO)
    {
        if( !ModelState.IsValid )
        {
            return BadRequest();
        }

        var donation = new Donation
        {
            DonorId = donationDTO.DonorId,
            ProductId = donationDTO.ProductId,
            Quantity = donationDTO.Quantity,
            ExpirationDate = donationDTO.ExpirationDate,
            StatusId = donationDTO.StatusId,
        };

        _context.Add(donation);
        await _context.SaveChangesAsync();

        var donationEntityDTO = new DonationDetailDTO
        {
            Id = donation.Id,
            DonorId = donation.DonorId,
           // Product = donation.Product.Name,
            Quantity = donation.Quantity,
            ExpirationDate = donation.ExpirationDate,
            StatusId = donation.StatusId,
           // Status = donation.Status.Name
        };

        return Ok(donationEntityDTO);
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet]
    public async Task<ActionResult<DonationDetailDTO>> GetDonation(int id)
    {
        var donation = await _context.Donations
            .Select(d => new DonationDTO
        {
            Id = d.Id,
            Product = d.Product.Name,
            Quantity = d.Quantity,
            ExpirationDate = d.ExpirationDate,
            Status = d.Status.Name

        }).FirstOrDefaultAsync(d => d.Id == id);

        if( donation == null )
        {
            return NotFound();
        }

        return Ok(donation);
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet()]
    public async Task<ActionResult<IList<DonationDetailDTO>>> GetDonationsByCityId(int cityId)
    {
        var donationsByCityId = await _context.Donations.Where( d => d.Donor.CityId == cityId)
            .Select( d => new DonationDTO
            {
                Id = d.Id,
                Product = d.Product.Name,
                Quantity = d.Quantity,
                ExpirationDate = d.ExpirationDate,
                Status = d.Status.Name
            }).ToListAsync();

        return Ok(donationsByCityId);
    }

}
    