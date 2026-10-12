using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using SeleniumNUnit.Helpers;

namespace SeleniumNUnit.Pages;

public class CalendarPage : BasePage
{
    public void SelectDates()
    {
        WaitHelpers.WaitForDocumentReadyState();
        var actions = new Actions(Browser.WebDriver);

        actions.ClickAndHold(Browser.WebDriver.FindElement(By.XPath($"(//div[contains(@class, 'rbc-date-cell') and .//button/text()='{Constants.BookingStartDay}'])")))
            .MoveByOffset(10, 10)
            .Release(Browser.WebDriver.FindElement(By.XPath($"(//div[contains(@class, 'rbc-date-cell') and .//button/text()='{Constants.BookingEndDay}'])")))
            .Build()
            .Perform();
    }
}