using EmailPush.Application.DTOs;
using EmailPush.Application.Utils;
using EmailPush.Domain.Entities;
using NUnit.Framework;

namespace EmailPush.Tests;

[TestFixture]
public class MapToDtoTests
{
    [Test]
    public void MapToDto_ValidCampaign_MapsAllPropertiesCorrectly()
    {
        // Arrange
        var campaignId = Guid.NewGuid();
        var createdAt = DateTime.UtcNow;
        var startedAt = DateTime.UtcNow.AddMinutes(5);
        var recipients = new List<string> { "user1@example.com", "user2@example.com" };

        var campaign = new Campaign
        {
            Id = campaignId,
            Name = "Test Campaign",
            Subject = "Test Subject",
            Content = "Test Content",
            Recipients = recipients,
            Status = CampaignStatus.Ready,
            CreatedAt = createdAt,
            StartedAt = startedAt,
            SentCount = 10
        };

        // Act
        var dto = CampaignMapper.ToDto(campaign);

        // Assert
        Assert.AreEqual(campaignId, dto.Id);
        Assert.AreEqual("Test Campaign", dto.Name);
        Assert.AreEqual("Test Subject", dto.Subject);
        Assert.AreEqual("Test Content", dto.Content);
        Assert.AreEqual(recipients, dto.Recipients);
        Assert.AreEqual("Ready", dto.Status);
        Assert.AreEqual(createdAt, dto.CreatedAt);
        Assert.AreEqual(startedAt, dto.StartedAt);
        Assert.AreEqual(10, dto.SentCount);
    }

    [TestCase(CampaignStatus.Draft, "Draft", "Draft status mapping")]
    [TestCase(CampaignStatus.Ready, "Ready", "Ready status mapping")]
    [TestCase(CampaignStatus.Sending, "Sending", "Sending status mapping")]
    [TestCase(CampaignStatus.Completed, "Completed", "Completed status mapping")]
    [TestCase(CampaignStatus.Failed, "Failed", "Failed status mapping")]
    public void MapToDto_ShouldMapStatusCorrectly_ForAllStatusTypes(CampaignStatus status, string expectedStatusString, string description)
    {
        // Arrange
        var campaign = new Campaign { Status = status };

        // Act
        var dto = CampaignMapper.ToDto(campaign);

        // Assert
        Assert.That(dto.Status, Is.EqualTo(expectedStatusString), $"Test açıklaması: {description}");
    }

    [Test]
    public void MapToDto_CampaignWithNullStartedAt_MapsNullCorrectly()
    {
        // Arrange
        var campaign = new Campaign
        {
            StartedAt = null
        };

        // Act
        var dto = CampaignMapper.ToDto(campaign);

        // Assert
        Assert.IsNull(dto.StartedAt);
    }

    [Test]
    public void MapToDto_CampaignWithEmptyRecipients_MapsEmptyListCorrectly()
    {
        // Arrange
        var campaign = new Campaign
        {
            Recipients = new List<string>()
        };

        // Act
        var dto = CampaignMapper.ToDto(campaign);

        // Assert
        Assert.IsNotNull(dto.Recipients);
        Assert.IsEmpty(dto.Recipients);
    }

    [Test]
    public void MapToDto_CampaignWithMultipleRecipients_PreservesRecipientsOrder()
    {
        // Arrange
        var recipients = new List<string>
        {
            "first@example.com",
            "second@example.com",
            "third@example.com"
        };

        var campaign = new Campaign
        {
            Recipients = recipients
        };

        // Act
        var dto = CampaignMapper.ToDto(campaign);

        // Assert
        Assert.AreEqual(3, dto.Recipients.Count);
        Assert.AreEqual("first@example.com", dto.Recipients[0]);
        Assert.AreEqual("second@example.com", dto.Recipients[1]);
        Assert.AreEqual("third@example.com", dto.Recipients[2]);
    }
}