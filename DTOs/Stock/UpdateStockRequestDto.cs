using System.ComponentModel.DataAnnotations;

namespace api.DTOs.Stock;

public class UpdateStockRequestDto
{
    [Required]
    [MaxLength(10, ErrorMessage = "Символ не может быть длиннее 10 букв")]
    public string Symbol { get; set; } = string.Empty;
    [Required]
    [MaxLength(10, ErrorMessage = "Название компании не может быть длиннее 10 символов")]
    public string CompanyName { get; set; } = string.Empty;
    [Required]
    [Range(1, 1000000000)]
    public decimal Purchase { get; set; }
    [Required]
    [Range(0.001, 100)]
    public decimal LastDiv { get; set; }
    [Required]
    [MaxLength(10, ErrorMessage = "Индустрия не может содержать более 10 символов")]
    public string Industry { get; set; } = string.Empty;
    [Range(1, 5000000000)]
    public long MarketCap { get; set; }
}