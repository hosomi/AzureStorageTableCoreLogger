using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Configuration;
using System;

namespace AzureStorageTableCoreLogger
{
    public class ConfigrationUtil
    {
        /// <summary>
        /// キー / 値ベースの構成設定を構築する。
        /// </summary>
        /// <param name="jsonFile">構成設定のファイル名。</param>
        /// <returns>構築した構成設定のインスタンス。</returns>
        public IConfigurationRoot GetConfigrationBuilder(string jsonFile = "local.settings.json")
        {
            if (string.IsNullOrEmpty(jsonFile))
            {
                throw new ArgumentException("設定ファイル名に NULL または空は渡せません。");
            }

            var config = new ConfigurationBuilder().AddJsonFile(jsonFile, true).AddEnvironmentVariables();
            return config?.Build();
        }


        /// <summary>
        /// ストレージアカウントへの参照を取得する。
        /// </summary>
        /// <param name="storageConnectionString">未指定はエミュレータへの接続、ローカルテスト用。</param>
        /// <returns>ストレージアカウントの参照。</returns>
        public static CloudStorageAccount GetCloudStorageAccount(string storageConnectionString = "UseDevelopmentStorage=true")
        {
            return CloudStorageAccount.Parse(storageConnectionString);
        }
    }

}
