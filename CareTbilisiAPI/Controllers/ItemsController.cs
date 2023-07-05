using AutoMapper;
using CareTbilisiAPI.Domain.Enums;
using CareTbilisiAPI.Domain.Interfaces.Repositories;
using System;
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
        private readonly IHostEnvironment _environment;
        private readonly IConfiguration _configuration;

        public ItemsController(IMapper mapper , IItemService service, IHostEnvironment environment, IConfiguration configuration)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _environment = environment ?? throw new ArgumentNullException(nameof(environment)); 
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));   
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

        // GET: api/<ItemsController>/GetFilteredItems
        [HttpPost]
        [Route("GetFilteredItems")]
        public ActionResult<IEnumerable<ResponseItemModel>> GetFilteredItems( [FromBody] ItemFilterModel filterModel)
        {
            filterModel.CreateDate ??= DateTime.MinValue;

            var filteredItems = _service.FilterItemByAttribute(filterModel.CityRegion, filterModel.Category, filterModel.CreateDate, currentPage : 1, pageSize : 6 );

            if (filteredItems.Count() == 0)
            {
                return NotFound("There are no such items");
            }
            return Ok(_mapper.Map<ICollection<ResponseItemModel>>(filteredItems).ToList());
        }

        // GET: api/<ItemsController>/GetSortedItems
        [HttpGet]
        [Route("GetSortedItems")]
        public ActionResult<IEnumerable<ResponseItemModel>> GetSortedItems( int currentPage = 1, int pageSize = 6 ) 
        {
            var items = _service.SortItemDescByCreateDay(currentPage , pageSize);

            if (items.Count() == 0)
            {
                return NotFound("There are no items");
            }

            return Ok(_mapper.Map<IEnumerable<ResponseItemModel>>(items).ToList());
        }

        // Patch: api/<ItemsController>/UploadPhoto
        [HttpPatch]
        [Route("UploadPhoto")]
        public IActionResult UploadPhoto(string id , IFormFile file)
        {
            if (!_service.Exist(id))
            {
                return NotFound("Not found a such item");
            }
            if (file.Length == 0)
                return BadRequest();

            var item = _service.GetById(id);

            var imageFilePath = SavePhoto(item.Id, file);

              item.PicturePath = imageFilePath;

            _service.Update(id, item);

            return Ok(new
            {
                id,
                photoSize = file.Length
            });
        }

        // Patch: api/<ItemsController>/GetPhoto/1
        [HttpGet]
        [Route("Photo/{id}")]
        public async Task<IActionResult> GetPhoto(string id)
        {
            if (!_service.Exist(id))
                return NotFound("Such item  not found");

            var item = _service.GetById(id);
            var data = await GetPhotoData(item);

            if (!data.Any())
                return NotFound();

            return File(data, "image/jpeg");
        }


        #region Private Methods

        private string SavePhoto( string id,  IFormFile file)
        {
            var imageDirectory = Path.Combine(_environment.ContentRootPath, "Uploads", "Photos", id);
            var imageFilePath = Path.Combine(imageDirectory, file.FileName);

            if (!Directory.Exists(imageDirectory))
                Directory.CreateDirectory(imageDirectory);

            using var stream = file.OpenReadStream();
            stream.Seek(0, SeekOrigin.Begin);

            using var fileStream = System.IO.File.Create(Path.Combine(imageFilePath));
            stream.CopyTo(fileStream);
            fileStream.Flush();

            return imageFilePath;
        }

        private  static async Task<byte[]> GetPhotoData(Item item)
        {
            if (string.IsNullOrEmpty(item.PicturePath) || !System.IO.File.Exists(item.PicturePath))
                return Array.Empty<byte>();

            return await System.IO.File.ReadAllBytesAsync(item.PicturePath);
        }

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
