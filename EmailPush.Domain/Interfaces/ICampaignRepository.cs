using EmailPush.Domain.Entities;

namespace EmailPush.Domain.Interfaces;

public interface ICampaignRepository : IGenericRepository<Campaign>
{
    Task<IEnumerable<Campaign>> GetActiveCampaignsAsync();
    Task<Campaign?> GetWithRecipientsAsync(Guid id);
}