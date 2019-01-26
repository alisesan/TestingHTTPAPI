using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FooApi.Models;

namespace FooApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize("mypolicy")]
    public class FooController: Controller
    {
        private readonly FooContext _context;

        public FooController(FooContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Get(int id)
        {
            var foo = await _context.Foo.FindAsync(id);

            if (foo != null)
            {
                return Ok(foo);
            }

            return NotFound();
        }

        [HttpPost("")]
        public async Task<ActionResult> Post(Foo foo)
        {
            await _context.Foo.AddAsync(foo);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(FooController.Get), new {id = foo.Id});
        }
    }
}
