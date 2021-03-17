using NUnit.Framework;
using System;

namespace Intercom.Integration.Tests
{
    [TestFixture()]
    public class TestBase
    {
        public string AppId;
        public string AppKey;

        public void CheckForApiCredentials()
        {
            this.AppKey = Environment.GetEnvironmentVariable("IntercomAppKey");
            this.AppId = Environment.GetEnvironmentVariable("IntercomAppId");

            if (string.IsNullOrEmpty(AppKey) || string.IsNullOrEmpty(AppId))
                Assert.Ignore("Intercom.Integration.Tests will be ignored because there are no environment variables defined for IntercomAppKey or IntercomAppId.");
        }

        public TestBase()
        {
        }
    }
}