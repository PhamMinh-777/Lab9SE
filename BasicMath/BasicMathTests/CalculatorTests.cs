using BasicMath;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace BasicMathTests
{
    [TestClass]
    public class CalculatorTests
    {
        [TestMethod]
        [DataRow(1, 2, 3)]
        [DataRow(-1, -2, -3)]
        [DataRow(0, 0, 0)]
        [DataRow(int.MaxValue, 1, (double)int.MaxValue + 1)]
        public void Add_MultipleValues_ReturnsCorrectSum(int a, int b, double expected)
        {
            var calculator = new BasicMaths();
            double result = calculator.Add(a, b);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        [DataRow(5, 3, 2)]
        [DataRow(-5, -3, -2)]
        [DataRow(0, 0, 0)]
        [DataRow(int.MinValue, 1, (double)int.MinValue - 1)]
        public void Subtract_MultipleValues_ReturnsCorrectDifference(int a, int b, double expected)
        {
            var calculator = new BasicMaths();
            double result = calculator.Subtract(a, b);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        [DataRow(2, 3, 6)]
        [DataRow(-2, -3, 6)]
        [DataRow(0, 5, 0)]
        [DataRow(int.MaxValue, 1, (double)int.MaxValue)]
        public void Multiply_MultipleValues_ReturnsCorrectProduct(int a, int b, double expected)
        {
            var calculator = new BasicMaths();
            double result = calculator.Multiply(a, b);
            Assert.AreEqual(expected, result);
        }
    }
}