using TestTaskProject.Common.Models;
using System.Linq;
using System;
using System.Collections.Generic;

namespace TestTaskProject.WebApi.Data
{
    public class DatabaseInitializer
    {
        public void SeedDatabase(MyDatabaseContext context)
        {
            SeedProducts(context);
            SeedSalePoints(context);
            SeedBuyers(context);
        }

        // Заполнение БД данными о продуктах
        private void SeedProducts(MyDatabaseContext context)
        {
            if (context.Products.Count() > 0)
                return;

            context.Products.Add(new Product() { Name = "Яблоко", Price = 10 });
            context.Products.Add(new Product() { Name = "Груша", Price = 15 });
            context.Products.Add(new Product() { Name = "Арбуз", Price = 80 });

            context.SaveChanges();
        }

        // Заполнение БД данными о торговых точках
        private void SeedSalePoints(MyDatabaseContext context)
        {
            if (context.SalePoints.Count() > 0)
                return;

            var sp = new SalesPoint() { Name = "Порядочный магазин" };
            var pp = new List<ProvidedProduct>();

            foreach (var p in context.Products)
            {
                pp.Add(new ProvidedProduct() { Product = p, ProductQuantity = 1000 });
            }

            sp.ProvidedProducts = pp;

            context.SalePoints.Add(sp);
            context.SaveChanges();
        }

        // Заполнение БД данными о покупателях
        private void SeedBuyers(MyDatabaseContext context)
        {
            if (context.Buyers.Count() > 0)
                return;

            var buyer1 = new Buyer() { Name = "Покупатель Анатолий" };
            var product1 = context.Products.First();
            var product2 = context.Products.Last();

            var sale = new Sale()
            {
                Buyer = buyer1,
                Date = DateTime.Now,
                SaleData = new List<SaleData>()
                {
                    new SaleData() { Product = product1, ProductQuantity = 10 },
                    new SaleData() { Product = product2, ProductQuantity = 100 }
                }
            };

            buyer1.Sales = new List<Sale>() { sale };

            context.Buyers.Add(buyer1);
            
            context.SaveChanges();
        }
    }
}
