using api.DTOs.Stock;
using api.Helpers;
using api.Interfaces;
using api.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class StockController : ControllerBase
{
    private readonly IStockRepository _stockRepository;
    public StockController(IStockRepository stockRepository)
    {
        _stockRepository = stockRepository;
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAll([FromQuery] QueryObject query)
    {
        if(!ModelState.IsValid)
            return BadRequest(ModelState);

        var stocks = await _stockRepository.GetAllAsync(query);
        
        var stockDto = stocks.Select(s => s.ToStockDto());
        
        return Ok(stockDto);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        if(!ModelState.IsValid)
            return BadRequest(ModelState);

        var stockModel = await _stockRepository.GetByIdAsync(id);
        
        if (stockModel == null)
        {
            return NotFound();
        }
        
        return Ok(stockModel.ToStockDto());
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateStockRequestDto stockDto)
    {
        if(!ModelState.IsValid)
            return BadRequest(ModelState);

        var stockModel = stockDto.ToStockFromCreateDto();
        
        await _stockRepository.CreateAsync(stockModel);
        
        return CreatedAtAction(nameof(GetById), new { id = stockModel.Id }, stockModel.ToStockDto());
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockRequestDto stockDto)
    {
        if(!ModelState.IsValid)
            return BadRequest(ModelState);

        var stockModel = await _stockRepository.UpdateAsync(id, stockDto);
        
        if (stockModel == null)
        {
            return NotFound();
        }
        
        return Ok(stockModel.ToStockDto());
    }
    
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        if(!ModelState.IsValid)
            return BadRequest(ModelState);

        var stockModel = await _stockRepository.DeleteAsync(id);

        if (stockModel == null)
        {
            return NotFound();
        }
            
        return NoContent();
    }
}