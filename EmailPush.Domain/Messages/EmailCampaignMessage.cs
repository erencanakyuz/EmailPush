namespace EmailPush.Domain.Messages;

public class EmailCampaignMessage
{
    // applications aras� bi transfer Data transfer objecti,
    //Rabbit MQ ya haz�rl�k i�in workerle iletii�im kurar.
    
    public Guid CampaignId { get; set; }
    public List<string> Recipients { get; set; } = new();
    public string Subject { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
}