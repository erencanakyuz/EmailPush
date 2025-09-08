using EmailPush.Domain.Entities;
using EmailPush.Domain.Enums;

namespace EmailPush.Domain.Interfaces;

public interface ICampaignRepository : IGenericRepository<Campaign>
{
    Task<Campaign?> GetWithRecipientsAsync(Guid id);
}