using Azure.Data.Tables;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AzureStorageTableCoreLogger.Tests
{
    [TestClass]
    public class TestAzureStorageTableCoreLogger
    {
        [TestMethod]
        public void TestLogging()
        {
            Logger log = new Logger("UseDevelopmentStorage=true", "TestTable", "TestKey");

            log.Debug("DEBUG");
            log.Info("INFO");
            try
            {
                int a = 0;
                int b = 0;
                int c = a / b;
            }
            catch (Exception e)
            {
                log.Error(e, "ERROR");
                log.Error(e);
            }
        }

        [TestMethod]
        public void TestInstanceCloudTableLogging()
        {
            var tableClient = new TableClient("UseDevelopmentStorage=true", "TestTable");
            tableClient.CreateIfNotExists();

            Logger log = new Logger(tableClient, "TestKey");

            log.Debug("TestInstanceCloudTableLogging-DEBUG");
            log.Info("TestInstanceCloudTableLogging-INFO");
            try
            {
                int a = 0;
                int b = 0;
                int c = a / b;
            }
            catch (Exception e)
            {
                log.Error(e, "TestInstanceCloudTableLogging-ERROR");
                log.Error(e);
            }
        }

    }
}
