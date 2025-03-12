using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;

namespace EpamAutomationTests.Pages
{
    public class HomePage
    {
        private readonly IWebDriver _driver;
        private readonly WebDriverWait _wait;

        public HomePage(IWebDriver driver, WebDriverWait wait)
        {
            _driver = driver;
            _wait = wait;
        }

        public void NavigateToAbout()
        {
            _driver.FindElement(By.LinkText("About")).Click();
        }

        public void NavigateToInsights()
        {
            _driver.FindElement(By.LinkText("Insights")).Click();
        }

        public void ScrollToEPAMAtAGlance()
        {
            var element = _driver.FindElement(By.CssSelector(".button__content--desktop"));
            IJavaScriptExecutor js = (IJavaScriptExecutor)_driver;
            js.ExecuteScript("arguments[0].scrollIntoView(true);", element);
        }

        public IWebElement DownloadButton => _driver.FindElement(By.XPath("//span[contains(@class, 'button__content') and contains(text(), 'DOWNLOAD')]"));

        public IWebElement Carousel => _driver.FindElement(By.CssSelector(".carousel"));

        public IWebElement ReadMoreButton => _driver.FindElement(By.CssSelector("div.single-slide__cta-container a.slider-cta-link"));
    }
}
