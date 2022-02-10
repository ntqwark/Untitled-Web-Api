using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestTaskProject.Common.BusinessLogic;
using TestTaskProject.Common.Models;
using TestTaskProject.WebApi.Data;

namespace TestTaskProject.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesController : ControllerBase
    {
        private readonly MyDatabaseContext _context;
        private readonly ISalesManager _salesManager;

        public SalesController(MyDatabaseContext context, ISalesManager salesManager)
        {
            _context = context;
            _salesManager = salesManager;
        }


        // Web API, реализующий бизнес-логику, указанную В ТЗ
        [Route("MakeSale")]
        [HttpPost]
        public async Task<MakeSaleResult> PostMakeSale([FromBody] MakeSale model)
        {
            // Запрос данных
            var buyer = await _context.Buyers
                .Include(t => t.Sales)
                .FirstOrDefaultAsync(t => t.Id == model.BuyerId);

            var salePoint = await _context.SalePoints
                .Include(t => t.ProvidedProducts)
                .ThenInclude(t => t.Product)
                .FirstOrDefaultAsync(t => t.Id == model.SalePointId);

            // Выполнение бизнес логики
            var result = _salesManager.CreateSale(model, salePoint, buyer);

            // Обработка результатов
            if (result.CreatedSale != null)
            {
                _context.Sales.Add(result.CreatedSale);
                await _context.SaveChangesAsync();
            }

            return result;
        }

        // GET: api/Sales
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Sale>>> GetSales()
        {
            var sales = await _context.Sales
                .Include(t => t.SalesPoint)
                .Include(t => t.Buyer)
                .Include(t => t.SaleData)
                .Include("SalesData.Product")
                .ToListAsync();

            // Избавление от рекурсивной вложенности
            sales.ForEach(salesItem => salesItem.Buyer.Sales = null);

            foreach (var salesItem in sales)
            {
                foreach (var t in salesItem.SaleData)
                {
                    // t.Product = _context.Products.Find()
                }
            }

            return sales;
        }

        // GET: api/Sales/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Sale>> GetSale(int id)
        {
            var sale = await _context.Sales.
                Include(t => t.SalesPoint)
                .Include(t => t.Buyer)
                .Include(t => t.SaleData)
                .Include("SalesData.Product")
                .FirstOrDefaultAsync(t => t.Id == id);

            // Избавление от рекурсивной вложенности
            sale.Buyer.Sales = null;

            if (sale == null)
            {
                return NotFound();
            }

            return sale;
        }

        // PUT: api/Sales/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSale(int id, Sale sale)
        {
            if (id != sale.Id)
            {
                return BadRequest();
            }

            _context.Entry(sale).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SaleExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Sales
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Sale>> PostSale(Sale sale)
        {
            _context.Sales.Add(sale);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSale", new { id = sale.Id }, sale);
        }

        // DELETE: api/Sales/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSale(int id)
        {
            var sale = await _context.Sales.FindAsync(id);
            if (sale == null)
            {
                return NotFound();
            }

            _context.Sales.Remove(sale);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SaleExists(int id)
        {
            return _context.Sales.Any(e => e.Id == id);
        }
    }
}
