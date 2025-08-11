using api.Models;
using api.DTOs.Stock;

namespace api.Interfaces;

public interface IStockRepository
{
    Task<List<Stock>> GetAllStocks();
    Task<Stock?> GetStockById(int id);
    Task<Stock> CreateStock(Stock stockModel);
    Task<Stock?> UpdateStock(int id, UpdateStockRequestDto stockDto);
    Task<Stock?> DeleteStock(int id);
}