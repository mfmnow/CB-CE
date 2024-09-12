namespace Mfm.Bc.CurrencyExchanger.Data.Contracts
{
    public interface IExchangeRatesCacheService
    {
        T GetCachedObject<T>(string key);
        void CreateCacheObject<T>(string key, T objectToCache);
    }
}
