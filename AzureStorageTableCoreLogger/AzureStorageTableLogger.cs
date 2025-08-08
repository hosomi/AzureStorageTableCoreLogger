using Azure.Data.Tables;
using Microsoft.Extensions.Logging;
using System;

namespace AzureStorageTableCoreLogger
{
    /// <summary>
    /// Extensions.Logging の AzureStorageTable 実装。
    /// </summary>
    public class AzureStorageTableLogger : ILogger
    {
        /// <summary>
        /// <seealso cref="TableClient"/>
        /// </summary>
        private readonly TableClient cloudTableLogging;

        /// <summary>
        /// <see cref="ITableEntity.PartitionKey"/>
        /// </summary>
        private readonly string partitionKey;

        /// <summary>
        /// デフォルトコンストラクタは使用不可。
        /// </summary>
        private AzureStorageTableLogger() { }

        /// <summary>
        /// コンストラクタ。
        /// （テーブルを直接指定、テーブルの存在チェックと作成は行いません。）
        /// </summary>
        /// <param name="cloudTableLogging">ログの書き込み先</param>
        /// <param name="partitionKey"></param>
        public AzureStorageTableLogger(TableClient cloudTableLogging, string partitionKey)
        {
            this.partitionKey = partitionKey;
            this.cloudTableLogging = cloudTableLogging;
        }

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="storageConnectionString">ストレージアカウントの接続文字列。</param>
        /// <param name="storageTableName">ストレージテーブルのテーブル名。</param>
        /// <param name="partitionKey"><see cref="ITableEntity.PartitionKey"/></param>
        public AzureStorageTableLogger(string storageConnectionString, string storageTableName, string partitionKey)
        {
            this.partitionKey = partitionKey;
            cloudTableLogging = new TableClient(storageConnectionString, storageTableName);
            cloudTableLogging.CreateIfNotExists();
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {

            var log4LogLevel = ConvertLogLevel(logLevel);
            if (!IsEnabled(log4LogLevel))
            {
                return;
            }

            var logEntity = new AzureStorageTableEntity
            {
                PartitionKey = this.partitionKey,
                RowKey = Guid.NewGuid().ToString(),
                LogLevel = log4LogLevel.ToString(),
                Message = formatter(state, exception),
            };

            cloudTableLogging.AddEntityAsync(logEntity).GetAwaiter().GetResult();
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            //var convertLogLevel = ConvertLogLevel(logLevel);
            //return IsEnabled(convertLogLevel);
            return true;
        }

        private bool IsEnabled(Log4LogLevel logLevel)
        {
            return true;
        }

        /// <summary>
        /// Convert log level to Log4 variant.
        /// </summary>
        /// <param name="logLevel">level to be converted.</param>
        /// <returns></returns>
        private static Log4LogLevel ConvertLogLevel(LogLevel logLevel)
        {
            switch (logLevel)
            {
                case Microsoft.Extensions.Logging.LogLevel.Trace:
                    return Log4LogLevel.TRACE;
                case Microsoft.Extensions.Logging.LogLevel.Debug:
                    return Log4LogLevel.DEBUG;
                case Microsoft.Extensions.Logging.LogLevel.Information:
                    return Log4LogLevel.INFO;
                case Microsoft.Extensions.Logging.LogLevel.Warning:
                    return Log4LogLevel.WARN;
                case Microsoft.Extensions.Logging.LogLevel.Error:
                    return Log4LogLevel.ERROR;
                case Microsoft.Extensions.Logging.LogLevel.Critical:
                    return Log4LogLevel.FATAL;
                case Microsoft.Extensions.Logging.LogLevel.None:
                    return Log4LogLevel.NONE;
                default:
                    return Log4LogLevel.DEBUG;
            }
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }
    }

}
