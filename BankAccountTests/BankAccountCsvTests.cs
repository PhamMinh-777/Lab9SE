using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Xunit;
using BankAccountApp;

namespace BankAccountTests
{
    public class BankAccountCsvTests
    {
        public static IEnumerable<object[]> GetTestData()
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "BankAccountTestData.csv");
            if (!File.Exists(filePath))
            {
                filePath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\BankAccountTestData.csv"));
            }
            if (!File.Exists(filePath))
            {
                filePath = "BankAccountTestData.csv";
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
                    yield return new object[] {
                        record.CustomerName,
                        record.InitialBalance,
                        record.DebitAmount,
                        record.ExpectedBalance
                    };
                }
            }
        }

        [Theory]
        [MemberData(nameof(GetTestData))]
        public void Debit_CsvData_UpdatesBalance(string customerName, decimal initialBalance, decimal debitAmount, string expectedBalance)
        {
            // Arrange
            var account = new BankAccount(customerName, initialBalance);

            // Act & Assert
            if (expectedBalance.Trim().Equals("Insufficient funds", StringComparison.OrdinalIgnoreCase))
            {
                Assert.Throws<InvalidOperationException>(() => account.Debit(debitAmount));
            }
            else
            {
                account.Debit(debitAmount);
                Assert.Equal(decimal.Parse(expectedBalance.Trim()), account.Balance);
            }
        }

        public class TestData
        {
            [CsvHelper.Configuration.Attributes.Name("customerName", "CustomerName")]
            public string CustomerName { get; set; } = string.Empty;

            [CsvHelper.Configuration.Attributes.Name("initialBalance", "InitialBalance")]
            public decimal InitialBalance { get; set; }

            [CsvHelper.Configuration.Attributes.Name("debitAmount", "DebitAmount")]
            public decimal DebitAmount { get; set; }

            [CsvHelper.Configuration.Attributes.Name("expectedBalance", "ExpectedBalance")]
            public string ExpectedBalance { get; set; } = string.Empty;
        }
    }
}
