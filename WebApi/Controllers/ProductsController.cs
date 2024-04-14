using Business.Interfaces;
using Business.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.Validation;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productRepository)
        {
            _productService = productRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductModel>>> GetByFilter([FromQuery] FilterSearchModel filterSearch)
        {
            var products = await _productService.GetByFilterAsync(filterSearch);

            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductModel>> GetById(int Id)
        {
            var product = await _productService.GetByIdAsync(Id);

            if(product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        [HttpGet("categories")]
        public async Task<ActionResult<IEnumerable<ProductCategoryModel>>> GetCategories()
        {
            var categories = await _productService.GetAllProductCategoriesAsync();

            return Ok(categories);
        }

        [HttpPost]
        public async Task<ActionResult> Add([FromBody] ProductModel model)
        {
            string validation = ProductValidation.Validation(model);

            if(validation != null)
            {
                return BadRequest(validation);
            }

            await _productService.AddAsync(model);

            return Ok(model);
        }

        [HttpPost("categories")]
        public async Task<ActionResult> AddCategory([FromBody] ProductCategoryModel model)
        {
            string validation = ProductCategoryValidation.Validation(model);

            if(validation != null)
            {
                return BadRequest(validation);
            }

            await _productService.AddCategoryAsync(model);

            return Ok(model);
        }


        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int Id, [FromBody] ProductModel model)
        {
            var validation = ProductValidation.Validation(model);
            
            if(validation != null)
            {
                return BadRequest(validation);
            }

            await _productService.UpdateAsync(model);

            return Ok();
        }

        [HttpPut("categories/{id}")]
        public async Task<ActionResult> UpdateCategory(int Id, [FromBody] ProductCategoryModel model)
        {
            var validation = ProductCategoryValidation.Validation(model);

            if(validation != null)
            {
                return BadRequest(model);
            }

            await _productService.UpdateCategoryAsync(model);

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int Id)
        {
            if(Id == 0)
            {
                return BadRequest();
            }

            await _productService.DeleteAsync(Id);

            return Ok();
        }

        [HttpDelete("categories/{id}")]
        public async Task<ActionResult> DeleteCategory(int Id)
        {
            if(Id == 0)
            {
                return BadRequest();
            }

            await _productService.RemoveCategoryAsync(Id);

            return Ok();
        }
    }
}
