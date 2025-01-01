using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace SeleniumTests.Utils;

public static class WaitHelper
{
    public static IWebElement WaitForElement(IWebDriver driver, By by, TimeSpan timeout)
    {
        var wait = new WebDriverWait(driver, timeout);
        return wait.Until(d => d.FindElement(by));
    }
}