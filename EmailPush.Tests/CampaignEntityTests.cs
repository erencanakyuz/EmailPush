using EmailPush.Domain.Entities;
using NUnit.Framework;

namespace EmailPush.Tests;

[TestFixture]
public class CampaignEntityTests
{
    [Test]
    public void TotalRecipients_EmptyRecipientList_ReturnsZero()
    {
        // Arrange
        var campaign = new Campaign
        {
            Recipients = new List<string>()
        };

        // Act
        var result = campaign.TotalRecipients;

        // Assert
        Assert.AreEqual(0, result);
    }

    [Test]
    public void TotalRecipients_WithRecipients_ReturnsCorrectCount()
    {
        // Arrange
        var campaign = new Campaign
        {
            Recipients = new List<string>
            {
                "user1@example.com",
                "user2@example.com",
                "user3@example.com"
            }
        };

        // Act
        var result = campaign.TotalRecipients;

        // Assert
        Assert.AreEqual(3, result);
    }

    [Test]
    public void TotalRecipients_SingleRecipient_ReturnsOne()
    {
        // Arrange
        var campaign = new Campaign
        {
            Recipients = new List<string> { "user@example.com" }
        };

        // Act
        var result = campaign.TotalRecipients;

        // Assert
        Assert.AreEqual(1, result);
    }

    [Test]
    public void Campaign_DefaultValues_AreSetCorrectly()
    {
        // Arrange & Act
        var campaign = new Campaign();

        // Assert
        Assert.AreEqual(Guid.Empty, campaign.Id);
        Assert.AreEqual(string.Empty, campaign.Name);
        Assert.AreEqual(string.Empty, campaign.Subject);
        Assert.AreEqual(string.Empty, campaign.Content);
        Assert.IsNotNull(campaign.Recipients);
        Assert.IsEmpty(campaign.Recipients);
        Assert.AreEqual(CampaignStatus.Draft, campaign.Status);
        Assert.AreEqual(0, campaign.SentCount);
        Assert.IsNull(campaign.StartedAt);
    }

    [Test]
    public void Campaign_SetProperties_WorksCorrectly()
    {
        // Arrange
        var campaignId = Guid.NewGuid();
        var campaignName = "Test Campaign";
        var subject = "Test Subject";
        var content = "Test Content";
        var recipients = new List<string> { "test@example.com" };
        var createdAt = DateTime.UtcNow;

        // Act
        var campaign = new Campaign
        {
            Id = campaignId,
            Name = campaignName,
            Subject = subject,
            Content = content,
            Recipients = recipients,
            Status = CampaignStatus.Ready,
            CreatedAt = createdAt,
            StartedAt = createdAt,
            SentCount = 5
        };

        // Assert
        Assert.AreEqual(campaignId, campaign.Id);
        Assert.AreEqual(campaignName, campaign.Name);
        Assert.AreEqual(subject, campaign.Subject);
        Assert.AreEqual(content, campaign.Content);
        Assert.AreEqual(recipients, campaign.Recipients);
        Assert.AreEqual(CampaignStatus.Ready, campaign.Status);
        Assert.AreEqual(createdAt, campaign.CreatedAt);
        Assert.AreEqual(createdAt, campaign.StartedAt);
        Assert.AreEqual(5, campaign.SentCount);
    }
}