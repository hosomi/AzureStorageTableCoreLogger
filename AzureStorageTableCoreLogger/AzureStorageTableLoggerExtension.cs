using Microsoft.Extensions.Logging;

namespace AzureStorageTableCoreLogger
{

    public static class AzureStorageTableLoggerExtension
    {
        public static ILoggerFactory AddTableStorageTableLoggger(this ILoggerFactory loggerFactory, string storageConnectionString, string storageTableName, string partitionKey)
        {
            loggerFactory.AddProvider(new AzureStorageTableLoggerProvider(storageConnectionString, storageTableName, partitionKey));
            return loggerFactory;
        }
    }
}
