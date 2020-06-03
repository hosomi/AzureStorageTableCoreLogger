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

    }
}
