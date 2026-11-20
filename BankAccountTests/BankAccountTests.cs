using Xunit;
using BankAccountApp;
using System;

namespace BankAccountTests
{
    public class BankAccountTests
    {
        [Theory]
        [InlineData("John Doe", 1000, 200, 800)] // EP: Normal valid debit
        [InlineData("Jane Smith", 500, 100, 400)] // EP: Normal valid debit
        [InlineData("Alice Johnson", 300, 50, 250)] // EP: Normal valid debit
        [InlineData("Boundary Test Exact", 500, 500, 0)] // BVA: Exact balance boundary
        [InlineData("Boundary Test Min", 500, 1, 499)] // BVA: Minimum debit boundary
        public void Debit_ValidAmount_UpdatesBalance(string customerName, decimal initialBalance, decimal debitAmount, decimal expectedBalance)
        {
            // Arrange
            var account = new BankAccount(customerName, initialBalance);

            // Act
            account.Debit(debitAmount);

            // Assert
            Assert.Equal(expectedBalance, account.Balance);
        }

        [Theory]
        [InlineData("Bob Brown", 100, 150)] // EP: Debit exceeds balance
        [InlineData("Boundary Over", 100, 100.01)] // BVA: Just above balance boundary
        public void Debit_InsufficientFunds_ThrowsException(string customerName, decimal initialBalance, decimal debitAmount)
        {
            // Arrange
            var account = new BankAccount(customerName, initialBalance);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => account.Debit(debitAmount));
        }

        [Theory]
        [InlineData("John Doe", 1000, 200, 1200)] // EP: Normal valid credit
        [InlineData("Jane Smith", 500, 100, 600)] // EP: Normal valid credit
        [InlineData("Alice Johnson", 300, 50, 350)] // EP: Normal valid credit
        [InlineData("Boundary Zero", 300, 0, 300)] // BVA: Zero credit boundary
        [InlineData("Boundary Min", 300, 1, 301)] // BVA: Minimum credit boundary
        public void Credit_ValidAmount_UpdatesBalance(string customerName, decimal initialBalance, decimal creditAmount, decimal expectedBalance)
        {
            // Arrange
            var account = new BankAccount(customerName, initialBalance);

            // Act
            account.Credit(creditAmount);

            // Assert
            Assert.Equal(expectedBalance, account.Balance);
        }

        [Theory]
        [InlineData("John Doe", 1000, 200, 800)] // EP: Normal valid withdraw
        [InlineData("Jane Smith", 500, 100, 400)] // EP: Normal valid withdraw
        [InlineData("Alice Johnson", 300, 50, 250)] // EP: Normal valid withdraw
        [InlineData("Boundary Exact Withdraw", 500, 500, 0)] // BVA: Withdraw exact balance
        [InlineData("Boundary Min Withdraw", 500, 1, 499)] // BVA: Minimum withdraw boundary
        public void Withdraw_ValidAmount_UpdatesBalance(string customerName, decimal initialBalance, decimal withdrawAmount, decimal expectedBalance)
        {
            // Arrange
            var account = new BankAccount(customerName, initialBalance);

            // Act
            account.Withdraw(withdrawAmount);

            // Assert
            Assert.Equal(expectedBalance, account.Balance);
        }

        [Theory]
        [InlineData("Bob Brown", 100, 150)] // EP: Withdraw exceeds balance
        [InlineData("Boundary Over Withdraw", 100, 100.01)] // BVA: Withdraw just above balance boundary
        public void Withdraw_InsufficientFunds_ThrowsException(string customerName, decimal initialBalance, decimal withdrawAmount)
        {
            // Arrange
            var account = new BankAccount(customerName, initialBalance);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => account.Withdraw(withdrawAmount));
        }
    }
}
