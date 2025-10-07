namespace Core.Interfaces
{
    public interface IApiKeyService
    {
        Task<string> GenerateApiKeyAsync();
    }
}
