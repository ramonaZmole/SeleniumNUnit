using System.Reflection;
using NUnit.Framework.Interfaces;
using RestSharp;

[assembly: Parallelizable(ParallelScope.All)]
[assembly: LevelOfParallelism(4)]
namespace SeleniumNUnit.Helpers;

public class BaseTest
{
    public TestContext TestContext { get; set; }
    public readonly RestClient Client = RequestHelper.GetRestClient(Constants.Url);

    [SetUp]
    public virtual void Before()
    {
        SetClientToken();
        Browser.InitializeDriver(new DriverOptions
        {
            IsHeadless = true,
            ChromeDriverPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)
        });
    }

    [TearDown]
    public virtual void After()
    {
        if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
        {
            var path = ScreenShot.GetScreenShotPath(TestContext.CurrentContext.Test.MethodName);
            TestContext.AddTestAttachment(path);
        }
        Browser.Cleanup();
    }

    private void SetClientToken()
    {
        var token = Client.GetLoginToken();
        Client.AddDefaultHeader("cookie", $"token={token}");
    }
}