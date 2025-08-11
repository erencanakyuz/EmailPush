using MassTransit;
using EmailPush.Domain.Entities;
using EmailPush.Domain.Interfaces;

namespace EmailPush.Worker.Consumers;

public class EmailCampaignConsumer : IConsumer<EmailCampaignMessage>
{
    private readonly ICampaignRepository _repository;
    private readonly ILogger<EmailCampaignConsumer> _logger;

    public EmailCampaignConsumer(ICampaignRepository repository, ILogger<EmailCampaignConsumer> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<EmailCampaignMessage> context)
    {
        var message = context.Message;
        _logger.LogInformation("Starting email campaign: {CampaignId}", message.CampaignId);

        var campaign = await _repository.GetByIdAsync(message.CampaignId);
        if (campaign == null) 
        {
            _logger.LogWarning("Campaign not found: {CampaignId}", message.CampaignId);
            return;
        }

        // Update status to sending
        campaign.Status = CampaignStatus.Sending;
        await _repository.UpdateAsync(campaign);

        // Simulate email sending
        foreach (var recipient in message.Recipients)
        {
            await Task.Delay(100); // Simulate processing time
            _logger.LogInformation("Email sent to: {Recipient}", recipient);

            campaign.SentCount++;
            await _repository.UpdateAsync(campaign);
        }

        // Mark as completed
        campaign.Status = CampaignStatus.Completed;
        await _repository.UpdateAsync(campaign);

        _logger.LogInformation("Campaign completed: {CampaignId}, Total sent: {SentCount}", 
            campaign.Id, campaign.SentCount);
    }
}

public class EmailCampaignMessage
{
    public Guid CampaignId { get; set; }
    public List<string> Recipients { get; set; } = new();
    public string Subject { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
}
