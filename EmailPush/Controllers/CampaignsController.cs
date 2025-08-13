using Microsoft.AspNetCore.Mvc;
using EmailPush.Domain.Entities;
using EmailPush.Application.Services;
using EmailPush.Application.DTOs;

namespace EmailPush.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CampaignsController : ControllerBase
{
    private readonly ICampaignService _campaignService;
    private readonly ILogger<CampaignsController> _logger;

    public CampaignsController(
        ICampaignService campaignService,
        ILogger<CampaignsController> logger)
    {
        _campaignService = campaignService;
        _logger = logger;
    }


    [HttpGet]
    public async Task<ActionResult<List<CampaignDto>>> GetAll()
    {
        var campaigns = await _campaignService.GetAllAsync();
        return Ok(campaigns);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CampaignDto>> GetById(Guid id)
    {
        var campaign = await _campaignService.GetByIdAsync(id);
        if (campaign == null)
            return NotFound();

        return Ok(campaign);
    }


    [HttpPost]
    public async Task<ActionResult<CampaignDto>> Create(CreateCampaignDto dto)
    {
        try
        {
            var created = await _campaignService.CreateAsync(dto);
            _logger.LogInformation("Campaign created: {CampaignId}", created.Id);
            
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }


    [HttpPut("{id}")]
    public async Task<ActionResult<CampaignDto>> Update(Guid id, CreateCampaignDto dto)
    {
        try
        {
            var updated = await _campaignService.UpdateAsync(id, dto);
            if (updated == null)
                return NotFound();

            _logger.LogInformation("Campaign updated: {CampaignId}", updated.Id);
            return Ok(updated);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }


    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        try
        {
            var result = await _campaignService.DeleteAsync(id);
            if (!result)
                return NotFound();

            _logger.LogInformation("Campaign deleted: {CampaignId}", id);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("{id}/start")]
    public async Task<ActionResult> StartSending(Guid id)
    {
        try
        {
            var result = await _campaignService.StartSendingAsync(id);
            if (!result)
                return NotFound();

            _logger.LogInformation("Campaign started: {CampaignId}", id);
            return Ok();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }


    [HttpGet("stats")]
    public async Task<ActionResult<CampaignStatsDto>> GetStats()
    {
        var stats = await _campaignService.GetStatsAsync();
        return Ok(stats);
    }
}