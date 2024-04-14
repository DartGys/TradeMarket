using Business.Interfaces;
using Business.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.Validation;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReceiptsController : ControllerBase
    {
        private readonly IReceiptService _receiptService;

        public ReceiptsController(IReceiptService receiptService)
        {
            _receiptService = receiptService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReceiptModel>>> Get()
        {
            var receipts = await _receiptService.GetAllAsync();

            return Ok(receipts);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ReceiptModel>> GetById(int Id)
        {
            var receipt = await _receiptService.GetByIdAsync(Id);

            if (receipt == null)
            {
                return NotFound();
            }

            return Ok(receipt);
        }

        [HttpGet("period")]
        public async Task<ActionResult<IEnumerable<ReceiptModel>>> GetByPeriod(DateTime startDate, DateTime endDate)
        {
            var receiptByPeriod = await _receiptService.GetReceiptsByPeriodAsync(startDate, endDate);

            return Ok(receiptByPeriod);
        }

        [HttpGet("{id}/sum")]
        public async Task<ActionResult<decimal>> GetReceiptSum(int Id)
        {
            var receiptSum = await _receiptService.ToPayAsync(Id);

            return Ok(receiptSum);
        }

        [HttpGet("{id}/details")]
        public async Task<ActionResult<IEnumerable<ReceiptDetailModel>>> GetReceiptDetails(int Id)
        {
            var receiptDetails = await _receiptService.GetReceiptDetailsAsync(Id);

            if (receiptDetails == null)
            {
                return NotFound();
            }

            return Ok(receiptDetails);
        }

        [HttpPost]
        public async Task<ActionResult> Add([FromBody] ReceiptModel model)
        {
            var validation = ReceiptValidation.Validation(model);

            if (validation != null)
            {
                return NotFound();
            }

            await _receiptService.AddAsync(model);

            return Ok(model);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int Id, [FromBody] ReceiptModel model)
        {
            var validation = ReceiptValidation.Validation(model);

            if(validation != null)
            {
                return BadRequest(validation);
            }

            await _receiptService.UpdateAsync(model);

            return Ok(model);
        }

        [HttpPut("{id}/products/add/{productId}/{quantity}")]
        public async Task<ActionResult> AddProductToReceipt(int id, int productId, int quantity)
        {
            try
            {
                await _receiptService.AddProductAsync(productId, id, quantity);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }

        [HttpPut("{id}/products/remove/{productId}/{quantity}")]
        public async Task<ActionResult> RemoveProductFromReceipt(int id, int productId, int quantity)
        {
            try
            {
                await _receiptService.RemoveProductAsync(productId, id, quantity);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }

        [HttpPut("{id}/checkout")]
        public async Task<ActionResult> ReceiptCheckOutChange(int id)
        {
            try
            {
                await _receiptService.CheckOutAsync(id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _receiptService.DeleteAsync(id);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }
    }
}
