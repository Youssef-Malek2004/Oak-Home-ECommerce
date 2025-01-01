using OpenQA.Selenium;

namespace SeleniumTests.Pages;

public class LoginPage(IWebDriver driver)
{
    private IWebElement EmailField => driver.FindElement(By.CssSelector("input[type='email']"));
    private IWebElement PasswordField => driver.FindElement(By.CssSelector("input[type='password']"));
    private IWebElement LoginButton => driver.FindElement(By.XPath("//button[text()='Login']"));

    public void Login(string email, string password)
    {
        EmailField.Clear();
        EmailField.SendKeys(email);
        
        PasswordField.Clear();
        PasswordField.SendKeys(password);
        
        LoginButton.Click();
    }
}