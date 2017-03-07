using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SimpleList.Models;
using SimpleList.Services;

namespace SimpleList.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class ListItemsController : Controller
    {
        private readonly IListService _service;
        private readonly ILogger<ListItemsController> _logger;

        public ListItemsController(IListService service, ILogger<ListItemsController> logger)
        {
            _service = service;
            _logger = logger;
        }

        // GET api/listitems
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var items = await _service.All();
                return Json(items);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception thrown while getting list items: {ex}");
            }

            return BadRequest();
        }

        // GET api/listitems/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var item = await _service.Get(id);
                if (item == null)
                {
                    return NotFound();
                }

                return Json(item);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception thrown while getting list item: {ex}");
            }

            return BadRequest();
        }

        // POST api/listitems
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]ListItem item)
        {
            try
            {
                var result = await _service.Add(item);
                return Json(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception thrown while creating list item: {ex}");
            }

            return BadRequest();
        }

        // PUT api/listitems/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody]ListItem item)
        {
            try
            {
                if (id != item.Id)
                {
                    return BadRequest();
                }

                await _service.Update(item);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception thrown while updating list item: {ex}");
            }

            return BadRequest();
        }

        // DELETE api/listitems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _service.Remove(id);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception thrown while deleting list item: {ex}");
            }

            return BadRequest();
        }
    }
}
