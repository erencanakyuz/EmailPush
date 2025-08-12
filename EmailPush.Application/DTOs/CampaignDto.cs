using System.ComponentModel.DataAnnotations;

namespace EmailPush.Application.DTOs;


/// Campaign Data Transfer Object
/// API den dönen kampanya bilgileri

public class CampaignDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public List<string> Recipients { get; set; } = new();
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? StartedAt { get; set; }
    public int TotalRecipients => Recipients.Count;
    public int SentCount { get; set; }
}


/// Yeni kampanya oluşturma için DTO

public class CreateCampaignDto
{

    /// Kampanya adı (maksimum 200 karakter)
    
    /// <example>Hoş Geldin Kampanyası</example>
    [Required(ErrorMessage = "Kampanya adı zorunludur")]
    [StringLength(200, ErrorMessage = "Kampanya adı maksimum 200 karakter olabilir")]
    public string Name { get; set; } = string.Empty;

   
    /// Email konusu (maksimum 500 karakter)

    [Required(ErrorMessage = "Email konusu zorunludur")]
    [StringLength(500, ErrorMessage = "Email konusu maksimum 500 karakter olabilir")]
    public string Subject { get; set; } = string.Empty;

  
    /// Email içeriği

    /// <example>Merhaba! Sitemize hoş geldiniz. Bu bir test email kampanyasıdır.</example>
    [Required(ErrorMessage = "Email içeriği zorunludur")]
    [MinLength(10, ErrorMessage = "Email içeriği en az 10 karakter olmalıdır")]
    public string Content { get; set; } = string.Empty;

   
    /// Alıcı email adresleri listesi (maksimum 1000 adet)
   
    /// <example>["test@example.com", "user@example.com"]</example>
    [Required(ErrorMessage = "En az bir alıcı gereklidir")]
    [MinLength(1, ErrorMessage = "En az bir alıcı gereklidir")]
    public List<string> Recipients { get; set; } = new();
}


/// Kampanya istatistikleri

public class CampaignStatsDto
{
    public int TotalCampaigns { get; set; }
    public int DraftCampaigns { get; set; }
    public int ReadyCampaigns { get; set; }
    public int SendingCampaigns { get; set; }
    public int CompletedCampaigns { get; set; }
    public int FailedCampaigns { get; set; }
    public int TotalEmailsSent { get; set; }
    public int TotalRecipients { get; set; }
}
