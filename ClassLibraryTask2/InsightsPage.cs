using OpenQA.Selenium;

namespace EpamAutomationTests.Pages
{
    public class InsightsPage
    {
        private readonly IWebDriver _driver;

        public InsightsPage(IWebDriver driver)
        {
            _driver = driver;
        }

        public string GetArticleTitleFromCarousel()
        {
            var articleTitle = _driver.FindElement(By.CssSelector("span.font-size-60 .museo-sans-500.gradient-text"));
            return articleTitle.Text;
        }
    }
}