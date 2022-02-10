using NUnit.Framework;
using System;
using System.Collections.Generic;
using TestTaskProject.Common.BusinessLogic;
using TestTaskProject.Common.Models;
using System.Linq;

namespace TestTaskProject.Tests
{
    /// <summary>
    /// Тестирование бизнес логики
    /// </summary>
    public class TestBusinessLogic
    {
        private SalesManager _salesManager;
        private List<Product> _products;
        private SalesPoint _salesPoint;

        [SetUp]
        public void Setup()
        {
            _salesManager = new SalesManager();

            var product1 = new Product() { Id = 1, Name = "Яблоко", Price = 100 };
            var product2 = new Product() { Id = 2, Name = "Банан", Price = 200 };
            var product3 = new Product() { Id = 3, Name = "Дыня", Price = 300 };

            _products = new List<Product> { product1, product2, product3 };

            _salesPoint = new SalesPoint()
            {
                Name = "Порядочный магазин",
                ProvidedProducts = new List<ProvidedProduct>()
                {
                    new () { Product = product1, ProductQuantity = 100 },
                    new () { Product = product2, ProductQuantity = 100 },
                    new () { Product = product3, ProductQuantity = 100 }
                }
            };
        }

        /// <summary>
        /// Тест на переданный NULL вместо объекта торговой точки
        /// </summary>
        [Test]
        public void TestNullSalesPoint()
        {
            var model = new MakeSale()
            {
                ProductsToBuy = new List<ProductToBuy>() 
                { 
                    new () { ProductId = 1, CountToBuy = 10 },
                    new () { ProductId = 2, CountToBuy = 1 }
                }
            };

            var result = _salesManager.CreateSale(model, null, null);
            Console.WriteLine(result.Message);

            Assert.AreEqual(
                result.Message,
                "Продажа не была создана. Магазин с указанным идентификатором не найден");
        }

        /// <summary>
        /// Тест на переданный идентификатор несуществующего товара
        /// </summary>
        [Test]
        public void TestInvalidProduct()
        {
            var model = new MakeSale()
            {
                ProductsToBuy = new List<ProductToBuy>() { new ProductToBuy() { ProductId = -1, CountToBuy = 10 } }
            };

            var result = _salesManager.CreateSale(model, _salesPoint, null);
            Console.WriteLine(result.Message);

            Assert.AreEqual(
                result.Message,
                "Продажа не была создана. Один из товаров к покупке не был найден в указанной торговой точке. (Id товара = -1)");
        }

        /// <summary>
        /// Тест случая, в котором кол-во требуемых товаров превышает кол-во существующих на складе товаров
        /// </summary>
        [Test]
        public void TestBigCountProducts()
        {
            var model = new MakeSale()
            {
                ProductsToBuy = new List<ProductToBuy>() { new ProductToBuy() { ProductId = 1, CountToBuy = 10000 } }
            };

            var result = _salesManager.CreateSale(model, _salesPoint, null);
            Console.WriteLine(result.Message);

            Assert.AreEqual(
                result.Message,
                "Продажа не была создана. Товаров на складе торговой точки недостаточно для совершения покупки. (Id товара = 1)");
        }

        /// <summary>
        /// Тест случая, в котором все хорошо
        /// </summary>
        [Test]
        public void TestGoodInputData()
        {
            var model = new MakeSale()
            {
                ProductsToBuy = new List<ProductToBuy>() { new ProductToBuy() { ProductId = 1, CountToBuy = 10 } }
            };

            int countBefore = _salesPoint.ProvidedProducts.FirstOrDefault(t => t.Product.Id == 1).ProductQuantity;
            var result = _salesManager.CreateSale(model, _salesPoint, null);
            int countAfter = _salesPoint.ProvidedProducts.FirstOrDefault(t => t.Product.Id == 1).ProductQuantity;

            Console.WriteLine(result.Message);

            if (result.Status == "Ok" && countBefore - 10 == countAfter)
            {
                Assert.Pass();
            }
            else
            {
                Assert.Fail();
            }
        }
    }
}