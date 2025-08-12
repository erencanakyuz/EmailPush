using Microsoft.AspNetCore.Mvc;
using EmailPush.Domain.Entities;
using EmailPush.Domain.Interfaces;
using MassTransit;

namespace EmailPush.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CampaignsController : ControllerBase
{
    private readonly ICampaignRepository _repository;
    private readonly IPublishEndpoint? _publishEndpoint;
    private readonly ILogger<CampaignsController> _logger;

    public CampaignsController(
        ICampaignRepository repository, 
        ILogger<CampaignsController> logger,
        IPublishEndpoint? publishEndpoint = null)
    {
        _repository = repository;
        _logger = logger;
        _publishEndpoint = publishEndpoint;
    }


    [HttpGet]
    public async Task<ActionResult<List<Campaign>>> GetAll()
    {
        var campaigns = await _repository.GetAllAsync();
        return Ok(campaigns.ToList());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Campaign>> GetById(Guid id)
    {
        var campaign = await _repository.GetByIdAsync(id);
        if (campaign == null)
            return NotFound();

        return Ok(campaign);
    }


    [HttpPost]
    public async Task<ActionResult<Campaign>> Create(Campaign campaign)
    {
        if (string.IsNullOrEmpty(campaign.Name))
            return BadRequest("Campaign name is required");

        if (campaign.Recipients == null || campaign.Recipients.Count == 0)
            return BadRequest("At least one recipient is required");

        campaign.Id = Guid.NewGuid();
        campaign.CreatedAt = DateTime.UtcNow;
        campaign.Status = CampaignStatus.Draft;
        campaign.SentCount = 0;

        var created = await _repository.AddAsync(campaign);
        _logger.LogInformation("Campaign created: {CampaignId}", created.Id);
        
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }


    [HttpPut("{id}")]
    public async Task<ActionResult<Campaign>> Update(Guid id, Campaign campaign)
    {
        var existing = await _repository.GetByIdAsync(id);
        if (existing == null)
            return NotFound();

        if (existing.Status != CampaignStatus.Draft)
            return BadRequest("Only draft campaigns can be updated");

        existing.Name = campaign.Name;
        existing.Subject = campaign.Subject;
        existing.Content = campaign.Content;
        existing.Recipients = campaign.Recipients;

        await _repository.UpdateAsync(existing);
        _logger.LogInformation("Campaign updated: {CampaignId}", existing.Id);
        
        return Ok(existing);
    }


    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        var campaign = await _repository.GetByIdAsync(id);
        if (campaign == null)
            return NotFound();

        if (campaign.Status != CampaignStatus.Draft)
            return BadRequest("Only draft campaigns can be deleted");

        await _repository.DeleteAsync(campaign);
        _logger.LogInformation("Campaign deleted: {CampaignId}", id);
        
        return NoContent();
    }

    [HttpPost("{id}/start")]
    public async Task<ActionResult> StartSending(Guid id)
    {
        var campaign = await _repository.GetByIdAsync(id);
        if (campaign == null)
            return NotFound();

        if (campaign.Status != CampaignStatus.Draft)
            return BadRequest("Only draft campaigns can be started");

        campaign.Status = CampaignStatus.Ready;
        campaign.StartedAt = DateTime.UtcNow;
        
        await _repository.UpdateAsync(campaign);

        // Send to queue for processing
        if (_publishEndpoint != null)
        {
            await _publishEndpoint.Publish(new
            {
                CampaignId = campaign.Id,
                Recipients = campaign.Recipients,
                Subject = campaign.Subject,
                Content = campaign.Content
            });
        }
        else
        {
            // Simulate queue processing
            _logger.LogInformation("EMAIL SIMULATION - Campaign: {CampaignName}, Recipients: {RecipientCount}", 
                campaign.Name, campaign.Recipients.Count);
        }

        _logger.LogInformation("Campaign started: {CampaignId}", id);
        return Ok();
    }


    [HttpGet("stats")]
    public async Task<ActionResult> GetStats()
    {
        var campaigns = await _repository.GetAllAsync();
        var campaignsList = campaigns.ToList();

        var stats = new
        {
            TotalCampaigns = campaignsList.Count,
            DraftCampaigns = campaignsList.Count(c => c.Status == CampaignStatus.Draft),
            CompletedCampaigns = campaignsList.Count(c => c.Status == CampaignStatus.Completed),
            TotalEmailsSent = campaignsList.Sum(c => c.SentCount)
        };

        return Ok(stats);
    }
}