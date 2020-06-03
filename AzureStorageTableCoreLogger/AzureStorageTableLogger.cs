using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;
using System;


namespace AzureStorageTableCoreLogger
{
    /// <summary>
    /// Extensions.Logging の AzureStorageTable 実装。
    /// </summary>
    public class AzureStorageTableLogger : ILogger
    {
        /// <summary>
        /// <seealso cref="CloudTable"/>
        /// </summary>
        private readonly CloudTable cloudTableLogging;

        /// <summary>
        /// <see cref="TableEntity.PartitionKey"/>
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
        public AzureStorageTableLogger(CloudTable cloudTableLogging, string partitionKey)
        {
            this.partitionKey = partitionKey;
            this.cloudTableLogging = cloudTableLogging;
        }

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="storageConnectionString">ストレージアカウントの接続文字列。</param>
        /// <param name="storageTableName">ストレージテーブルのテーブル名。</param>
        /// <param name="partitionKey"><see cref="TableEntity.PartitionKey"/></param>
        public AzureStorageTableLogger(string storageConnectionString, string storageTableName, string partitionKey)
        {
            this.partitionKey = partitionKey;
            var storageAccount = ConfigrationUtil.GetCloudStorageAccount(storageConnectionString);
            var cloudTableClient = storageAccount.CreateCloudTableClient();
            cloudTableLogging = cloudTableClient.GetTableReference(storageTableName);
            cloudTableLogging.CreateIfNotExistsAsync().Wait();
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            var logEntity = new AzureStorageTableEntity
            {
                PartitionKey = this.partitionKey,
                RowKey = Guid.NewGuid().ToString(),
                LogLevel = Enum.ToObject(typeof(Log4LogLevel), (int)logLevel).ToString(),
                Message = formatter(state, exception),
            };

            TableOperation toInsert = TableOperation.Insert(logEntity);
            cloudTableLogging.ExecuteAsync(toInsert).GetAwaiter().GetResult();
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }
    }

}
