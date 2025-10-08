using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using System.Globalization;
using System.Xml.Serialization;

namespace Infrastructure.Services
{
    public class ExchangeService : IExchangeService
    {
        private readonly IHttpClientFactory _factory;
        private readonly CurrencyContext _context;

        public ExchangeService(IHttpClientFactory factory, CurrencyContext context)
        {
            _factory = factory;
            _context = context;
        }

        public async Task<List<ExchangeEntity>?> GetData(KeyValuePair<string, string> currencyCodes, DateTime startDate, DateTime endDate)
        {
            //Validate Date
            var isValidateDate = ValidateDate(startDate, endDate);
            if (!isValidateDate)
                return null;
            CheckDatabase(currencyCodes, startDate, endDate);
            var httpClient = _factory.CreateClient();
            var url = $"https://data-api.ecb.europa.eu/service/data/EXR/D.{currencyCodes.Key}.{currencyCodes.Value}.SP00.A?startPeriod={startDate:yyyy-MM-dd}&endPeriod={endDate:yyyy-MM-dd}";
            //http://localhost:5218/api/exchange?currencyCodes.Key=PLN&currencyCodes.Value=EUR&startDate=2009-02-01&endDate=2009-05-31

            using var response = await httpClient.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();

            var serializer = new XmlSerializer(typeof(GenericData));
            using var reader = new StringReader(content);
            var data = (GenericData)serializer.Deserialize(reader);

            List<ExchangeEntity> entities = MapToExchangeEntities(data);

            foreach (var exchangeEntity in entities)
            {
                CheckIfDateExistInDatabase(exchangeEntity);
                Console.WriteLine($"{exchangeEntity.Currency} to {exchangeEntity.CurrencyDenom} is {exchangeEntity.ExchangeRateValue}");
            }

            return entities;
        }

        private void CheckIfDateExistInDatabase(ExchangeEntity exchangeEntity)
        {
            if (_context.ExchangeEntities.Find(exchangeEntity.Currency, exchangeEntity.CurrencyDenom,
                    exchangeEntity.Date) == null)
            {
                _context.ExchangeEntities.Add(exchangeEntity);
                _context.SaveChanges();
            }
        }

        private void CheckDatabase(KeyValuePair<string, string> currencyCodes, DateTime startDate, DateTime endDate)
        {
            var entities = _context.ExchangeEntities.Where(x => x.Date >= startDate && x.Date <= endDate).Where(y =>
                y.Currency == currencyCodes.Key && y.CurrencyDenom == currencyCodes.Value).ToList();

            Console.WriteLine("FROM DATABASE-------------------");
            foreach (var exchangeEntity in entities)
            {
                Console.WriteLine($"{exchangeEntity.Currency} to {exchangeEntity.CurrencyDenom} is {exchangeEntity.ExchangeRateValue}");
            }
        }

        private bool ValidateDate(DateTime startDate, DateTime endDate)
        {
            if (startDate > DateTime.Now || endDate > DateTime.Now)
                return false;

            return true;
        }

        public async Task<List<ExchangeEntity>?> GetTestData()
        {

            var httpClient = _factory.CreateClient();
            //var url = "https://data-api.ecb.europa.eu/service/data/EXR/D.PLN.EUR.SP00.A?startPeriod=2009-02-02&endPeriod=2009-02-02";
            var url = "https://data-api.ecb.europa.eu/service/data/EXR/D.PLN.EUR.SP00.A?startPeriod=2009-02-01&endPeriod=2009-05-31";

            using var response = await httpClient.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();

            var serializer = new XmlSerializer(typeof(GenericData));
            using var reader = new StringReader(content);
            var data = (GenericData)serializer.Deserialize(reader);

            List<ExchangeEntity> entities = MapToExchangeEntities(data);

            foreach (var exchangeEntity in entities)
            {
                Console.WriteLine($"{exchangeEntity.Currency} to {exchangeEntity.CurrencyDenom} is {exchangeEntity.ExchangeRateValue}");
            }

            return entities;
        }

        private List<ExchangeEntity> MapToExchangeEntities(GenericData data)
        {
            var result = new List<ExchangeEntity>();

            foreach (var series in data.DataSet.Series)
            {
                string currency = series.SeriesKey.Values
                    .FirstOrDefault(v => v.Id == "CURRENCY")?.Value;
                string currencyDenom = series.SeriesKey.Values
                    .FirstOrDefault(v => v.Id == "CURRENCY_DENOM")?.Value;

                foreach (var obs in series.Observations)
                {
                    if (DateTime.TryParse(obs.Dimension.Value, out var date))
                    {
                        decimal exchangeRateValue;
                        if (!decimal.TryParse(obs.Value?.Value, NumberStyles.Any,
                                CultureInfo.InvariantCulture, out exchangeRateValue))
                        {
                            exchangeRateValue = 0m;
                        }
                        result.Add(new ExchangeEntity
                        {
                            Currency = currency,
                            CurrencyDenom = currencyDenom,
                            Date = date,
                            ExchangeRateValue = exchangeRateValue
                        });
                    }
                }
            }

            return result;
        }
    }
}
