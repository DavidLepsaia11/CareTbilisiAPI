using AutoMapper;
using CareTbilisiAPI.Domain.Interfaces.Repositories;
using CareTbilisiAPI.Domain.Models;
using CareTbilisiAPI.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CareTbilisiAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private IItemRepository _repository;
        private IMapper _mapper;

        public ItemsController(IMapper mapper , IItemRepository repository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        // GET: api/<ItemsController>
        [HttpGet]
        public ActionResult<IEnumerable<ResponseItemModel>> GetAll()
        {
            var allItems = _repository.GetAll();

            if (allItems.Count == 0)
            {
                return NotFound("There are no items");
            }
            return Ok(_mapper.Map<ICollection<ResponseItemModel>>(allItems).ToList());
        }

        // GET api/<ItemsController>/5
        [HttpGet("{id}")]
        public ActionResult<ResponseItemModel> Get(string id)
        {
            var item = _repository.GetById(id);

            if (item == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<ResponseItemModel>(item));
        }

        // POST api/<ItemsController>
        [HttpPost]
        public ActionResult<ResponseItemModel> Post([FromBody] RequestItemModel requestItemModel)
        {
            var item = _mapper.Map<Item>(requestItemModel);
            var createdItem = _repository.Create(item);
            var responseModel = _mapper.Map<ResponseItemModel>(createdItem);

            return CreatedAtAction("Get", responseModel);
        }

        // Patch api/<ItemsController>/5
        [HttpPatch("{id}")]
        public IActionResult Patch(string id, [FromBody] RequestItemModel requestItemModel)
        {
            var item = _repository.GetById(id);

            if (item == null)
            {
                return NotFound();
            }
            item = _mapper.Map<Item>(requestItemModel);
            _repository.UpdateByField(id, item);

            return NoContent();
        }

        // DELETE api/<ItemsController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            var item = _repository.GetById(id);

            if (item == null)
            {
                return NotFound();
            }
            _repository.Remove(id);
            return NoContent();
        }
    }
}
