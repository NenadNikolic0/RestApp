using System.Net;
using RestApp.Interface;

namespace RestApp
{
    public class RetryableRestClient : IRestClient
    {
        private readonly IRestClient _restClient;
        private readonly ILogger _logger;
        private readonly int _retryCount;
        private readonly TimeSpan _retryDelay;

        public RetryableRestClient(IRestClient restClient, ILogger logger, int retryCount = 3, TimeSpan? retryDelay = null)
        {
            _restClient = restClient;
            _logger = logger;
            _retryCount = retryCount;
            _retryDelay = retryDelay ?? TimeSpan.FromSeconds(1);
        }

        public async Task<TModel> Get<TModel>(string url)
        {
            return await RetryOperation(() => _restClient.Get<TModel>(url), $"Failed to get data from {url}");
        }

        public async Task<TModel> Put<TModel>(string url, TModel model)
        {
            return await RetryOperation(() => _restClient.Put<TModel>(url, model), $"Failed to edit data");
        }

        public async Task<TModel> Post<TModel>(string url, TModel model)
        {
            return await RetryOperation(() => _restClient.Post<TModel>(url, model), $"Failed to insert data");
        }

        public async Task<TModel> Delete<TModel>(int id)
        {
            return await RetryOperation(() => _restClient.Delete<TModel>(id), $"Failed to delete data with id {id}");
        }

        private async Task<TModel> RetryOperation<TModel>(Func<Task<TModel>> operation, string errorMessage)
        {
            Exception lastException = null;
            bool isLogged = false;

            for (int attempt = 0; attempt < _retryCount; attempt++)
            {
                try
                {
                    return await operation();
                }
                catch (WebException ex)
                {
                    lastException = ex;
                    LogError(ex.Message, isLogged);
                    await Task.Delay(_retryDelay);
                }
                catch (Exception ex)
                {
                    throw new Exception($"{errorMessage} after {_retryCount} attempts");
                }
            }

            LogError(lastException?.Message ?? errorMessage, isLogged);
            return default;
        }

        private void LogError(string message, bool isLogged)
        {
            if (!isLogged)
            {
                _logger.LogError(message);
                isLogged = true;
            }
        }
    }
}