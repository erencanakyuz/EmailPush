using Microsoft.AspNetCore.Mvc;
using EmailPush.Application.Services;
using EmailPush.Application.DTOs;

namespace EmailPush.Api.Controllers;

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


    /// <summary>
    /// Get all email campaigns with optional status filter
    /// </summary>
    /// <param name="status">Filter by status: 0=Draft, 1=Ready, 2=Sending, 3=Completed, 4=Failed</param>
    [HttpGet]
    [ProducesResponseType(typeof(List<CampaignDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<CampaignDto>>> GetCampaigns([FromQuery] CampaignStatus? status = null)
    {
        if (status.HasValue)
        {
            var filteredCampaigns = await _campaignService.GetCampaignsByStatusAsync(status.Value);
            return Ok(filteredCampaigns);
        }
        
        var campaigns = await _campaignService.GetAllAsync();
        return Ok(campaigns);
    }

    /// <summary>
    /// Get a specific campaign by ID
    /// </summary>
    /// <param name="id">Campaign unique identifier</param>
    /// <returns>Campaign details</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(CampaignDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CampaignDto>> GetById(Guid id)
    {
        var campaign = await _campaignService.GetByIdAsync(id);
        if (campaign == null)
            return NotFound();

        return Ok(campaign);
    }


    /// <summary>
    /// Create a new email campaign
    /// </summary>
    /// <param name="dto">Campaign creation data</param>
    /// <returns>Created campaign</returns>
    /// <response code="201">Campaign created successfully</response>
    /// <response code="400">Invalid campaign data or email addresses</response>
    [HttpPost]
    [ProducesResponseType(typeof(CampaignDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
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


    /// <summary>
    /// Update an existing campaign (only draft campaigns)
    /// </summary>
    /// <param name="id">Campaign unique identifier</param>
    /// <param name="dto">Campaign data</param>
    /// <returns>Updated campaign</returns>
    /// <response code="200">Campaign updated successfully</response>
    /// <response code="400">Invalid data or campaign not in draft status</response>
    /// <response code="404">Campaign not found</response>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(CampaignDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
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

    /// <summary>
    /// Partially update an existing campaign (only draft campaigns)
    /// </summary>
    /// <param name="id">Campaign unique identifier</param>
    /// <param name="dto">Partial campaign data for update</param>
    /// <returns>Updated campaign</returns>
    /// <response code="200">Campaign updated successfully</response>
    /// <response code="400">Invalid data or campaign not in draft status</response>
    /// <response code="404">Campaign not found</response>
    [HttpPatch("{id}")]
    [ProducesResponseType(typeof(CampaignDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CampaignDto>> PatchUpdate(Guid id, UpdateCampaignDto dto)
    {
        try
        {
            var updated = await _campaignService.PatchAsync(id, dto);
            if (updated == null)
                return NotFound();

            _logger.LogInformation("Campaign partially updated: {CampaignId}", updated.Id);
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

    /// <summary>
    /// Delete a campaign (only draft campaigns)
    /// </summary>
    /// <param name="id">Campaign unique identifier</param>
    /// <returns>No content</returns>
    /// <response code="204">Campaign deleted successfully</response>
    /// <response code="400">Campaign not in draft status</response>
    /// <response code="404">Campaign not found</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
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

    /// <summary>
    /// Start sending emails for a campaign
    /// </summary>
    /// <param name="id">Campaign unique identifier</param>
    /// <returns>Success confirmation</returns>
    /// <response code="200">Campaign started successfully</response>
    /// <response code="400">Campaign not in draft status</response>
    /// <response code="404">Campaign not found</response>
    [HttpPost("{id}/start")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
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


    /// <summary>
    /// Get campaign statistics
    /// </summary>
    /// <returns>Statistics including total campaigns, emails sent, etc.</returns>
    [HttpGet("stats")]
    [ProducesResponseType(typeof(CampaignStatsDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<CampaignStatsDto>> GetStats()
    {
        var stats = await _campaignService.GetStatsAsync();
        return Ok(stats);
    }
}