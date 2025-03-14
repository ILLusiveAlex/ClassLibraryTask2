using EpamAutomationTests.Pages;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.IO;
using System.Linq;
using System.Threading;

namespace EpamAutomationTests
{
    [TestClass]
    public class EpamTests
    {
        private IWebDriver? driver;
        private WebDriverWait? wait;
        private string downloadPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");


        [TestInitialize]
        public void Setup()
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("start-maximized");
            options.AddArgument("headless");

            driver = new ChromeDriver(options);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        [TestMethod]
        [DataRow("Java")]
        [DataRow("C#")]
        public void ValidateJobSearch(string keyword)
        {
            driver.Navigate().GoToUrl("https://www.epam.com/");

            wait.Until(d => d.FindElement(By.LinkText("Careers"))).Click();

            var keywordField = wait.Until(driver => driver.FindElement(By.Id("new_form_job_search-keyword")));
            keywordField.Clear();
            keywordField.SendKeys(keyword);

            driver.FindElement(By.CssSelector("div.recruiting-search__location")).Click();
            driver.FindElement(By.XPath("//li[contains(text(), 'All Locations')]")).Click();

            driver.FindElement(By.CssSelector("label.recruiting-search__filter-label-23")).Click();

            driver.FindElement(By.ClassName("job-search-button-transparent-23"))?.Click();

            var latestJob = wait.Until(d => d.FindElements(By.ClassName("search-result__item"))).Last();
            latestJob.FindElement(By.CssSelector("div.search-result__item-controls a.search-result__item-apply-23")).Click();

            Assert.IsTrue(driver.PageSource.Contains(keyword), "The job description does not contain the expected keyword.");
        }

        [TestMethod]
        [DataRow("BLOCKCHAIN")]
        [DataRow("Cloud")]
        [DataRow("Automation")]
        public void ValidateGlobalSearch(string searchTerm)
        {
            driver.Navigate().GoToUrl("https://www.epam.com/");

            driver.FindElement(By.ClassName("header-search__button"))?.Click();

            var searchInput = driver.FindElement(By.TagName("input"));
            searchInput.Clear();
            searchInput.SendKeys(searchTerm);

            driver.FindElement(By.CssSelector("span.bth-text-layer"))?.Click();

            var results = wait.Until(d => d.FindElements(By.XPath("//li[@class='search-results__item']/a")))
                            .Select(e => e.Text.ToLower())
                            .ToList();

            Assert.IsTrue(results.All(text => text.Contains(searchTerm.ToLower())), "Not all results contain the expected term.");
        }

        [TestMethod]
        [DataRow("EPAM_Corporate_Overview_Q4FY-2024.pdf")]
        public void ValidateFileDownload(string fileName)
        {
            var homePage = new HomePage(driver, wait);
            driver.Navigate().GoToUrl("https://www.epam.com/");

            homePage.NavigateToAbout();
            homePage.ScrollToEPAMAtAGlance();

            homePage.DownloadButton.Click();

            var filePath = Path.Combine(downloadPath, fileName);
            var timeout = DateTime.Now.AddSeconds(30);
            while (!File.Exists(filePath) && DateTime.Now < timeout)
            {
                Thread.Sleep(1000);
            }

            Assert.IsTrue(File.Exists(filePath), "The file was not downloaded.");
        }

        [TestMethod]
        public void ValidateArticleTitleInCarousel()
        {
            var homePage = new HomePage(driver, wait);
            var insightsPage = new InsightsPage(driver);

            driver.Navigate().GoToUrl("https://www.epam.com/");

            homePage.NavigateToInsights();

            var nextButton = wait.Until(d => d.FindElement(By.CssSelector("button.slider__right-arrow.slider-navigation-arrow")));
            ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollBy(0, 160);");
          
            nextButton.Click();
            Thread.Sleep(1000);  
            nextButton.Click();

           
            string articleTitle = insightsPage.GetArticleTitleFromCarousel();

           
            var readMoreButton = wait.Until(d => d.FindElement(By.CssSelector("a.slider-cta-link")));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", readMoreButton);

           
            var newArticleTitle = driver.FindElement(By.CssSelector("span.museo-sans-light")).Text;
            Assert.AreEqual(articleTitle, newArticleTitle, "The article title does not match.");
        }

        [TestCleanup]
        public void TearDown()
        {
            driver.Quit();
        }
    }
}
