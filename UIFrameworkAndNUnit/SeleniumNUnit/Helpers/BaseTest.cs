using System.Reflection;
using NUnit.Framework.Interfaces;
using RestSharp;

[assembly: Parallelizable(ParallelScope.All)]
[assembly: LevelOfParallelism(4)]
namespace SeleniumNUnit.Helpers;

public class BaseTest
{
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
        Browser.WebDriver.Manage().Window.Maximize();
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

    private void SetClientToken()
    {
        var token = Client.GetLoginToken();
        Client.AddDefaultHeader("cookie", $"token={token}");
    }
}