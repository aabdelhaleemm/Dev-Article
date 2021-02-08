using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface ICacheService
    {
        Task<string> GetCacheValueAsync(string key);
        Task SetCacheValueAsync(string key, string value);
        Task<bool> DeleteKeyAsync(string key);
    }
}