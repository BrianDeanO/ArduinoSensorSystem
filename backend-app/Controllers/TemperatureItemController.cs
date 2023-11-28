using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using backEndApp.Models;

namespace backEndApp.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class TemperatureItemController : ControllerBase {

        private readonly TemperatureItemContext _context;
        
        public TemperatureItemController(TemperatureItemContext context) {
            _context = context;
        }

        // GET: api/TemperatureItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TemperatureItemDTO>>> GetTemperatureItems() {
            return await _context.TemperatureItems
                .Select(x => TemperatureToDTO(x))
                .ToListAsync();
        }

        // GET: api/TemperatureItems/5
        // <snippet_GetByID>
        // This value of is is from the web browser's URL, and then is 
        //      inputted into the function via the (long id) parameter to get the id
        [HttpGet("{id}")]
        public async Task<ActionResult<TemperatureItemDTO>> GetTemperatureItem(long id) {
            var temperatureItem = await _context.TemperatureItems.FindAsync(id);

            if (temperatureItem == null) {
                return NotFound();
            }

            return TemperatureToDTO(temperatureItem);
        }
        // </snippet_GetByID>

        // PUT: api/TemperatureItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        // <snippet_Update>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTemperatureItem(long id, TemperatureItem temperatureDTO) {
            if (id != temperatureDTO.TemperatureID) {
                return BadRequest();
            }

            var temperatureItem = await _context.TemperatureItems.FindAsync(id);
            if (temperatureItem == null) {
                return NotFound();
            }

            temperatureItem.TemperatureValue = temperatureDTO.TemperatureValue;
            temperatureItem.TemperatureUnit = temperatureDTO.TemperatureUnit;
            temperatureItem.TemperatureTime = temperatureDTO.TemperatureTime;

            try {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!TemperatureItemExists(id)) {
                return NotFound();
            }

            return NoContent();
        }
        // <snippet_Update>

        // POST: api/TemperatureItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        // <snippet_Create>
        [HttpPost]
        public async Task<ActionResult<TemperatureItem>> PostTemperatureItem(TemperatureItem temperatureDTO) {
            var temperatureItem = new TemperatureItem {
                TemperatureValue = temperatureDTO.TemperatureValue,
                TemperatureUnit = temperatureDTO.TemperatureUnit,
                TemperatureTime = temperatureDTO.TemperatureTime,
            };

            _context.TemperatureItems.Add(temperatureItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetTemperatureItem),
                new { id = temperatureItem.TemperatureID },
                TemperatureToDTO(temperatureItem));
        }
        // <snippet_Create>

        // DELETE: api/TemperatureItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTemperatureItem(long id) {
            var temperatureItem = await _context.TemperatureItems.FindAsync(id);
            if (temperatureItem == null)
            {
                return NotFound();
            }

            _context.TemperatureItems.Remove(temperatureItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TemperatureItemExists(long id) {
            return _context.TemperatureItems.Any(e => e.TemperatureID == id);
        }

        private static TemperatureItemDTO TemperatureToDTO(TemperatureItem temperatureItem) => new TemperatureItemDTO {
            TemperatureID = temperatureItem.TemperatureID,
            TemperatureValue = temperatureItem.TemperatureValue,
            TemperatureUnit = temperatureItem.TemperatureUnit,
            TemperatureTime = temperatureItem.TemperatureTime,
        };
    }
}
