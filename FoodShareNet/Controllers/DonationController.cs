using Microsoft.AspNetCore.Mvc;
using FoodShareNet.Domain.Entities;
using FoodShareNetAPI.DTO.Donation;
using FoodShareNet.Application.Interfaces;

namespace FoodShareNetAPI.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class DonationController : ControllerBase
{
    private readonly IDonationService _donationService;
    public DonationController(IDonationService donationService)
    {
        _donationService = donationService;
        
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

        var createdDonation = await _donationService.CreateDonationAsync(donation);

        var donationDetailDTO = new DonationDetailDTO
        {
           Id = createdDonation.Id,
           DonorId = createdDonation.DonorId,
           //Product = createdDonation.Product.Name,
           Quantity = createdDonation.Quantity,
           ExpirationDate = createdDonation.ExpirationDate,
           StatusId = createdDonation.StatusId,
           //Status = createdDonation.Status.Name
        };

        return Ok(donationDetailDTO);
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet]
    public async Task<ActionResult<DonationDetailDTO>> GetDonation(int id)
    {
        try
        {
            var donation = await _donationService.GetDonationByIdAsync(id);
            return Ok(donation);
        }
        catch(Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet()]
    public async Task<ActionResult<IList<DonationDetailDTO>>> GetDonationsByCityId(int cityId)
    {

        try
        {
            var donationsByCityId = await _donationService.GetDonationsByCityIdAsync(cityId);

            var dontationDetails = donationsByCityId
            .Select(donation => new DonationDetailDTO
            {
                Id = donation.Id,
                DonorId = donation.DonorId,
               // Product = donation.Product.Name,
                Quantity = donation.Quantity,
                ExpirationDate = donation.ExpirationDate,
                StatusId = donation.StatusId,
               // Status = donation.Status.Name
            }).ToList();

            return Ok(dontationDetails);
        }
        catch(Exception ex)
        {
            return NotFound(ex.Message);
        }

    }

}
    