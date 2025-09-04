using Core.Entities;

namespace Core.Interfaces
{
    public interface IExchangeService
    {
        Task<List<ExchangeEntity>?> GetTestData();
    }
}
