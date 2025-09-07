using MassTransit;
using EmailPush.Domain.Interfaces;
using EmailPush.Domain.Entities;
using EmailPush.Infrastructure.Messages;

namespace EmailPush.Worker;

public class EmailConsumer : IConsumer<EmailCampaignMessage>
{
    private readonly ICampaignRepository _repository;
    private readonly ILogger<EmailConsumer> _logger;

    public EmailConsumer(ICampaignRepository repository, ILogger<EmailConsumer> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<EmailCampaignMessage> context)
    {
        var message = context.Message;
        _logger.LogInformation($"Starting email campaign: {message.CampaignId}");

        var campaign = await _repository.GetByIdAsync(message.CampaignId);
        if (campaign == null) 
        {
            _logger.LogWarning($"Campaign {message.CampaignId} not found");
            return;
        }

        campaign.Status = CampaignStatus.Sending;
        await _repository.UpdateAsync(campaign);

        // Simulate email sending
        foreach (var recipient in message.Recipients)
        {
            await Task.Delay(100); // Simulation delay
            _logger.LogInformation($"Email sent to: {recipient}");
            
            campaign.SentCount++;
        }

        campaign.Status = CampaignStatus.Completed;
        await _repository.UpdateAsync(campaign);

        _logger.LogInformation($"Campaign {message.CampaignId} completed! Sent {campaign.SentCount} emails.");
    }
}