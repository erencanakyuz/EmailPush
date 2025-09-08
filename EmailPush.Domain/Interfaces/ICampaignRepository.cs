using EmailPush.Domain.Entities;

namespace EmailPush.Domain.Interfaces;

public interface ICampaignRepository : IGenericRepository<Campaign>
{
    Task<IEnumerable<Campaign>> GetActiveCampaignsAsync(); // status filteri ile getirtmek için
    //ready ve sending olanları getirecek 
    // bu lyeni interface lazım, bunuda genericten türetiriz, direkt generice koyamayız her yerde
    //status özelliği yok zorunlu yapamayız,(product veya user gibi)
    Task<IEnumerable<Campaign>> GetByStatusAsync(CampaignStatus status);
    Task<Campaign?> GetWithRecipientsAsync(Guid id);

    // Pagination support
    Task<IEnumerable<Campaign>> GetPagedByStatusAsync(CampaignStatus status, int pageNumber, int pageSize);
    Task<int> GetCountByStatusAsync(CampaignStatus status);
}