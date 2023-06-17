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
            _repository = repository;
        }

        // GET: api/<ItemsController>
        [HttpGet]
        public ActionResult<IEnumerable<Item>> GetAll()
        {
            var allItems = _repository.GetAll();

            if (allItems.Count == 0)
            {
                return NotFound("There are no items");
            }

            return Ok(allItems);
        }

        // GET api/<ItemsController>/5
        [HttpGet("{id}")]
        public ActionResult<Item> Get(string id)
        {
            var item = _repository.GetById(id);

            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        // POST api/<ItemsController>
        [HttpPost]
        public void Post([FromBody] RequestItemModel requestItemModel)
        {
            var item = _mapper.Map<Item>(requestItemModel);

            _repository.Create(item);
        }

        // PUT api/<ItemsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ItemsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
