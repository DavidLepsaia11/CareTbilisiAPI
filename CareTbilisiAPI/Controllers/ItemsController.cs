using AutoMapper;
using CareTbilisiAPI.Domain.Interfaces.Repositories;
using CareTbilisiAPI.Domain.Interfaces.Services;
using CareTbilisiAPI.Domain.Models;
using CareTbilisiAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CareTbilisiAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private IItemService _service;
        private IMapper _mapper;

        public ItemsController(IMapper mapper , IItemService service)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        // GET: api/<ItemsController>
        [HttpGet]
        public ActionResult<IEnumerable<ResponseItemModel>> GetAll()
        {
            var allItems = _service.GetAll();

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
            var item = _service.GetById(id);

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
            item.CreateDate = DateTime.Now; 

            var createdModel = _service.Create(item);
            var responseModel = _mapper.Map<ResponseItemModel>(createdModel);

            return CreatedAtAction("Get", new { id = responseModel.Id }, responseModel);
        }

        // Patch api/<ItemsController>/5
        [HttpPatch("{id}")]
        public IActionResult Patch(string id, [FromBody] RequestItemModel requestItemModel)
        {
            var checkeditem = _service.GetById(id);

            if (checkeditem == null)
            {
                return NotFound();
            }

            var UpdateModel = PrepareItemForUpdate(requestItemModel, checkeditem);
            UpdateModel.UpdateDate = DateTime.Now;  

            _service.Update(id, UpdateModel);

            return NoContent();
        }

        // DELETE api/<ItemsController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            var item = _service.GetById(id);

            if (item == null)
            {
                return NotFound();
            }
            _service.Remove(id);
            return NoContent();
        }


        #region Private Methods
        private Item PrepareItemForUpdate(RequestItemModel requestModel, Item model) 
        {
            var modelForUpdate = _mapper.Map<Item>(requestModel);

            Type type = model.GetType();

            PropertyInfo[] properties = type.GetProperties();

            foreach (PropertyInfo property in properties)
            {
                if (property.Name != "Id")
                {
                    var valueUpdateModel = property?.GetValue(modelForUpdate);

                    if (!(valueUpdateModel == null && string.IsNullOrEmpty(valueUpdateModel?.ToString())))
                    {
                        property?.SetValue(model, valueUpdateModel);
                    }
                }
            }
            return model;
        }
        #endregion
    }
}
