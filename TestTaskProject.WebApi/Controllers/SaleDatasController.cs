using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestTaskProject.Common.Models;
using TestTaskProject.WebApi.Data;

namespace TestTaskProject.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SaleDatasController : ControllerBase
    {
        private readonly MyDatabaseContext _context;

        public SaleDatasController(MyDatabaseContext context)
        {
            _context = context;
        }

        // GET: api/SaleDatas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SaleData>>> GetSalesData()
        {
            return await _context.SalesData
                .Include(t => t.Product)
                .ToListAsync();
        }

        // GET: api/SaleDatas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SaleData>> GetSaleData(int id)
        {
            var saleData = await _context.SalesData
                .Include(t => t.Product)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (saleData == null)
            {
                return NotFound();
            }

            return saleData;
        }

        // PUT: api/SaleDatas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSaleData(int id, SaleData saleData)
        {
            if (id != saleData.Id)
            {
                return BadRequest();
            }

            _context.Entry(saleData).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SaleDataExists(id))
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

        // POST: api/SaleDatas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<SaleData>> PostSaleData(SaleData saleData)
        {
            _context.SalesData.Add(saleData);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSaleData", new { id = saleData.Id }, saleData);
        }

        // DELETE: api/SaleDatas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSaleData(int id)
        {
            var saleData = await _context.SalesData.FindAsync(id);
            if (saleData == null)
            {
                return NotFound();
            }

            _context.SalesData.Remove(saleData);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SaleDataExists(int id)
        {
            return _context.SalesData.Any(e => e.Id == id);
        }
    }
}
