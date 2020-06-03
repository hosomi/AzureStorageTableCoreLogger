using Microsoft.Extensions.Logging;
using System;
using System.Text;

namespace AzureStorageTableCoreLogger
{
    public class Logger
    {
        /// <summary>
        /// ロガーのインスタンス。
        /// </summary>
        private AzureStorageTableLogger Log;


        /// <summary>
        /// ロガーインスタンスを生成。
        /// </summary>
        /// <param name="storageConnectionString">ストレージアカウントの接続文字列。</param>
        /// <param name="storageTableName">ストレージテーブルのテーブル名。</param>
        /// <param name="venreyId">対象アカウント（ストレージテーブルの PartitionKey になる値）。</param>
        /// <param name="baseSeq">オプション：対象 BaseSeq （ストレージテーブルの PartitionKey になる値、キーは venreyId-BaseSeq になります）。</param>
        public Logger(string storageConnectionString, string storageTableName, string partiotionKey)
        {
            SetInstanceAzureStorageTableLogger(storageConnectionString, storageTableName, partiotionKey);
        }

        /// <summary>
        /// デバッグ出力。
        /// </summary>
        /// <param name="message">出力内容。</param>
        public void Debug(string message)
        {
            if (Log == null)
            {
                return;
            }
            Log.LogDebug(message);
        }

        /// <summary>
        /// 情報出力。
        /// </summary>
        /// <param name="message">出力内容。</param>
        public void Info(string message)
        {
            if (Log == null)
            {
                return;
            }
            Log.LogInformation(message);
        }

        /// <summary>
        /// エラー出力。
        /// </summary>
        /// <param name="message">出力内容。</param>
        /// <param name="exception">例外。</param>
        public void Error(Exception exception, string message)
        {
            if (Log == null)
            {
                return;
            }
            Log.LogError($"{message}{Environment.NewLine}{ToString(exception)}");
        }

        /// <summary>
        /// エラー出力。
        /// </summary>
        /// <param name="exception">例外。</param>
        public void Error(Exception exception)
        {
            if (Log == null)
            {
                return;
            }
            Log.LogError(ToString(exception));
        }

        /// <summary>
        /// エラー出力。
        /// </summary>
        /// <param name="exception">例外。</param>
        public void Error(string message)
        {
            if (Log == null)
            {
                return;
            }
            Log.LogError(message);
        }

        /// <summary>
        /// 例外内容を組み立てる。
        /// </summary>
        /// <param name="e">例外。</param>
        /// <returns>例外内容。</returns>
        private string ToStringExceptionMessageStack(Exception e)
        {
            if (e.InnerException != null)
            {
                StringBuilder message = new StringBuilder();
                message.AppendLine(ToStringExceptionMessageStack(e.InnerException));
                message.Append("   ");
                message.AppendLine(e.Message);
                return message.ToString();
            }
            else
            {
                return $"   {e.Message}";
            }
        }

        /// <summary>
        /// 例外の呼び出しコールを文字列にする。
        /// </summary>
        /// <param name="e">例外。</param>
        /// <returns>例外の呼び出しコールの文字列。</returns>
        private string ToStringExceptionCallStack(Exception e)
        {
            if (e.InnerException != null)
            {
                StringBuilder message = new StringBuilder();
                message.AppendLine(ToStringExceptionCallStack(e.InnerException));
                message.AppendLine("--- Next Call Stack:");
                message.AppendLine(e.StackTrace);
                return (message.ToString());
            }
            else
            {
                return e.StackTrace;
            }
        }

        /// <summary>
        /// 例外の種類を文字列にする。
        /// </summary>
        /// <param name="e">例外。</param>
        /// <returns>例外の種類の文字列。</returns>
        private string ToStringExceptionTypeStack(Exception e)
        {
            if (e.InnerException != null)
            {
                StringBuilder message = new StringBuilder();
                message.AppendLine(ToStringExceptionTypeStack(e.InnerException));
                message.Append("   ");
                message.AppendLine(e.GetType().ToString());
                return (message.ToString());
            }
            else
            {
                return $"   {e.GetType()}";
            }
        }

        /// <summary>
        /// 例外を文字列にする。
        /// </summary>
        /// <param name="exception">例外。</param>
        /// <returns>例外の文字列。</returns>
        private string ToString(Exception exception)
        {
            StringBuilder error = new StringBuilder();
            error.AppendLine("Exception classes:   ");
            error.Append(ToStringExceptionTypeStack(exception));
            error.AppendLine("");
            error.AppendLine("Exception messages: ");
            error.Append(ToStringExceptionMessageStack(exception));
            error.AppendLine("");
            error.AppendLine("Stack Traces:");
            error.Append(ToStringExceptionCallStack(exception));
            return error.ToString();
        }

        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        /// <param name="storageConnectionString">ストレージアカウントの接続文字列。</param>
        /// <param name="storageTableName">ストレージテーブルのテーブル名。</param>
        /// <param name="venreyId">対象アカウント（ストレージテーブルの PartitionKey になる値）。</param>
        /// <param name="baseSeq">オプション：対象 BaseSeq （ストレージテーブルの PartitionKey になる値、キーは venreyId-BaseSeq になります）。</param>
        private void SetInstanceAzureStorageTableLogger(string storageConnectionString, string storageTableName, string partiotionKey)
        {
            try
            {
                this.Log = new AzureStorageTableLogger(storageConnectionString, storageTableName, partiotionKey);
            }
            catch (Exception e)
            {
                this.Log = null;
                this.exception = e;
                Console.WriteLine(ToString(e));
            }
        }

        private Exception exception;

        /// <summary>
        /// コンストラクタ初期化のエラー情報。
        /// </summary>
        /// <returns></returns>
        public Exception GetException()
        {
            return this.exception;
        }
    }

}
