using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class CurrencyContext : DbContext
    {
        public CurrencyContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<ExchangeEntity> ExchangeEntities { get; set; }
        public DbSet<ApiKey> ApiKeys { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ExchangeEntity>().HasKey(k => new { k.Currency, k.CurrencyDenom, k.Date });
            modelBuilder.Entity<ExchangeEntity>().Property(x => x.ExchangeRateValue).HasColumnType("decimal(18,4)");
        }
    }
}
