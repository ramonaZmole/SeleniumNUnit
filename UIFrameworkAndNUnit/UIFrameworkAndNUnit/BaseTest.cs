using NsTestFrameworkUI.Helpers;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace UIFrameworkAndNUnit
{
    public class BaseTest
    {
        [SetUp]
        public virtual void Before()
        {
            Browser.InitializeDriver(true);
        }

        [TearDown]
        public virtual void After()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
            {
                var imagePath = ScreenShot.GetScreenShotPath(TestContext.CurrentContext.Test.MethodName);
                TestContext.AddTestAttachment(imagePath);
            }

            Browser.Cleanup();
        }
    }
}
