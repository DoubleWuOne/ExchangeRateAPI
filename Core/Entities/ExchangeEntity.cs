namespace Core.Entities
{
    public class ExchangeEntity
    {
        public required string Currency { get; set; }
        public required string CurrencyDenom { get; set; }
        public DateTime Date { get; set; }
        public required decimal ExchangeRateValue { get; set; }
    }
}
