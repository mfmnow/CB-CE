using Mfm.Bc.CurrencyExchanger.Data.Contracts;
using System.Collections.Concurrent;

namespace Mfm.Bc.CurrencyExchanger.Data.Services
{
    public class ExchangeRatesCacheService : IExchangeRatesCacheService
    {
        private readonly ConcurrentDictionary<string, object> _cachedObjects = new ConcurrentDictionary<string, object>();

        public T GetCachedObject<T>(string key)
        {
            object cachedObject;
            _cachedObjects.TryGetValue(key, out cachedObject);
            if(cachedObject == null)
            {
                return default(T);
            }
            return (T)cachedObject;
        }

        public void CreateCacheObject<T>(string key, T objectToCache)
        {
            _cachedObjects.GetOrAdd(key, objectToCache);
        }
    }
}
