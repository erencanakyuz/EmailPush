using Microsoft.AspNetCore.Mvc;
using MediatR;
using EmailPush.Application.Commands;
using EmailPush.Application.Queries;
using EmailPush.Application.DTOs;
// Removed direct reference to Domain.Entities to avoid domain layer leakage
// Will use string-based status filtering instead

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
    /// <param name="status">Filter by status: Draft, Ready, Sending, Completed, Failed</param>
    /// <param name="pageNumber">Page number (default: 1)</param>
    /// <param name="pageSize">Page size (default: 10, max: 100)</param>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResponseDto<CampaignDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResponseDto<CampaignDto>>> GetCampaigns(
        [FromQuery] string? status = null,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        // Validate pagination parameters
        if (pageNumber < 1)
            pageNumber = 1;
            
        if (pageSize < 1)
            pageSize = 10;
            
        if (pageSize > 100)
            pageSize = 100;

        var query = new GetCampaignsQuery 
        { 
            PageNumber = pageNumber,
            PageSize = pageSize
        };
        
        // Convert string status to enum if provided
        if (!string.IsNullOrEmpty(status))
        {
            if (Enum.TryParse<CampaignStatus>(status, true, out var statusEnum))
            {
                query.Status = statusEnum;
            }
            else
            {
                return BadRequest($"Invalid status value: {status}. Valid values are: Draft, Ready, Sending, Completed, Failed");
            }
        }
        
        var campaigns = await _mediator.Send(query, HttpContext.RequestAborted);
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
        var campaign = await _mediator.Send(query, HttpContext.RequestAborted);
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
        
        try
        {
            var created = await _mediator.Send(command, HttpContext.RequestAborted);
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
        var command = new UpdateCampaignCommand
        {
            Id = id,
            Name = dto.Name,
            Subject = dto.Subject,
            Content = dto.Content,
            Recipients = dto.Recipients
        };
        
        try
        {
            var updated = await _mediator.Send(command, HttpContext.RequestAborted);
            if (updated == null)
                return NotFound();

            _logger.LogInformation("Campaign updated: {CampaignId}", updated.Id);
            return Ok(updated);
        }
        catch (InvalidOperationException ex)
        {
            // Business rule violation (e.g., trying to update non-draft campaign)
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
        var command = new PatchCampaignCommand
        {
            Id = id,
            Name = dto.Name,
            Subject = dto.Subject,
            Content = dto.Content,
            Recipients = dto.Recipients
        };
        
        try
        {
            var updated = await _mediator.Send(command, HttpContext.RequestAborted);
            if (updated == null)
                return NotFound();

            _logger.LogInformation("Campaign partially updated: {CampaignId}", updated.Id);
            return Ok(updated);
        }
        catch (InvalidOperationException ex)
        {
            // Business rule violation (e.g., trying to update non-draft campaign)
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
        var command = new DeleteCampaignCommand { Id = id };
        
        try
        {
            var result = await _mediator.Send(command, HttpContext.RequestAborted);
            if (!result)
                return NotFound();

            _logger.LogInformation("Campaign deleted: {CampaignId}", id);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            // Business rule violation (e.g., trying to delete non-draft campaign)
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
        var command = new StartCampaignCommand { Id = id };
        
        try
        {
            var result = await _mediator.Send(command, HttpContext.RequestAborted);
            if (!result)
                return NotFound();

            _logger.LogInformation("Campaign started: {CampaignId}", id);
            return Ok();
        }
        catch (InvalidOperationException ex)
        {
            // Business rule violation (e.g., trying to start non-draft campaign)
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
        var query = new GetCampaignStatsQuery();
        var stats = await _mediator.Send(query, HttpContext.RequestAborted);
        return Ok(stats);
    }
}