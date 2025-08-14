using EmailPush.Application.DTOs;
using EmailPush.Application.Utils;
using EmailPush.Domain.Entities;
using NUnit.Framework;

namespace EmailPush.Tests;

[TestFixture]
public class CampaignMapperTests
{
    [Test]
    public void ToDto_ValidCampaign_MapsAllPropertiesCorrectly()
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


    [Test]
    public void FromCreateDto_ValidDto_CreatesCampaignWithCorrectDefaults()
    {
        // Arrange
        var dto = new CreateCampaignDto
        {
            Name = "Test Campaign",
            Subject = "Test Subject", 
            Content = "Test Content",
            Recipients = new List<string> { "test@example.com" }
        };

        // Act
        var campaign = CampaignMapper.FromCreateDto(dto);

        // Assert
        Assert.AreNotEqual(Guid.Empty, campaign.Id);
        Assert.AreEqual("Test Campaign", campaign.Name);
        Assert.AreEqual("Test Subject", campaign.Subject);
        Assert.AreEqual("Test Content", campaign.Content);
        Assert.AreEqual(dto.Recipients, campaign.Recipients);
        Assert.AreEqual(CampaignStatus.Draft, campaign.Status);
        Assert.AreEqual(0, campaign.SentCount);
        Assert.IsNull(campaign.StartedAt);
        Assert.That(campaign.CreatedAt, Is.EqualTo(DateTime.UtcNow).Within(TimeSpan.FromSeconds(1)));
    }


    [Test]
    public void UpdateFromDto_ValidInputs_UpdatesCampaignProperties()
    {
        // Arrange
        var existingCampaign = new Campaign
        {
            Id = Guid.NewGuid(),
            Name = "Old Name",
            Subject = "Old Subject",
            Content = "Old Content", 
            Recipients = new List<string> { "old@example.com" },
            Status = CampaignStatus.Draft,
            CreatedAt = DateTime.UtcNow.AddDays(-1),
            SentCount = 0
        };

        var updateDto = new CreateCampaignDto
        {
            Name = "New Name",
            Subject = "New Subject",
            Content = "New Content",
            Recipients = new List<string> { "new@example.com", "new2@example.com" }
        };

        var originalId = existingCampaign.Id;
        var originalCreatedAt = existingCampaign.CreatedAt;
        var originalStatus = existingCampaign.Status;

        // Act
        CampaignMapper.UpdateFromDto(existingCampaign, updateDto);

        // Assert - Updated properties
        Assert.AreEqual("New Name", existingCampaign.Name);
        Assert.AreEqual("New Subject", existingCampaign.Subject);
        Assert.AreEqual("New Content", existingCampaign.Content);
        Assert.AreEqual(updateDto.Recipients, existingCampaign.Recipients);

        // Assert - Unchanged properties
        Assert.AreEqual(originalId, existingCampaign.Id);
        Assert.AreEqual(originalCreatedAt, existingCampaign.CreatedAt);
        Assert.AreEqual(originalStatus, existingCampaign.Status);
    }


}