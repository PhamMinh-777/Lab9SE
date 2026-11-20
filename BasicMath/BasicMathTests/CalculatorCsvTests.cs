
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BasicMath;
using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.ComponentModel;

namespace BasicMathTests
{
    [TestClass]
    public class CalculatorCsvTests
    {
        public partial class TestData
        {
            public int A { get; set; }
            public int B { get; set; }
            public double Expected { get; set; }
        }

        public static IEnumerable<object[]> GetTestData(string fileName)
        {
            string projectDirectory = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\.."));
            string filePath = Path.Combine(projectDirectory, "DataSource", fileName);

            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                foreach (var record in csv.GetRecords<TestData>())
                {
                    yield return new object[]
                    {
                        record.A,
                        record.B,
                        record.Expected
                    };
                }
            }
        }

        [TestMethod]
        [DynamicData(nameof(GetAddTestData), DynamicDataSourceType.Method)]
        public void Test_AddCsv(int a, int b, double expected)
        {
            var bm = new BasicMaths();
            Assert.AreEqual(expected, bm.Add(a, b));
        }
        public static IEnumerable<object[]> GetAddTestData() => GetTestData("add_testdata.csv");

        [TestMethod]
        [DynamicData(nameof(GetSubtractTestData), DynamicDataSourceType.Method)]
        public void Test_SubtractCsv(int a, int b, double expected)
        {
            var bm = new BasicMaths();
            Assert.AreEqual(expected, bm.Subtract(a, b));
        }
        public static IEnumerable<object[]> GetSubtractTestData() => GetTestData("subtract_testdata.csv");


        [TestMethod]
        [DynamicData(nameof(GetMultiplyTestData), DynamicDataSourceType.Method)]
        public void Test_MultiplyCsv(int a, int b, double expected)
        {
            var bm = new BasicMaths();
            Assert.AreEqual(expected, bm.Multiply(a, b));
        }
        public static IEnumerable<object[]> GetMultiplyTestData() => GetTestData("multiply_testdata.csv");

        
        [TestMethod]
        [DynamicData(nameof(GetDivideTestData), DynamicDataSourceType.Method)]  
        public void Test_DivideCsv(int a, int b, double expected)
        {
            var bm = new BasicMaths();
            Assert.AreEqual(expected, bm.Divide(a, b));
        }
        public static IEnumerable<object[]> GetDivideTestData() => GetTestData("divide_testdata.csv");

    }
}
 