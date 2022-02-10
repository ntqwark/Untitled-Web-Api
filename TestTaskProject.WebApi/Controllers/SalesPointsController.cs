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
    public class SalesPointsController : ControllerBase
    {
        private readonly MyDatabaseContext _context;

        public SalesPointsController(MyDatabaseContext context)
        {
            _context = context;
        }

        // GET: api/SalesPoints
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SalesPoint>>> GetSalesPoints()
        {
            return await _context.SalePoints
                .Include(t => t.ProvidedProducts)
                .ThenInclude(t => t.Product)
                .ToListAsync();
        }

        // GET: api/SalesPoints/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SalesPoint>> GetSalesPoint(int id)
        {
            var salesPoint = await _context.SalePoints
                .Include(t => t.ProvidedProducts)
                .ThenInclude(t => t.Product)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (salesPoint == null)
            {
                return NotFound();
            }

            return salesPoint;
        }

        // PUT: api/SalesPoints/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSalesPoint(int id, SalesPoint salesPoint)
        {
            if (id != salesPoint.Id)
            {
                return BadRequest();
            }

            _context.Entry(salesPoint).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SalesPointExists(id))
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

        // POST: api/SalesPoints
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<SalesPoint>> PostSalesPoint(SalesPoint salesPoint)
        {
            _context.SalePoints.Add(salesPoint);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSalesPoint", new { id = salesPoint.Id }, salesPoint);
        }

        // DELETE: api/SalesPoints/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSalesPoint(int id)
        {
            var salesPoint = await _context.SalePoints.FindAsync(id);
            if (salesPoint == null)
            {
                return NotFound();
            }

            _context.SalePoints.Remove(salesPoint);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SalesPointExists(int id)
        {
            return _context.SalePoints.Any(e => e.Id == id);
        }
    }
}
