using Azure;
using Azure.Data.Tables;
using System;

namespace AzureStorageTableCoreLogger
{
    /// <summary>
    /// 拡張するログの追加項目を定義。
    /// </summary>
    public class AzureStorageTableEntity : ITableEntity
    {
        public string LogLevel { get; set; } // ログレベル（Debug, Information, Error）。
        public string Message { get; set; } // メッセージ。
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
    }
}
