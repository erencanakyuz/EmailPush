using EmailPush.Domain.Common;

namespace EmailPush.Domain.Events;

public sealed class CampaignCreatedEvent : DomainEvent
{
    public Guid CampaignId { get; }
    public string CampaignName { get; }
    public int TotalRecipients { get; }

    public CampaignCreatedEvent(Guid campaignId, string campaignName, int totalRecipients)
    {
        CampaignId = campaignId;
        CampaignName = campaignName;
        TotalRecipients = totalRecipients;
    }
}

public sealed class CampaignStartedEvent : DomainEvent
{
    public Guid CampaignId { get; }
    public string CampaignName { get; }
    public int TotalRecipients { get; }
    public DateTime StartedAt { get; }

    public CampaignStartedEvent(Guid campaignId, string campaignName, int totalRecipients, DateTime startedAt)
    {
        CampaignId = campaignId;
        CampaignName = campaignName;
        TotalRecipients = totalRecipients;
        StartedAt = startedAt;
    }
}

public sealed class CampaignCompletedEvent : DomainEvent
{
    public Guid CampaignId { get; }
    public string CampaignName { get; }
    public int TotalRecipients { get; }
    public int SentCount { get; }
    public DateTime CompletedAt { get; }

    public CampaignCompletedEvent(Guid campaignId, string campaignName, int totalRecipients, int sentCount, DateTime completedAt)
    {
        CampaignId = campaignId;
        CampaignName = campaignName;
        TotalRecipients = totalRecipients;
        SentCount = sentCount;
        CompletedAt = completedAt;
    }
}

public sealed class CampaignFailedEvent : DomainEvent
{
    public Guid CampaignId { get; }
    public string CampaignName { get; }
    public int TotalRecipients { get; }
    public int SentCount { get; }
    public string FailureReason { get; }
    public DateTime FailedAt { get; }

    public CampaignFailedEvent(Guid campaignId, string campaignName, int totalRecipients, int sentCount, string failureReason, DateTime failedAt)
    {
        CampaignId = campaignId;
        CampaignName = campaignName;
        TotalRecipients = totalRecipients;
        SentCount = sentCount;
        FailureReason = failureReason;
        FailedAt = failedAt;
    }
}

public sealed class EmailSentEvent : DomainEvent
{
    public Guid CampaignId { get; }
    public string RecipientEmail { get; }
    public DateTime SentAt { get; }
    public bool IsSuccess { get; }
    public string? ErrorMessage { get; }

    public EmailSentEvent(Guid campaignId, string recipientEmail, DateTime sentAt, bool isSuccess, string? errorMessage = null)
    {
        CampaignId = campaignId;
        RecipientEmail = recipientEmail;
        SentAt = sentAt;
        IsSuccess = isSuccess;
        ErrorMessage = errorMessage;
    }
}