using api.Data;
using api.DTOs.Stock;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[Route("api/[controller]")]
public class StockController : ControllerBase
{
    private readonly ApplicationDBContext _context;
    public StockController(ApplicationDBContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var stocks = _context.Stocks.ToList()
            .Select(s => s.ToStockDto());
        return Ok(stocks);
    }

    [HttpGet("{id:int}")]
    public IActionResult GetById([FromRoute] int id)
    {
        var stock = _context.Stocks.Find(id);
        if (stock == null)
        {
            return NotFound();
        }
        return Ok(stock.ToStockDto());
    }

    [HttpPost]
    public IActionResult Create([FromBody] CreateStockRequestDto stockDto)
    {
        var stocModel = stockDto.ToStockFromCreateDTO();
        _context.Stocks.Add(stocModel);
        _context.SaveChanges();
        return CreatedAtAction(nameof(GetById), new { id = stocModel.Id }, stocModel.ToStockDto());
    }

    [HttpPut("{id:int}")]
    public IActionResult Update([FromRoute] int id, [FromBody] UpdateStockRequestDto stockDto)
    {
        var stockModel = _context.Stocks.FirstOrDefault(s => s.Id == id);

        if (stockModel == null)
        {
            return NotFound();
        }
        
        stockModel.Symbol = stockDto.Symbol;
        stockModel.CompanyName = stockDto.CompanyName;
        stockModel.Purchase =  stockDto.Purchase;
        stockModel.LastDiv = stockDto.LastDiv;
        stockModel.Industry = stockDto.Industry;
        stockModel.MarketCap = stockDto.MarketCap;
        
        _context.SaveChanges();
        
        return Ok(stockModel.ToStockDto());
    }
    
    [HttpDelete("{id:int}")]
    public IActionResult Delete([FromRoute] int id)
    {
        var stockModel = _context.Stocks.FirstOrDefault(s => s.Id == id);

        if (stockModel == null)
        {
            return NotFound();
        }
            
        _context.Stocks.Remove(stockModel);
            
        _context.SaveChanges();
            
        return NoContent();
    }
}