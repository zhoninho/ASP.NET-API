using api.DTOs.Stock;
using api.Interfaces;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StockController : ControllerBase
{
    private readonly IStockRepository _stockRepository;
    public StockController(IStockRepository stockRepository)
    {
        _stockRepository = stockRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var stocks = await _stockRepository.GetAllAsync();
        var stockDto = stocks.Select(s => s.ToStockDto());
        return Ok(stockDto);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
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
        var stockModel = stockDto.ToStockFromCreateDto();
        await _stockRepository.CreateAsync(stockModel);
        return CreatedAtAction(nameof(GetById), new { id = stockModel.Id }, stockModel.ToStockDto());
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockRequestDto stockDto)
    {
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
        var stockModel = await _stockRepository.DeleteAsync(id);

        if (stockModel == null)
        {
            return NotFound();
        }
            
        return NoContent();
    }
}