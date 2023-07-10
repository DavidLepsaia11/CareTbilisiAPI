﻿using AutoMapper;
using CareTbilisiAPI.Domain.Enums;
using CareTbilisiAPI.Domain.Interfaces.Repositories;
using System;
using CareTbilisiAPI.Domain.Interfaces.Services;
using CareTbilisiAPI.Domain.Models;
using CareTbilisiAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Cors;


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
       // [Authorize]
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
       // [Authorize]
        [HttpPost]
        public ActionResult<ResponseItemModel> Post([FromBody] RequestItemModel requestItemModel)
        {

            //  var item = _mapper.Map<Item>(requestItemModel);

            var item = new Item()
            {
                CreateDate = DateTime.Now,
                UserId = HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                Status = StatusEnum.NotStarted,
                Category = (ProblemTypeEnum)GetEnumKey(requestItemModel.Category, nameof(ProblemTypeEnum)),
                CityRegion = (CityRegionEnum)GetEnumKey(requestItemModel.CityRegion, nameof(CityRegionEnum)),
                Description = requestItemModel.Description,
                Location = requestItemModel.Location,
                Comments = requestItemModel?.Comments
            };


            var createdModel = _service.Create(item);

            var responseModel = _mapper.Map<ResponseItemModel>(createdModel);

            return CreatedAtAction("Get", new { id = responseModel.Id }, responseModel);
        }

        // Patch api/<ItemsController>/5
       // [Authorize]
        [HttpPatch("{id}")]
        public IActionResult Patch(string id, [FromBody] RequestPatchItemModel requestItemModel)
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

        // Patch api/<ItemsController>/UpdateStatus?id=648ed42a9419eac146fe1516&status=1
      //  [Authorize]
        [HttpPatch]
        [Route("UpdateStatus")]
        public IActionResult UpdateStatus(string id, string status)
        {
            var checkeditem = _service.GetById(id);

            if (checkeditem == null)
            {
                return NotFound();
            }

            int statusKey = GetEnumKey(status, nameof(StatusEnum));
            checkeditem.Status = statusKey == -1 ? null : (StatusEnum) statusKey;
            checkeditem.UpdateDate = DateTime.Now;

            _service.Update(id, checkeditem);

            return NoContent();
        }

        // DELETE api/<ItemsController>/5
        [Authorize]
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
        //[Authorize]
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

            int cityRegionKey = GetEnumKey(filterModel.CityRegion, "CityRegionEnum");
            int categoryKey = GetEnumKey(filterModel.Category, "ProblemTypeEnum");

            CityRegionEnum? cityRegionEnum = cityRegionKey == -1 ? null : (CityRegionEnum)cityRegionKey;
            ProblemTypeEnum? categoryEnum = categoryKey == -1 ? null : (ProblemTypeEnum)categoryKey;

            var filteredItems = _service.FilterItemByAttribute(cityRegionEnum, categoryEnum, filterModel.CreateDate, currentPage : 1, pageSize : 6 );

            if (filteredItems.Count() == 0)
            {
                return NotFound("There are no such items");
            }
            return Ok(_mapper.Map<ICollection<ResponseItemModel>>(filteredItems).ToList());
        }

        // GET: api/<ItemsController>/GetSortedItems
        [EnableCors("CorsPolicy")]
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

        // GET: api/<ItemsController>/GetItemsByUserId
      //  [Authorize]
        [HttpGet]
        [Route("GetItemsByUserId")]
        public ActionResult<IEnumerable<ResponseItemModel>> filterItemsByUserId(string userId, int currentPage = 1, int pageSize = 6)
        {
            var items = _service.filterItemsByUserId(userId, currentPage, pageSize);

            if (items.Count() == 0)
            {
                return NotFound("There are no added items via user");
            }

            return Ok(_mapper.Map<IEnumerable<ResponseItemModel>>(items).ToList());
        }

        // Patch: api/<ItemsController>/UploadPhoto
        [Authorize]
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
        [Authorize]
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

        private int GetEnumKey(string? descriptionEnum, string enumType) 
        {
            string replacedDesc = string.Empty;
            int enumKey = -1;

            if (descriptionEnum == null)
            {
                return enumKey;
            }

            try
            {
                if (descriptionEnum.Contains(" "))
                {
                     replacedDesc = descriptionEnum.Replace(" ", "_");
                }

                if (enumType == "CityRegionEnum")
                {
                    var enumValue = (CityRegionEnum)Enum.Parse(typeof(CityRegionEnum), replacedDesc);
                    enumKey = (int)enumValue;
                }
                else if(enumType == "ProblemTypeEnum")
                {
                    var enumValue = !string.IsNullOrEmpty(replacedDesc) ? (ProblemTypeEnum)Enum.Parse(typeof(ProblemTypeEnum), replacedDesc) : (ProblemTypeEnum)Enum.Parse(typeof(ProblemTypeEnum), descriptionEnum);
                    enumKey = (int)enumValue;
                }
                else if (enumType == "StatusEnum")
                {
                    var enumValue = (StatusEnum)Enum.Parse(typeof(StatusEnum), descriptionEnum);
                    enumKey = (int)enumValue;
                }
            }
            catch (Exception)
            {
                return enumKey;
            }

            return enumKey;
        }

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

        private Item PrepareItemForUpdate(RequestPatchItemModel requestModel, Item model) 
        {
            int categoryKey = GetEnumKey(requestModel.Category, nameof(ProblemTypeEnum));
            int cityRegionKey = GetEnumKey(requestModel.CityRegion, nameof(CityRegionEnum));


            var modelForUpdate = new Item()
            {
                Category = categoryKey == -1 ? null : (ProblemTypeEnum) categoryKey,
                CityRegion = cityRegionKey == -1 ? null : (CityRegionEnum)cityRegionKey,
                Description = requestModel.Description,
                Location = requestModel.Location,
                Comments = requestModel?.Comments
            }; 

            /*_mapper.Map<Item>(requestModel);*/


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
