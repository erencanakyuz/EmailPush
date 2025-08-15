using EmailPush.Domain.Entities;

namespace EmailPush.Domain.Interfaces;

public interface ICampaignRepository : IGenericRepository<Campaign>
{
    Task<IEnumerable<Campaign>> GetActiveCampaignsAsync(); // status filteri ile getirtmek için
    //ready ve sending olanları getirecek 
    // bu lyeni interface lazım, bunuda genericten türetiriz, direkt generice koyamayız her yerde
    //status özelliği yok zorunlu yapamayız,(product veya user gibi)
    Task<Campaign?> GetWithRecipientsAsync(Guid id);
}