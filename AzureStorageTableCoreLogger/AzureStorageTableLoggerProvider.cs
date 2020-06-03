using Microsoft.Extensions.Logging;

namespace AzureStorageTableCoreLogger
{
    public class AzureStorageTableLoggerProvider : ILoggerProvider
    {
        /// <summary>
        /// 接続文字列。
        /// </summary>
        private readonly string storageConnectionString;

        /// <summary>
        /// AzureStorageTable のテーブル名。
        /// </summary>
        private readonly string storageTableName;

        /// <summary>
        /// AzureStorageTable の PartitionKey。
        /// </summary>

        private readonly string partitionKey;

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="storageConnectionString">ストレージアカウントの接続文字列。</param>
        /// <param name="storageTableName">ストレージテーブルのテーブル名。</param>
        /// <param name="partitionKey"><see cref="TableEntity.PartitionKey"/></param>
        public AzureStorageTableLoggerProvider(string storageConnectionString, string storageTableName, string partitionKey)
        {
            this.storageConnectionString = storageConnectionString;
            this.storageTableName = storageTableName;
            this.partitionKey = partitionKey;
        }
        public ILogger CreateLogger(string categoryName)
        {
            return new AzureStorageTableLogger(storageConnectionString, storageTableName, partitionKey);
        }

        public void Dispose()
        {

        }
    }

}
