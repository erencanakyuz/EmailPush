namespace EmailPush.Domain.Entities;

public class Campaign
{

    //Domain katmanı, Projenin çekirdeği, Entity'ler burada bulunur,Core Layer
    // Interfaces, Entities, ValueObjects, Enums, gibi yapılar burada bulunur
    //teknolojiden bağımsızdır, verileri içerir
 
    public Guid Id { get; set; } // kimlik,bir entityde her şey aynı olsa bile
    // ID farklı olur ve bunlar farklı nesneler olurlar
    //Swaggerda ID özelinde draft yaparız, gönderim yaparız işimizi aşırı kolaylaştır

    public string Name { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public List<string> Recipients { get; set; } = new();
    public CampaignStatus Status { get; set; } // private set; ile dışardan değiştirmeyi engelleyebiliriz
    // burda mesela bir kampanyanın alıcı sayısı negatif olamaz diye kural koyabiliriz
    // bu da domaindeki bussines logic olurdu,
    //değişmez core logicler burda olur, Diğer tüm işleyiş Application katmanında olur
    public DateTime CreatedAt { get; set; }
    public DateTime? StartedAt { get; set; }
    public int TotalRecipients => Recipients.Count;
    public int SentCount { get; set; }
}

public enum CampaignStatus
{
    Draft,
    Ready,
    Sending,
    Completed,
    Failed
}