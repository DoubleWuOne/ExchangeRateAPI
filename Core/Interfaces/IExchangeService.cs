using Core.Entities;

namespace Core.Interfaces
{
    public interface IExchangeService
    {
        Task<List<ExchangeEntity>?> GetTestData();
        Task<List<ExchangeEntity>?> GetData(KeyValuePair<string, string> currencyCodes, DateTime startDate, DateTime endDate);
    }
}
