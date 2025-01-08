using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumTests.Drivers;
using SeleniumTests.Pages;
using SeleniumTests.Utils;

namespace SeleniumTests.Tests;

[TestFixture]
public class LoginTests
{
    private IWebDriver _driver;
    private LoginPage _loginPage;
    private WebDriverWait _wait;
    private string? _baseUrl;

    [SetUp]
    public void SetUp()
    {
        _baseUrl = ConfigReader.GetValue("BaseUrl");
        _driver = DriverFactory.CreateDriver();
        _driver.Navigate().GoToUrl($"{_baseUrl}/login");
        _loginPage = new LoginPage(_driver);
        _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
    }

    [TearDown]
    public void TearDown()
    {
        _driver.Quit();
        _driver.Dispose();
    }

    [Test]
    public void TestSuccessfulLogin()
    {
        _loginPage.Login("hamada@gmail.com", "hamada2004");
        
        // Thread.Sleep(2000);
        _wait.Until(d => d.Url.Contains("/shop"));
        Assert.That(_driver.Url, Is.EqualTo($"{_baseUrl}/shop"));
    }
    
    [Test]
    public void TestInvalidLogin()
    {
        _loginPage.Login("incorrect@gmail.com", "incorrect");
        
        var errorMessage = _wait.Until(d => d.FindElement(By.ClassName("MuiAlert-message"))).Text;
        Assert.That(errorMessage, Is.EqualTo("Invalid email or password. Please try again."));
    }

    [Test]
    public void TestValidEmailInvalidPassword()
    {
        _loginPage.Login("hamada@gmail.com", "incorrect");
        
        var errorMessage = _wait.Until(d => d.FindElement(By.ClassName("MuiAlert-message"))).Text;
        Assert.That(errorMessage, Is.EqualTo("Invalid email or password. Please try again."));
    }

    [Test]
    public void TestInvalidEmailValidPassword()
    {
        _loginPage.Login("incorrect@gmail.com", "hamada2004");
        
        var errorMessage = _wait.Until(d => d.FindElement(By.ClassName("MuiAlert-message"))).Text;
        Assert.That(errorMessage, Is.EqualTo("Invalid email or password. Please try again."));
    }
}