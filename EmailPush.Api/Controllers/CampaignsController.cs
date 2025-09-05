using Microsoft.AspNetCore.Mvc;
using MediatR;
using EmailPush.Application.Commands;
using EmailPush.Application.Queries;
using EmailPush.Application.DTOs;
using EmailPush.Domain.Entities;

namespace EmailPush.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CampaignsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<CampaignsController> _logger;

    public CampaignsController(
        IMediator mediator,
        ILogger<CampaignsController> logger)
    {
        _mediator = mediator;
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
            var query = new GetCampaignsByStatusQuery { Status = status.Value };
            var filteredCampaigns = await _mediator.Send(query);
            return Ok(filteredCampaigns);
        }
        
        var campaigns = await _mediator.Send(new GetAllCampaignsQuery());
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
        var query = new GetCampaignByIdQuery { Id = id };
        var campaign = await _mediator.Send(query);
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
        var command = new CreateCampaignCommand
        {
            Name = dto.Name,
            Subject = dto.Subject,
            Content = dto.Content,
            Recipients = dto.Recipients
        };
        
        var created = await _mediator.Send(command);
        _logger.LogInformation("Campaign created: {CampaignId}", created.Id);
        
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
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
        var command = new UpdateCampaignCommand
        {
            Id = id,
            Name = dto.Name,
            Subject = dto.Subject,
            Content = dto.Content,
            Recipients = dto.Recipients
        };
        
        var updated = await _mediator.Send(command);
        if (updated == null)
            return NotFound();

        _logger.LogInformation("Campaign updated: {CampaignId}", updated.Id);
        return Ok(updated);
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
        var command = new PatchCampaignCommand
        {
            Id = id,
            Name = dto.Name,
            Subject = dto.Subject,
            Content = dto.Content,
            Recipients = dto.Recipients
        };
        
        var updated = await _mediator.Send(command);
        if (updated == null)
            return NotFound();

        _logger.LogInformation("Campaign partially updated: {CampaignId}", updated.Id);
        return Ok(updated);
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
        var command = new DeleteCampaignCommand { Id = id };
        var result = await _mediator.Send(command);
        if (!result)
            return NotFound();

        _logger.LogInformation("Campaign deleted: {CampaignId}", id);
        return NoContent();
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
        var command = new StartCampaignCommand { Id = id };
        var result = await _mediator.Send(command);
        if (!result)
            return NotFound();

        _logger.LogInformation("Campaign started: {CampaignId}", id);
        return Ok();
    }


    /// <summary>
    /// Get campaign statistics
    /// </summary>
    /// <returns>Statistics including total campaigns, emails sent, etc.</returns>
    [HttpGet("stats")]
    [ProducesResponseType(typeof(CampaignStatsDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<CampaignStatsDto>> GetStats()
    {
        var query = new GetCampaignStatsQuery();
        var stats = await _mediator.Send(query);
        return Ok(stats);
    }
}