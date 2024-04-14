using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Business.Interfaces;
using Business.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Validation;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        // GET: api/customers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerModel>>> Get()
        {
            var customers = await _customerService.GetAllAsync();
            return Ok(customers);
        }

        //GET: api/customers/1
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerModel>> GetById(int id)
        {
            var customer = await _customerService.GetByIdAsync(id);

            if(customer == null)
            {
                return NotFound();
            }

            return Ok(customer);
        }
        
        //GET: api/customers/products/1
        [HttpGet("products/{id}")]
        public async Task<ActionResult<CustomerModel>> GetByProductId(int id)
        {
            var customer = await _customerService.GetCustomersByProductIdAsync(id);

            if (customer == null)
            {
                return NotFound();
            }

            return Ok(customer);
        }

        // POST: api/customers
        [HttpPost]
        public async Task<ActionResult> Add([FromBody] CustomerModel value)
        {
            string validation = CustomerValidation.Validation(value);

            if (validation != null)
            {
                return BadRequest(validation);
            }

            await _customerService.AddAsync(value);

            return Ok(value);
        }

        // PUT: api/customers/1
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int Id, [FromBody] CustomerModel value)
        {
            string validation = CustomerValidation.Validation(value);

            if (validation != null)
            {
                return BadRequest(validation);
            }
            
            await _customerService.UpdateAsync(value);

            var updatedCustomer = await _customerService.GetByIdAsync(Id);

            return Ok(updatedCustomer);
        }

        // DELETE: api/customers/1
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            if(id == 0)
            {
                return BadRequest();
            }

            await _customerService.DeleteAsync(id);

            return Ok();
        }
    }
}
