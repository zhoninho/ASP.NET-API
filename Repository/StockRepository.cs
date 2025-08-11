﻿using api.Data;
using api.DTOs.Stock;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository;

public class StockRepository: IStockRepository
{
    private readonly ApplicationDBContext  _context;

    public StockRepository(ApplicationDBContext context)
    {
        _context = context;
    }
    public async Task<List<Stock>> GetAllStocks()
    {
        return await _context.Stocks.ToListAsync();
    }

    public async Task<Stock?> GetStockById(int id)
    {
        return await _context.Stocks.FindAsync(id);
    }

    public async Task<Stock> CreateStock(Stock stockModel)
    {
        await _context.Stocks.AddAsync(stockModel);
        await _context.SaveChangesAsync();
        return stockModel;
    }

    public async Task<Stock?> UpdateStock(int id, UpdateStockRequestDto stockDto)
    {
        var existingStock = await _context.Stocks.FirstOrDefaultAsync(x => x.Id == id);
        if (existingStock == null)
        {
            return null;
        }
        
        existingStock.Symbol = stockDto.Symbol;
        existingStock.CompanyName = stockDto.CompanyName;
        existingStock.Purchase =  stockDto.Purchase;
        existingStock.LastDiv = stockDto.LastDiv;
        existingStock.Industry = stockDto.Industry;
        existingStock.MarketCap = stockDto.MarketCap;
        
        await _context.SaveChangesAsync();
        return existingStock;
    }

    public async Task<Stock?> DeleteStock(int id)
    {
        var stockModel = await _context.Stocks.FirstOrDefaultAsync(s => s.Id == id);

        if (stockModel == null)
        {
            return null;
        }
        
        _context.Stocks.Remove(stockModel);
        await _context.SaveChangesAsync();
        return stockModel;
    }
}