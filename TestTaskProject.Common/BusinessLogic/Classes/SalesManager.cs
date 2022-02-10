using System;
using System.Collections.Generic;
using System.Linq;
using TestTaskProject.Common.Models;

namespace TestTaskProject.Common.BusinessLogic
{
    public class SalesManager : ISalesManager
    {
        public MakeSaleResult CreateSale(MakeSale model, SalesPoint salePoint, Buyer buyer)
        {
            // Проверки
            {
                if (salePoint is null)
                {
                    return new MakeSaleResult()
                    {
                        Status = "Error",
                        Message = "Продажа не была создана. Магазин с указанным идентификатором не найден"
                    };
                }

                if (model.ProductsToBuy is null || model.ProductsToBuy.Count == 0)
                {
                    return new MakeSaleResult()
                    {
                        Status = "Error",
                        Message = "Продажа не была создана. Не указано ни одного товара к покупке"
                    };
                }

                foreach (var productToBuy in model.ProductsToBuy)
                {
                    var product = salePoint.ProvidedProducts.FirstOrDefault(t => t.Product.Id == productToBuy.ProductId);

                    if (product.ProductQuantity <= 0)
                    {
                        return new MakeSaleResult()
                        {
                            Status = "Error",
                            Message = $"Продажа не была создана. Один из товаров к покупке имеет некорректное количество. (Id товара = {productToBuy.ProductId})"
                        };
                    }

                    if (product is null)
                    {
                        return new MakeSaleResult()
                        {
                            Status = "Error",
                            Message = $"Продажа не была создана. Один из товаров к покупке не был найден в указанной торговой точке. (Id товара = {productToBuy.ProductId})"
                        };
                    }

                    if (product.ProductQuantity < productToBuy.CountToBuy)
                    {
                        return new MakeSaleResult()
                        {
                            Status = "Error",
                            Message = $"Продажа не была создана. Товаров на складе торговой точки недостаточно для совершения покупки. (Id товара = {productToBuy.ProductId})"
                        };
                    }
                }
            }

            // Формирование и последующий возврат продажи
            {
                var saleData = new List<SaleData>();

                foreach (var product in model.ProductsToBuy)
                {
                    var pp = salePoint.ProvidedProducts.FirstOrDefault(t => t.Product.Id == product.ProductId);
                    pp.ProductQuantity -= product.CountToBuy;

                    var saleDataItem = new SaleData()
                    {
                        Product = pp.Product,
                        ProductQuantity = product.CountToBuy
                    };

                    saleData.Add(saleDataItem);
                }

                var sale = new Sale()
                {
                    Buyer = buyer,
                    Date = DateTime.Now,
                    SalesPoint = salePoint,
                    SaleData = saleData
                };

                var result = new MakeSaleResult()
                {
                    Status = "Ok",
                    CreatedSale = sale,
                    Message = "Продажа успешно создана!"
                };

                return result;
            }
        }
    }
}
