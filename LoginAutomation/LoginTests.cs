using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Xunit;
using Allure.Xunit.Attributes;
using Allure.Net.Commons;

namespace LoginAutomation
{
    [AllureSuite("Login Suite")]
    [AllureSubSuite("Form Authentication")]
    [AllureFeature("User Login Feature")]
    public class LoginTests
    {
        public static IEnumerable<object[]> GetTestData()
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "LoginTestData.csv");
            if (!File.Exists(filePath))
            {
                filePath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\LoginTestData.csv"));
            }
            if (!File.Exists(filePath))
            {
                filePath = "LoginTestData.csv";
            }

            var config = new CsvHelper.Configuration.CsvConfiguration(CultureInfo.InvariantCulture)
            {
                PrepareHeaderForMatch = args => args.Header.Trim(),
                HeaderValidated = null,
                MissingFieldFound = null
            };

            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, config))
            {
                foreach (var record in csv.GetRecords<TestData>())
                {
                    yield return new object[] { record.Username, record.Password };
                }
            }
        }

        [Theory]
        [MemberData(nameof(GetTestData))]
        [AllureStory("Login With Valid Credentials")]
        [AllureSeverity(SeverityLevel.critical)]
        [AllureTag("Smoke", "Regression", "Selenium")]
        public void Login_WithValidCredentials_ShouldSucceed(string username, string password)
        {
            // Initialize Chrome options
            var options = new ChromeOptions();

            // Check standard Chrome locations
            string localChrome = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"Google\Chrome\Application\chrome.exe");
            string progFilesChrome = @"C:\Program Files\Google\Chrome\Application\chrome.exe";
            string progFilesX86Chrome = @"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe";

            if (File.Exists(localChrome))
            {
                options.BinaryLocation = localChrome;
            }
            else if (File.Exists(progFilesChrome))
            {
                options.BinaryLocation = progFilesChrome;
            }
            else if (File.Exists(progFilesX86Chrome))
            {
                options.BinaryLocation = progFilesX86Chrome;
            }

            //options.AddArgument("--headless=new");
            options.AddArgument("--no-sandbox");
            options.AddArgument("--disable-dev-shm-usage");

            using (IWebDriver driver = new ChromeDriver(options))
            {
                // Navigate to the login page
                string htmlPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "login.html");
                if (!File.Exists(htmlPath))
                {
                    htmlPath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\login.html"));
                }
                Uri fileUri = new Uri(htmlPath);
                driver.Navigate().GoToUrl(fileUri.AbsoluteUri);

                // Find the username and password fields and fill them
                var usernameInput = driver.FindElement(By.Id("username"));
                usernameInput.Clear();
                usernameInput.SendKeys(username);

                var passwordInput = driver.FindElement(By.Id("password"));
                passwordInput.Clear();
                passwordInput.SendKeys(password);

                // Click the login button
                var loginBtn = driver.FindElement(By.Id("loginButton"));
                loginBtn.Click();

                // Add assertions as needed (validate successful login feedback)
                var messageElement = driver.FindElement(By.Id("loginMessage"));
                Assert.NotNull(messageElement);
                Assert.Contains(username, messageElement.Text);

                // Small pause to simulate process as stated in PDF
                System.Threading.Thread.Sleep(10000);
            }
        }

        public class TestData
        {
            [CsvHelper.Configuration.Attributes.Name("username", "Username")]
            public string Username { get; set; } = string.Empty;

            [CsvHelper.Configuration.Attributes.Name("password", "Password")]
            public string Password { get; set; } = string.Empty;
        }
    }
}
