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
            Browser.InitializeDriver();
        }

        [TearDown]
        public virtual void After()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
                ScreenShot.TakeAndAttachScreenShot(TestContext.CurrentContext.Test.MethodName);
            Browser.Cleanup();
        }
    }
}
