using NUnit.Framework;
using System.Collections.Generic;
using TestTaskProject.Common.Models;

namespace TestTaskProject.Tests
{
    /// <summary>
    /// Тестирование сущности Sale
    /// </summary>
    public class TestSaleEntity
    {
        [SetUp]
        public void Setup()
        {

        }

        /// <summary>
        /// Случай, в котором у объекта нет данных SaleData
        /// </summary>
        [Test]
        public void TestNullSaleData()
        {
            var sale = new Sale();
            Assert.AreEqual(sale.TotalAmount, 0);
        }

        /// <summary>
        /// Случай, в котором коллекция с данными была создана, но не содержит объектов
        /// </summary>
        [Test]
        public void TestZeroSaleData()
        {
            var sale = new Sale()
            {
                SaleData = new List<SaleData>()
            };

            Assert.AreEqual(sale.TotalAmount, 0);
        }

        /// <summary>
        /// Случай, в котором у объекта есть все данные, для корректной работы логики
        /// </summary>
        [Test]
        public void TestGoodInputData()
        {
            var sale = new Sale()
            {
                SaleData = new List<SaleData>()
                {
                    new SaleData()
                    {
                        Product = new () { Price = 100 },
                        ProductQuantity = 10
                    }
                }
            };

            Assert.AreEqual(sale.TotalAmount, 1000);
        }
    }
}
