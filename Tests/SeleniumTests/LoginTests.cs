using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using NUnit.Framework;
using System;

namespace SeleniumTests
{
    [TestFixture]
    public class LoginTests
    {
        private IWebDriver _driver;

        [SetUp]
        public void SetUp()
        {
            _driver = new ChromeDriver();
            _driver.Navigate().GoToUrl("http://localhost:5173/login");
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
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));

            // Wait for and enter email
            var emailField = wait.Until(d => d.FindElement(By.CssSelector("input[type='email']")));
            emailField.SendKeys("hamada@gmail.com");

            // Wait for and enter password
            var passwordField = wait.Until(d => d.FindElement(By.CssSelector("input[type='password']")));
            passwordField.SendKeys("hamada2004");

            // Wait for and click the login button
            var loginButton = wait.Until(d => d.FindElement(By.XPath("//button[text()='Login']")));
            loginButton.Click();

            // Wait for redirection to the shop page
            wait.Until(d => d.Url.Contains("/shop"));

            // Assert that the URL is correct
            Assert.That(_driver.Url, Is.EqualTo("http://localhost:5173/shop"));
        }

        [Test]
        public void TestInvalidLogin()
        {
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));

            // Wait for and enter invalid email
            var emailField = wait.Until(d => d.FindElement(By.CssSelector("input[type='email']")));
            emailField.SendKeys("invalidEmail@example.com");

            // Wait for and enter invalid password
            var passwordField = wait.Until(d => d.FindElement(By.CssSelector("input[type='password']")));
            passwordField.SendKeys("InvalidPassword");

            // Wait for and click the login button
            var loginButton = wait.Until(d => d.FindElement(By.XPath("//button[text()='Login']")));
            loginButton.Click();

            // Wait for error message and assert
            var errorMessage = wait.Until(d => d.FindElement(By.ClassName("MuiAlert-message"))).Text;
            Assert.That(errorMessage, Is.EqualTo("Invalid email or password. Please try again."));
        }

        [Test]
        public void TestValidEmailInvalidPassword()
        {
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));

            // Wait for and enter valid email
            var emailField = wait.Until(d => d.FindElement(By.CssSelector("input[type='email']")));
            emailField.SendKeys("validEmail@example.com");

            // Wait for and enter invalid password
            var passwordField = wait.Until(d => d.FindElement(By.CssSelector("input[type='password']")));
            passwordField.SendKeys("InvalidPassword");

            // Wait for and click the login button
            var loginButton = wait.Until(d => d.FindElement(By.XPath("//button[text()='Login']")));
            loginButton.Click();

            // Wait for error message and assert
            var errorMessage = wait.Until(d => d.FindElement(By.ClassName("MuiAlert-message"))).Text;
            Assert.That(errorMessage, Is.EqualTo("Invalid email or password. Please try again."));
        }

        [Test]
        public void TestInvalidEmailValidPassword()
        {
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));

            // Wait for and enter invalid email
            var emailField = wait.Until(d => d.FindElement(By.CssSelector("input[type='email']")));
            emailField.SendKeys("invalidEmail@example.com");

            // Wait for and enter valid password
            var passwordField = wait.Until(d => d.FindElement(By.CssSelector("input[type='password']")));
            passwordField.SendKeys("ValidPassword123");

            // Wait for and click the login button
            var loginButton = wait.Until(d => d.FindElement(By.XPath("//button[text()='Login']")));
            loginButton.Click();

            // Wait for error message and assert
            var errorMessage = wait.Until(d => d.FindElement(By.ClassName("MuiAlert-message"))).Text;
            Assert.That(errorMessage, Is.EqualTo("Invalid email or password. Please try again."));
        }
    }
}
