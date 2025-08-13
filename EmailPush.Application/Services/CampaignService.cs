using EmailPush.Application.DTOs;
using EmailPush.Domain.Entities;
using EmailPush.Domain.Interfaces;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace EmailPush.Application.Services;


/// Campaign Service Implementation


public class CampaignService : ICampaignService
{
    private readonly ICampaignRepository _repository;
    private readonly IPublishEndpoint? _publishEndpoint;
    private readonly ILogger<CampaignService> _logger;

    public CampaignService(
        ICampaignRepository repository,
        ILogger<CampaignService> logger,
        IPublishEndpoint? publishEndpoint = null)
    {
        _repository = repository;
        _logger = logger;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<CampaignDto?> GetByIdAsync(Guid id)
    {
        var campaign = await _repository.GetByIdAsync(id);
        return campaign != null ? MapToDto(campaign) : null;
    }

    public async Task<List<CampaignDto>> GetAllAsync()
    {
        var campaigns = await _repository.GetAllAsync();
        return campaigns.Select(MapToDto).ToList();
    }

    public async Task<CampaignDto> CreateAsync(CreateCampaignDto dto)
    {
        // Email validation
        var invalidEmails = dto.Recipients.Where(email => !IsValidEmail(email)).ToList();
        if (invalidEmails.Any())
        {
            throw new ArgumentException($"Invalid email addresses: {string.Join(", ", invalidEmails)}");
        }

        var campaign = new Campaign
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            Subject = dto.Subject,
            Content = dto.Content,
            Recipients = dto.Recipients,
            Status = CampaignStatus.Draft,
            CreatedAt = DateTime.UtcNow,
            SentCount = 0
        };

        var created = await _repository.AddAsync(campaign);
        _logger.LogInformation("Yeni kampanya oluşturuldu: {CampaignId} - {CampaignName}", created.Id, created.Name);
        
        return MapToDto(created);
    }

    public async Task<CampaignDto?> UpdateAsync(Guid id, CreateCampaignDto dto)
    {
        var campaign = await _repository.GetByIdAsync(id);
        if (campaign == null)
            return null;

        if (campaign.Status != CampaignStatus.Draft)
        {
            throw new InvalidOperationException("Only draft campaigns can be updated");
        }

        // Email validation
        var invalidEmails = dto.Recipients.Where(email => !IsValidEmail(email)).ToList();
        if (invalidEmails.Any())
        {
            throw new ArgumentException($"Invalid email addresses: {string.Join(", ", invalidEmails)}");
        }

        campaign.Name = dto.Name;
        campaign.Subject = dto.Subject;
        campaign.Content = dto.Content;
        campaign.Recipients = dto.Recipients;

        await _repository.UpdateAsync(campaign);
        _logger.LogInformation("Kampanya güncellendi: {CampaignId} - {CampaignName}", campaign.Id, campaign.Name);

        return MapToDto(campaign);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var campaign = await _repository.GetByIdAsync(id);
        if (campaign == null)
            return false;

        if (campaign.Status != CampaignStatus.Draft)
        {
            throw new InvalidOperationException("Only draft campaigns can be deleted");
        }

        await _repository.DeleteAsync(campaign);
        _logger.LogInformation("Kampanya silindi: {CampaignId} - {CampaignName}", campaign.Id, campaign.Name);

        return true;
    }

    public async Task<bool> StartSendingAsync(Guid id)
    {
        var campaign = await _repository.GetByIdAsync(id);
        if (campaign == null)
            return false;

        if (campaign.Status != CampaignStatus.Draft)
        {
            throw new InvalidOperationException("Only draft campaigns can be started");
        }

        campaign.Status = CampaignStatus.Ready;
        campaign.StartedAt = DateTime.UtcNow;
        await _repository.UpdateAsync(campaign);

        _logger.LogInformation("Kampanya başlatıldı: {CampaignId} - {CampaignName}, Alıcı sayısı: {RecipientCount}", 
            campaign.Id, campaign.Name, campaign.Recipients.Count);

        // RabbitMQ is here we will consider is further use for later
        if (_publishEndpoint != null)
        {
            // await _publishEndpoint.Publish(new EmailCampaignMessage { ... });
        }
        else
        {
            //Console write
            _logger.LogInformation("EMAIL SENDING SIMULATION - Campaign: {CampaignName}, Recipients: {Recipients}", 
                campaign.Name, string.Join(", ", campaign.Recipients));
        }

        return true;
    }

    public async Task<CampaignStatsDto> GetStatsAsync()
    {
        var campaigns = await _repository.GetAllAsync();
        var campaignsList = campaigns.ToList();

        return new CampaignStatsDto
        {
            TotalCampaigns = campaignsList.Count,
            DraftCampaigns = campaignsList.Count(c => c.Status == CampaignStatus.Draft),
            ReadyCampaigns = campaignsList.Count(c => c.Status == CampaignStatus.Ready),
            SendingCampaigns = campaignsList.Count(c => c.Status == CampaignStatus.Sending),
            CompletedCampaigns = campaignsList.Count(c => c.Status == CampaignStatus.Completed),
            FailedCampaigns = campaignsList.Count(c => c.Status == CampaignStatus.Failed),
            TotalEmailsSent = campaignsList.Sum(c => c.SentCount),
            TotalRecipients = campaignsList.Sum(c => c.Recipients.Count)
        };
    }

    private static CampaignDto MapToDto(Campaign campaign)
    {
        return new CampaignDto
        {
            Id = campaign.Id,
            Name = campaign.Name,
            Subject = campaign.Subject,
            Content = campaign.Content,
            Recipients = campaign.Recipients,
            Status = campaign.Status.ToString(),
            CreatedAt = campaign.CreatedAt,
            StartedAt = campaign.StartedAt,
            SentCount = campaign.SentCount
        };
    }

    private static bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }
}
