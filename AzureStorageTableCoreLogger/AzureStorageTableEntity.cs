using Microsoft.WindowsAzure.Storage.Table;

namespace AzureStorageTableCoreLogger
{
    /// <summary>
    /// 拡張するログの追加項目を定義。
    /// </summary>
    public class AzureStorageTableEntity : TableEntity
    {
        public string LogLevel { get; set; } // ログレベル（Debug, Information, Error）。
        public string Message { get; set; } // メッセージ。
    }
}
