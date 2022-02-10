using NUnit.Framework;
using System.Collections.Generic;
using TestTaskProject.Common.Models;

namespace TestTaskProject.Tests
{
    /// <summary>
    /// Тестирование сущности SaleData
    /// </summary>
    public class TestSaleData
    {
        [SetUp]
        public void Setup()
        {

        }

        /// <summary>
        /// Случай, в котором у объекта нет данных о продукте
        /// </summary>
        [Test]
        public void TestNullProduct()
        {
            var sd = new SaleData();
            Assert.AreEqual(sd.ProductIdAmount, 0);
        }

        /// <summary>
        /// Случай, в котором у объекта есть все данные, для корректной работы логики
        /// </summary>
        [Test]
        public void TestGoodInputData()
        {
            var sd = new SaleData()
            {
                Product = new Product() { Price = 100 },
                ProductQuantity = 5
            };

            Assert.AreEqual(sd.ProductIdAmount, 500);
        }
    }
}
