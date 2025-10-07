using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Services
{
    public class ApiKeyService : IApiKeyService
    {
        private readonly IConfiguration _config;
        private readonly CurrencyContext _currencyContext;

        public ApiKeyService(IConfiguration config, CurrencyContext currencyContext)
        {
            _config = config;
            _currencyContext = currencyContext;
        }

        public async Task<string> GenerateApiKeyAsync()
        {
            await RemoveExpiredApiKeys();

            var apiKeyEntity = await AddNewApiKeytoDatabase();

            var newApiKey = _currencyContext.ApiKeys.FirstOrDefaultAsync(x => x.Id == apiKeyEntity.Id);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, newApiKey.Id.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = apiKeyEntity.ExpireDate;

            var token = new JwtSecurityToken(
                claims: claims,
                expires: expires,
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        private async Task<ApiKey> AddNewApiKeytoDatabase()
        {
            var apiKeyEntity = new ApiKey
            {
                Id = Guid.NewGuid(),
                ExpireDate = DateTime.Now.AddDays(1),
            };
            await _currencyContext.ApiKeys.AddAsync(apiKeyEntity);
            await _currencyContext.SaveChangesAsync();
            return apiKeyEntity;
        }

        private async Task RemoveExpiredApiKeys()
        {
            var expiredKey = await _currencyContext.ApiKeys.Where(x => x.ExpireDate < DateTime.Now).ToListAsync();

            if (expiredKey.Any())
            {
                _currencyContext.RemoveRange(expiredKey);
                await _currencyContext.SaveChangesAsync();
            }
        }
    }
}
