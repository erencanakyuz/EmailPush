using EmailPush.Domain.Entities;
using NUnit.Framework;

namespace EmailPush.Tests;

[TestFixture]
public class CampaignEntityTests
{

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
    public void Campaign_DefaultValues_AreSetCorrectly()
    {
        // Arrange & Act
        var campaign = new Campaign();

        // Assert
        Assert.AreNotEqual(Guid.Empty, campaign.Id); // Id should be auto-generated
        Assert.AreEqual(string.Empty, campaign.Name);
        Assert.AreEqual(string.Empty, campaign.Subject);
        Assert.AreEqual(string.Empty, campaign.Content);
        Assert.IsNotNull(campaign.Recipients);
        Assert.IsEmpty(campaign.Recipients);
        Assert.AreEqual(CampaignStatus.Draft, campaign.Status);
        Assert.AreEqual(0, campaign.SentCount);
        Assert.IsNull(campaign.StartedAt);
    }

}