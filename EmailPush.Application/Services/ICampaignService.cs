using EmailPush.Application.DTOs;

namespace EmailPush.Application.Services;

/// <summary>
/// Campaign Service Interface
/// Kampanya işlemleri için servis arayüzü
/// </summary>
public interface ICampaignService
{
    /// <summary>
    /// Kampanya ID'sine göre kampanya getirir
    /// </summary>
    Task<CampaignDto?> GetByIdAsync(Guid id);

    /// <summary>
    /// Tüm kampanyaları listeler
    /// </summary>
    Task<List<CampaignDto>> GetAllAsync();

    /// <summary>
    /// Yeni kampanya oluşturur
    /// </summary>
    Task<CampaignDto> CreateAsync(CreateCampaignDto dto);

    /// <summary>
    /// Kampanya günceller (sadece draft durumundaki kampanyalar)
    /// </summary>
    Task<CampaignDto?> UpdateAsync(Guid id, CreateCampaignDto dto);

    /// <summary>
    /// Kampanya siler (sadece draft durumundaki kampanyalar)
    /// </summary>
    Task<bool> DeleteAsync(Guid id);

    /// <summary>
    /// Kampanya gönderimini başlatır
    /// </summary>
    Task<bool> StartSendingAsync(Guid id);

    /// <summary>
    /// Kampanya istatistiklerini getirir
    /// </summary>
    Task<CampaignStatsDto> GetStatsAsync();
}
