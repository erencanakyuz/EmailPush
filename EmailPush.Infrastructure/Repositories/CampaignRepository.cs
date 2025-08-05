using EmailPush.Domain.Entities;
using EmailPush.Domain.Interfaces;
using EmailPush.Infrastructure.Data;

namespace EmailPush.Infrastructure.Repositories;

public class CampaignRepository : GenericRepository<Campaign>, ICampaignRepository
{
    public CampaignRepository(ApplicationDbContext context) : base(context)
    {
    }
}