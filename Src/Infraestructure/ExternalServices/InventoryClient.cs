namespace prismodPurchase.Src.Infraestructure.ExternalServices;

public class StockValidationItemDto
{
    public string ProductCen { get; set; } = string.Empty;
    public decimal Quantity { get; set; }
}

public class StockValidationRequestDto
{
    public string WarehouseCen { get; set; } = string.Empty;
    public string? Source { get; set; }
    public string? ReferenceCen { get; set; }
    public List<StockValidationItemDto> Items { get; set; } = new();
}

public interface IInventoryClient
{
    Task<bool> IncreaseStockAsync(string companyCen, StockValidationRequestDto request);
}

public class InventoryClient : IInventoryClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<InventoryClient> _logger;

    public InventoryClient(HttpClient httpClient, ILogger<InventoryClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<bool> IncreaseStockAsync(string companyCen, StockValidationRequestDto request)
    {
        var response = await _httpClient.PostAsJsonAsync($"/api/inventory/companies/{companyCen}/stock/increase", request);
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            _logger.LogError("Failed to increase stock in Inventory API: {Error}", error);
            return false;
        }
        return true;
    }
}
